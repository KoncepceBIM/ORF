using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace ORF.Entities
{
    public class Project : IfcRootWrapper<IIfcProject>
    {
        private IIfcSite _site;
        internal IIfcSite Site => _site ?? (_site = Create.Site(s => {
            s.Name = Entity.Name;
            Create.RelAggregates(r => {
                r.RelatedObjects.Add(s);
                r.RelatingObject = Entity;
            });
        }));
        internal Project(IIfcProject project, bool init) : base(project)
        {
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

        public IfcCompoundPlaneAngleMeasure? Latitude { get => _site?.RefLatitude; set => Site.RefLatitude = value; }
        public IfcCompoundPlaneAngleMeasure? Longitude { get => _site?.RefLongitude; set => Site.RefLongitude = value; }
        public IIfcPostalAddress Address { get => _site?.SiteAddress; set => Site.SiteAddress = value; }

        public string LongName { get => Entity.LongName; set => Entity.LongName = value; }
    }
}
