using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace ORF.Entities
{
    public class CostValue : IfcWrapper<IIfcCostValue>, IEntity
    {
        public CostValue(IIfcCostValue entity) : base(entity)
        {
        }
        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }
        public string Category { get => Entity.Category; set => Entity.Category = value; }

        public IfcArithmeticOperatorEnum? Operator { get => Entity.ArithmeticOperator; set => Entity.ArithmeticOperator = value; }

        public double? Value
        {
            get
            {
                if (Entity.AppliedValue == null)
                    return null;
                if (!(Entity.AppliedValue is IIfcValue v))
                    throw new NotSupportedException("Only simple values are supported for now");
                if (v.UnderlyingSystemType == typeof(double))
                    return (double)(v.Value);
                if (v.UnderlyingSystemType == typeof(float))
                    return (float)(v.Value);
                if (v.UnderlyingSystemType == typeof(int))
                    return (int)(v.Value);
                if (v.UnderlyingSystemType == typeof(long))
                    return (long)(v.Value);

                throw new NotSupportedException("Only simple numeric values are supported for now");
            }
            set
            {
                if (!value.HasValue)
                {
                    Entity.AppliedValue = null;
                    return;
                }

                Entity.AppliedValue = new IfcReal(value.Value);
            }
        }

    }
}
