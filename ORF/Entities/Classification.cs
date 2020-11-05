using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class Classification : IfcWrapper<IIfcClassification>, IEntity, IClassificationParent
    {
        internal Classification(IIfcClassification entity, bool init) : base(entity)
        {
            Children = new ClassificationItemCollection(this, init);
        }

        public Classification(CostModel model, string name) : this(model.Create.Classification(c => c.Name = name), false)
        {
            model.Classifications.Add(this);
        }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }
        public string Source { get => Entity.Source; set => Entity.Source = value; }
        public string Edition { get => Entity.Edition; set => Entity.Edition = value; }
        public string Location { get => Entity.Location; set => Entity.Location = value; }

        IIfcClassificationReferenceSelect IClassificationParent.Entity => Entity;

        public ClassificationItemCollection Children { get; }
    }

}
