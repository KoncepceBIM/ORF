using ORF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.IO;
using Xbim.IO.Memory;

namespace ORF
{
    public sealed class Model: IDisposable
    {
        public IModel IFC { get; private set; }
        private Create Create { get; }

        public Model(string path, XbimEditorCredentials credentials = null)
        {
            var ext = Path.GetExtension(path);
            if (ext.EndsWith("orf", StringComparison.OrdinalIgnoreCase))
            {
                using (var file = File.OpenRead(path))
                using (var arch = new ZipArchive(file, ZipArchiveMode.Read))
                {
                    var type = StorageType.Invalid;
                    var entry = arch.Entries.FirstOrDefault(e => { 
                        if (e.Name.EndsWith(".ifc", StringComparison.OrdinalIgnoreCase))
                        {
                            type = StorageType.Ifc;
                            return true;
                        }
                        if (e.Name.EndsWith(".ifcxml", StringComparison.OrdinalIgnoreCase))
                        {
                            type = StorageType.IfcXml;
                            return true;
                        }
                        return false;
                    });
                    if (entry == null || type == StorageType.Invalid)
                        throw new NotSupportedException("File doesn't contain any IFC file");

                    using (var stream = entry.Open())
                    {
                        IFC = IfcStore.Open(stream, type, Xbim.Common.Step21.XbimSchemaVersion.Ifc4, XbimModelType.MemoryModel, credentials);
                    }
                }
            }
            else
            { 
                IFC = IfcStore.Open(path, credentials);
            }

            Create = new Create(IFC);
            var project = IFC.Instances.FirstOrDefault<IIfcProject>();
            if (project != null)
            { 
                Project = new Project(project);
                _declarations = Project.Entity.Declares.FirstOrDefault();
            }

            // optimization for inverse relationships
            using (IFC.BeginInverseCaching())
            {
                _schedules = new HashSet<CostSchedule>(IFC.Instances.OfType<IIfcCostSchedule>().Select(s => new CostSchedule(s)));
            }
        }

        public Model(XbimEditorCredentials credentials = null)
        {
            IfcStore.ModelProviderFactory.UseMemoryModelProvider();
            IFC = IfcStore.Create(credentials, Xbim.Common.Step21.XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel);
            Create = new Create(IFC);
            using (var txn = IFC.BeginTransaction("Project creation"))
            {
                CreateProject();
            }
        }

        public Project Project { get; private set; }
        private IIfcRelDeclares _declarations;
        private IIfcRelDeclares Declarations => _declarations ?? 
            (_declarations = IFC.Instances.New<IfcRelDeclares>(r => r.RelatingContext = Project.Entity as IfcProject));

        private readonly HashSet<CostSchedule> _schedules = new HashSet<CostSchedule>();
        public IEnumerable<CostSchedule> Schedules => _schedules.ToList().AsReadOnly();

        public CostSchedule CreateSchedule()
        {
            var native = Create.CostSchedule();
            var schedule = new CostSchedule(native);
            Declarations.RelatedDefinitions.Add(native);

            _schedules.Add(schedule);

            return schedule;
        }

        public CostItem CreateCostItem()
        {
            return new CostItem(Create.CostItem());
        }

        public CostSystem CreateCostSystem()
        {
            return new CostSystem(Create.DocumentReference());
        }

        private void CreateProject()
        {
            Project = new Project(Create.Project(p => p.UnitsInContext = Create.UnitAssignment()));
        }

        public ITransaction BeginTransaction => IFC.BeginTransaction("Modifications");

        public void Dispose()
        {
            IFC.Dispose();
            IFC = null;
        }
    }
}
