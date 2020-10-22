using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class Project : IfcRootWrapper<IIfcProject>
    {
        public Project(IIfcProject project) : base(project)
        {
        }

        public IList<IIfcUnit> Units => Entity?.UnitsInContext?.Units;

        public IIfcNamedUnit AreaUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.AREAUNIT); }
        public IIfcNamedUnit LengthUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.LENGTHUNIT); }
        public IIfcNamedUnit TimeUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.TIMEUNIT); }
        public IIfcNamedUnit VolumeUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.VOLUMEUNIT); }
        public IIfcNamedUnit WeightUnit { get => Units.OfType<IIfcNamedUnit>().FirstOrDefault(u => u.UnitType == IfcUnitEnum.MASSUNIT); }
    }
}
