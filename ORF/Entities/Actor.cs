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

        public Actor(CostModel model, string name) : this(model.Create.Actor(a => a.Name = name), false)
        {

        }

        public IIfcPerson Person
        {
            get
            {
                if (Entity.TheActor == null)
                    return null;
                if (Entity.TheActor is IIfcPerson person)
                    return person;
                if (Entity.TheActor is IIfcPersonAndOrganization pao)
                    return pao.ThePerson;
                return null;
            }
            set
            {
                if (Entity.TheActor == null || Entity.TheActor is IIfcPerson)
                {
                    Entity.TheActor = value;
                    return;
                }
                if (Entity.TheActor is IIfcPersonAndOrganization pao)
                {
                    pao.ThePerson = value;
                    return;
                }
                if (Entity.TheActor is IIfcOrganization org)
                {
                    var c = new Create(Model);
                    Entity.TheActor = c.PersonAndOrganization(p =>
                    {
                        p.ThePerson = value;
                        p.TheOrganization = org;
                    });
                }
            }
        }

        public IIfcOrganization Organization
        {
            get
            {
                if (Entity.TheActor == null)
                    return null;
                if (Entity.TheActor is IIfcOrganization org)
                    return org;
                if (Entity.TheActor is IIfcPersonAndOrganization pao)
                    return pao.TheOrganization;
                return null;
            }
            set
            {
                if (Entity.TheActor == null || Entity.TheActor is IIfcOrganization)
                {
                    Entity.TheActor = value;
                    return;
                }
                if (Entity.TheActor is IIfcPersonAndOrganization pao)
                {
                    pao.TheOrganization = value;
                    return;
                }
                if (Entity.TheActor is IIfcPerson person)
                {
                    var c = new Create(Model);
                    Entity.TheActor = c.PersonAndOrganization(p =>
                    {
                        p.ThePerson = person;
                        p.TheOrganization = value;
                    });
                }
            }
        }
    }
}
