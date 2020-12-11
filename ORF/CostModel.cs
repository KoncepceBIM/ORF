using ORF.Entities;
using ORF.Validation;
using System;
using System.Collections;
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
    public sealed class CostModel : IDisposable
    {
        private IModel _internalModel;

        public IModel IFC { get => _internalModel; private set { _internalModel = value; if (_internalModel != null) _internalModel.Tag = this; }  }

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
                    var entry = arch.Entries.FirstOrDefault(e =>
                    {
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

            // optimization for inverse relationships
            using (IFC.BeginInverseCaching())
            {
                Classifications = new ClassificationCollection(this);
            }

            Create = new Create(IFC);
            var project = IFC.Instances.FirstOrDefault<IIfcProject>();
            if (project != null)
            {
                // optimization for inverse relationships
                using (IFC.BeginInverseCaching())
                {
                    Project = new Project(project, true);
                }
            }
            else
            {
                CreateProject("Default");
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

            Classifications = new ClassificationCollection(this);
        }

        public ClassificationCollection Classifications { get; }

        public Project Project { get; private set; }


        private void CreateProject(string projectName)
        {
            // there should only be one project in the model
            if (Project != null)
                return;

            Project = new Project(Create.Project(p =>
            {
                p.UnitsInContext = Create.UnitAssignment();
                p.Name = projectName;
            }), false);
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

    public class ClassificationCollection : ICollection<Classification>
    {
        private readonly HashSet<Classification> _inner = new HashSet<Classification>();
        private readonly CostModel model;

        public ClassificationCollection(CostModel model)
        {
            this.model = model;

            _inner = new HashSet<Classification>(model.IFC.Instances.OfType<IIfcClassification>()
                .Select(s => new Classification(s, true)));
        }

        public int Count => _inner.Count;

        public bool IsReadOnly => false;

        public void Add(Classification item)
        {
            if (!_inner.Add(item))
                return;
        }

        public void Clear()
        {
            foreach (var c in _inner)
            {
                    model.IFC.Delete(c.Entity);
            }
            _inner.Clear();
        }

        public bool Contains(Classification item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(Classification[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Classification> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public bool Remove(Classification item)
        {
            model.IFC.Delete(item.Entity);
            return _inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}
