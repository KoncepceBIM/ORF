using System;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public abstract class IfcWrapper<T> where T : IPersistEntity
    {
        protected IfcWrapper(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entity = entity;
            Create = new Create(Model);
        }

        public T Entity { get; }
        protected IModel Model => Entity.Model;
        protected Create Create { get; }

        public CostModel CostModel => Model.Tag as CostModel;

        public override bool Equals(object obj)
        {
            if (!(obj is IfcWrapper<T> w))
                return false;

            return Entity.Equals(w.Entity);
        }

        public override int GetHashCode()
        {
            return Entity.EntityLabel;
        }

        public static bool operator ==(IfcWrapper<T> a, IfcWrapper<T> b) 
        {
            return a.Equals(b);
        }

        public static bool operator !=(IfcWrapper<T> a, IfcWrapper<T> b)
        {
            return !a.Equals(b);
        }
    }
}