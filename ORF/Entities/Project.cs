using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;

namespace ORF.Entities
{
    public class Project : IfcRootWrapper<IIfcProject>
    {
        private IIfcSite _site;
        internal IIfcSite Site => _site ?? (_site = Create.Site(s =>
        {
            s.Name = Entity.Name;
            Create.RelAggregates(r =>
            {
                r.RelatedObjects.Add(s);
                r.RelatingObject = Entity;
            });
        }));
        internal Project(IIfcProject project, bool init) : base(project)
        {
            CostSchedules = new CostSchedulesCollection(this, init);
            ClassificationItems = new ClassificationCollection(Entity, init);

            if (!init)
                return;

            var rel = project.IsDecomposedBy.FirstOrDefault(r => r.RelatedObjects.Any(o => o is IIfcSite));
            _site = rel?.RelatedObjects.OfType<IIfcSite>().FirstOrDefault();
        }

        public IList<IIfcUnit> Units => Entity?.UnitsInContext?.Units;

        public IIfcNamedUnit AreaUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.AREAUNIT); }
        public IIfcNamedUnit LengthUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.LENGTHUNIT); }
        public IIfcNamedUnit TimeUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.TIMEUNIT); }
        public IIfcNamedUnit VolumeUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.VOLUMEUNIT); }
        public IIfcNamedUnit WeightUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.MASSUNIT); }
        public IIfcMonetaryUnit MonetaryUnit { get => Units.OfType<IIfcMonetaryUnit>().FirstOrDefault(); }

        public List<long> Latitude { get => _site?.RefLatitude; set => Site.RefLatitude = value; }
        public List<long> Longitude { get => _site?.RefLongitude; set => Site.RefLongitude = value; }
        public IIfcPostalAddress Address { get => _site?.SiteAddress; set => Site.SiteAddress = value; }

        public string LongName { get => Entity.LongName; set => Entity.LongName = value; }

        public CostSchedulesCollection CostSchedules { get; }

        public ClassificationCollection ClassificationItems { get; }
    }

    public class CostSchedulesCollection : ICollection<CostSchedule>
    {
        private readonly HashSet<CostSchedule> schedules = new HashSet<CostSchedule>();
        private readonly IList<IIfcRelDeclares> relations = new List<IIfcRelDeclares>();
        private readonly IEntityCollection create;
        private readonly Project project;

        internal CostSchedulesCollection(Project project, bool init)
        {
            create = project.Entity.Model.Instances;
            this.project = project;

            if (!init)
                return;

            var rels = project.Entity.Declares.ToList();
            var schedules = rels.SelectMany(r => r.RelatedDefinitions)
                .OfType<IIfcCostSchedule>()
                .Select(i => new CostSchedule(i, init));

            if (project.Entity.Model.SchemaVersion == Xbim.Common.Step21.XbimSchemaVersion.Ifc2X3)
                schedules = project.Entity.Model.Instances.OfType<IIfcCostSchedule>()
                    .Select(i => new CostSchedule(i, init));

            this.schedules = new HashSet<CostSchedule>(schedules);
            relations = rels;
        }

        public int Count => schedules.Count;

        public bool IsReadOnly => false;

        public void Add(CostSchedule item)
        {
            if (!schedules.Add(item))
                return;

            if (relations.Any())
            {
                relations[0].RelatedDefinitions.Add(item.Entity);
                return;
            }

            if (project.Entity.Model.SchemaVersion == Xbim.Common.Step21.XbimSchemaVersion.Ifc2X3)
                return;

            var rel = create.New<IfcRelDeclares>(r => r.RelatingContext = project.Entity as IfcProject);
            relations.Add(rel);
            rel.RelatedDefinitions.Add(item.Entity);
        }

        public void Clear()
        {
            var childrenIds = new HashSet<int>(schedules.Select(c => c.Entity.EntityLabel));
            foreach (var rel in relations)
            {
                foreach (var toRemove in rel.RelatedDefinitions.Where(d => childrenIds.Contains(d.EntityLabel)))
                {
                    rel.RelatedDefinitions.Remove(toRemove);
                }
                if (!rel.RelatedDefinitions.Any())
                {
                    rel.Model.Delete(rel);
                    relations.Remove(rel);
                }
            }
            schedules.Clear();
        }

        public bool Contains(CostSchedule item)
        {
            return schedules.Contains(item);
        }

        public void CopyTo(CostSchedule[] array, int arrayIndex)
        {
            schedules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CostSchedule> GetEnumerator()
        {
            return schedules.GetEnumerator();
        }

        public bool Remove(CostSchedule item)
        {
            if (!schedules.Remove(item))
                return false;

            foreach (var rel in relations)
            {
                rel.RelatedDefinitions.Remove(item.Entity);
                if (!rel.RelatedDefinitions.Any())
                {
                    rel.Model.Delete(rel);
                    relations.Remove(rel);
                }
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
