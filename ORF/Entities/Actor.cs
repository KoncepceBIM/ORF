using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class Actor : IfcObjectWrapper<IIfcActor>
    {
        internal Actor(IIfcActor root, bool initPsets) : base(root, initPsets)
        {
        }

        public Actor(CostModel model, string name): this(model.Create.Actor(a => a.Name = name), false)
        {
            
        }

        public IIfcActorSelect PersonAndOrganization { get => Entity.TheActor; set => Entity.TheActor = value; }
    }
}
