using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class PropertySet : IfcWrapper<IIfcPropertySet>, IEntity
    {
        internal PropertySet(IIfcPropertySet entity) : base(entity)
        {
        }

        public PropertySet(CostModel model, string name = null): base(model.Create.PropertySet(ps => ps.Name = name))
        {

        }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }

        public IIfcValue this[string property]
        {
            get
            {
                return Entity.HasProperties
                    .OfType<IIfcPropertySingleValue>()
                    .FirstOrDefault(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase))?
                    .NominalValue;
            }
            set
            {
                var prop = Entity.HasProperties
                    .OfType<IIfcPropertySingleValue>()
                    .FirstOrDefault(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase));
                if (prop == null)
                {
                    prop = Create.PropertySingleValue(p => p.Name = property);
                    Entity.HasProperties.Add(prop);
                }
                prop.NominalValue = value;
            }
        }


        protected bool GetValue<T>(out T value, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") where T:IIfcValue
        {
            var info = GetType().GetProperty(memberName);
            var result = this[info.Name];
            if (result == null)
            {
                value = default;
                return false;
            }

            value = (T)result;
            return true;
        }

        protected void SetValue(IIfcValue value, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            var info = GetType().GetProperty(memberName);
            this[info.Name] = value;
        }

      
    }
}
