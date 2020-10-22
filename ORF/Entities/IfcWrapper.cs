using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public abstract class IfcWrapper<T> where T: IPersistEntity
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
    }
}
