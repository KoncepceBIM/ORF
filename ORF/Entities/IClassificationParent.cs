using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public interface IClassificationParent: IEntity
    {
        IIfcClassificationReferenceSelect Entity { get; }
    }
}
