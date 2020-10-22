using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public abstract class IfcRootWrapper<T>: IfcWrapper<T>, IEntity where T: IIfcRoot
    {
        protected IfcRootWrapper(T root): base(root)
        {

        }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }
        public Guid Id { get => Entity.GlobalId; set => Entity.GlobalId = value; }

    }
}
