using System;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class Quantity : IfcWrapper<IIfcPhysicalSimpleQuantity>, IEntity
    {
        internal Quantity(IIfcPhysicalSimpleQuantity entity) : base(entity)
        {
            if (entity is IIfcQuantityArea) Type = QuantityTypeEnum.Area;
            if (entity is IIfcQuantityCount) Type = QuantityTypeEnum.Count;
            if (entity is IIfcQuantityLength) Type = QuantityTypeEnum.Length;
            if (entity is IIfcQuantityTime) Type = QuantityTypeEnum.Time;
            if (entity is IIfcQuantityVolume) Type = QuantityTypeEnum.Volume;
            if (entity is IIfcQuantityWeight) Type = QuantityTypeEnum.Weight;
        }

        public QuantityTypeEnum Type { get; }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }

        public IIfcNamedUnit Unit
        {
            set 
            {
                if (value == null)
                {
                    Entity.Unit = value;
                    return;
                }

                // check unit compatibility
                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        if (value.UnitType != IfcUnitEnum.AREAUNIT) throw new ArgumentException("Incompatible unit type");
                        break;
                    case QuantityTypeEnum.Length:
                        if (value.UnitType != IfcUnitEnum.LENGTHUNIT) throw new ArgumentException("Incompatible unit type");
                        break;
                    case QuantityTypeEnum.Time:
                        if (value.UnitType != IfcUnitEnum.TIMEUNIT) throw new ArgumentException("Incompatible unit type");
                        break;
                    case QuantityTypeEnum.Volume:
                        if (value.UnitType != IfcUnitEnum.VOLUMEUNIT) throw new ArgumentException("Incompatible unit type");
                        break;
                    case QuantityTypeEnum.Weight:
                        if (value.UnitType != IfcUnitEnum.MASSUNIT) throw new ArgumentException("Incompatible unit type");
                        break;
                    default:
                        break;
                }

                Entity.Unit = value;
            }
            get
            {
                if (Entity.Unit != null)
                    return Entity.Unit;

                var project = Model.Instances.FirstOrDefault<IIfcProject>();
                if (project == null || project.UnitsInContext == null)
                    return null;

                var units = project.UnitsInContext.Units.OfType<IIfcNamedUnit>().ToList();
                if (units.Count == 0)
                    return null;

                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        return units.FirstOrDefault(u => u.UnitType == IfcUnitEnum.AREAUNIT);
                    case QuantityTypeEnum.Count:
                        return null;
                    case QuantityTypeEnum.Length:
                        return units.FirstOrDefault(u => u.UnitType == IfcUnitEnum.LENGTHUNIT);
                    case QuantityTypeEnum.Time:
                        return units.FirstOrDefault(u => u.UnitType == IfcUnitEnum.TIMEUNIT);
                    case QuantityTypeEnum.Volume:
                        return units.FirstOrDefault(u => u.UnitType == IfcUnitEnum.VOLUMEUNIT);
                    case QuantityTypeEnum.Weight:
                        return units.FirstOrDefault(u => u.UnitType == IfcUnitEnum.MASSUNIT);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public double Value
        {
            get
            {
                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        return ((IIfcQuantityArea)Entity).AreaValue;
                    case QuantityTypeEnum.Count:
                        return ((IIfcQuantityCount)Entity).CountValue;
                    case QuantityTypeEnum.Length:
                        return ((IIfcQuantityLength)Entity).LengthValue;
                    case QuantityTypeEnum.Time:
                        return ((IIfcQuantityTime)Entity).TimeValue;
                    case QuantityTypeEnum.Volume:
                        return ((IIfcQuantityVolume)Entity).VolumeValue;
                    case QuantityTypeEnum.Weight:
                        return ((IIfcQuantityWeight)Entity).WeightValue;
                    default:
                        throw new NotSupportedException();
                }
            }
            set
            {
                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        ((IIfcQuantityArea)Entity).AreaValue = value;
                        break;
                    case QuantityTypeEnum.Count:
                        ((IIfcQuantityCount)Entity).CountValue = value;
                        break;
                    case QuantityTypeEnum.Length:
                        ((IIfcQuantityLength)Entity).LengthValue = value;
                        break;
                    case QuantityTypeEnum.Time:
                        ((IIfcQuantityTime)Entity).TimeValue = value;
                        break;
                    case QuantityTypeEnum.Volume:
                        ((IIfcQuantityVolume)Entity).VolumeValue = value;
                        break;
                    case QuantityTypeEnum.Weight:
                        ((IIfcQuantityWeight)Entity).WeightValue = value;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public string Formula
        {
            get
            {
                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        return ((IIfcQuantityArea)Entity).Formula;
                    case QuantityTypeEnum.Count:
                        return ((IIfcQuantityCount)Entity).Formula;
                    case QuantityTypeEnum.Length:
                        return ((IIfcQuantityLength)Entity).Formula;
                    case QuantityTypeEnum.Time:
                        return ((IIfcQuantityTime)Entity).Formula;
                    case QuantityTypeEnum.Volume:
                        return ((IIfcQuantityVolume)Entity).Formula;
                    case QuantityTypeEnum.Weight:
                        return ((IIfcQuantityWeight)Entity).Formula;
                    default:
                        throw new NotSupportedException();
                }
            }
            set
            {
                switch (Type)
                {
                    case QuantityTypeEnum.Area:
                        ((IIfcQuantityArea)Entity).Formula = value;
                        break;
                    case QuantityTypeEnum.Count:
                        ((IIfcQuantityCount)Entity).Formula = value;
                        break;
                    case QuantityTypeEnum.Length:
                        ((IIfcQuantityLength)Entity).Formula = value;
                        break;
                    case QuantityTypeEnum.Time:
                        ((IIfcQuantityTime)Entity).Formula = value;
                        break;
                    case QuantityTypeEnum.Volume:
                        ((IIfcQuantityVolume)Entity).Formula = value;
                        break;
                    case QuantityTypeEnum.Weight:
                        ((IIfcQuantityWeight)Entity).Formula = value;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }

    public enum QuantityTypeEnum
    {
        Area, 
        Count, 
        Length, 
        Time, 
        Volume, 
        Weight
    }

    
}
