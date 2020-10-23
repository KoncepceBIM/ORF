using ORF.Entities;
using ORF.Validation;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Xbim.Common;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.IO;

namespace ORF
{
    public sealed class CostModel: IDisposable
    {
        public IModel IFC { get; private set; }
        public Create Create { get; }

        public CostModel(string path, XbimEditorCredentials credentials = null)
        {
            var ext = Path.GetExtension(path);
            if (ext.EndsWith(".orf", StringComparison.OrdinalIgnoreCase))
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

        public CostModel(XbimEditorCredentials credentials, string projectName)
        {
            IfcStore.ModelProviderFactory.UseMemoryModelProvider();
            IFC = IfcStore.Create(credentials, Xbim.Common.Step21.XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel);
            Create = new Create(IFC);
            using (var txn = IFC.BeginTransaction("Project creation"))
            {
                CreateProject(projectName);
                txn.Commit();
            }
        }

        public Project Project { get; private set; }
        private IIfcRelDeclares _declarations;
        private IIfcRelDeclares Declarations => _declarations ?? 
            (_declarations = IFC.Instances.New<IfcRelDeclares>(r => r.RelatingContext = Project.Entity as IfcProject));

        private readonly HashSet<CostSchedule> _schedules = new HashSet<CostSchedule>();
        public IEnumerable<CostSchedule> Schedules => _schedules.ToList().AsReadOnly();

        public CostSchedule CreateSchedule(string name)
        {
            var native = Create.CostSchedule(s => s.Name = name);
            var schedule = new CostSchedule(native);
            if (IFC.SchemaVersion != Xbim.Common.Step21.XbimSchemaVersion.Ifc2X3)
                Declarations.RelatedDefinitions.Add(native);

            _schedules.Add(schedule);

            return schedule;
        }

        private void CreateProject(string projectName)
        {
            // there should only be one project in the model
            if (Project != null)
                return;

            Project = new Project(Create.Project(p => {
                p.UnitsInContext = Create.UnitAssignment();
                p.Name = projectName;
            }));
        }

        public ITransaction BeginTransaction() => IFC.BeginTransaction("Modifications");

        public void SaveAsIfc(string path)
        {
            path = Path.ChangeExtension(path, ".ifc");
            ((IfcStore)IFC).SaveAs(path);
        }

        public void SaveAsIfcXml(string path)
        {
            path = Path.ChangeExtension(path, ".ifcxml");
            ((IfcStore)IFC).SaveAs(path);
        }

        public void SaveAsORF(string path)
        {
            path = Path.ChangeExtension(path, ".orf");
            using (var file = File.Create(path))
            {
                using (var archive = new ZipArchive(file, ZipArchiveMode.Create))
                {
                    var name = Path.GetFileNameWithoutExtension(path) + ".ifc";
                    var entry = archive.CreateEntry(name);
                    using (var stream = entry.Open())
                    {
                        ((IfcStore)IFC).SaveAsIfc(stream);
                    }
                }
            }
        }

        public bool IsValid(out IEnumerable<ValidationResult> errors)
        {
            var validator = new ModelValidator();
            var result = validator.Check(IFC);
            errors = validator.Errors;
            return result;
        }

        public void Dispose()
        {
            IFC.Dispose();
            IFC = null;
        }
    }
}
