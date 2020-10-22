using System;
using System.Collections.Generic;
using System.Text;

namespace ORF.Entities
{
    public interface IEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
