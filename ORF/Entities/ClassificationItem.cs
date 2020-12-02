using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class ClassificationItem : IfcWrapper<IIfcClassificationReference>, IClassificationParent
    {
        internal ClassificationItem(IIfcClassificationReference entity, bool init) : base(entity)
        {
            Children = new ClassificationItemCollection(this, init);
        }

        public ClassificationItem(CostModel model): this(model.Create.ClassificationReference(), false)
        {

        }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }
        public string Identification { get => Entity.Identification; set => Entity.Identification = value; }
        public string Sort { get => Entity.Sort; set => Entity.Sort = value; }

        public ClassificationItemCollection Children { get; }
        public IClassificationParent Parent { get; internal set; }

        IIfcClassificationReferenceSelect IClassificationParent.Entity => Entity;
    }
    public class ClassificationItemCollection : ICollection<ClassificationItem>
    {
        private readonly HashSet<ClassificationItem> inner = new HashSet<ClassificationItem>();
        private readonly IClassificationParent parent;

        internal ClassificationItemCollection(IClassificationParent parent, bool init)
        {
            this.parent = parent;
            if (!init)
                return;

            inner = new HashSet<ClassificationItem>(
                parent.Entity.Model.Instances
                    .Where<IIfcClassificationReference>(c => c.ReferencedSource == parent.Entity)
                    .Select(c => new ClassificationItem(c, init) { Parent = parent })
                );
        }

        public int Count => inner.Count;

        public bool IsReadOnly => false;

        public void Add(ClassificationItem item)
        {
            if (!inner.Add(item))
                return;

            item.Entity.ReferencedSource = parent.Entity;
            item.Parent = parent;
        }

        public void Clear()
        {
            foreach (var item in inner)
            {
                item.Parent = null;
                item.Entity.ReferencedSource = null;
            }
            inner.Clear();
        }

        public bool Contains(ClassificationItem item)
        {
            return inner.Contains(item);
        }

        public void CopyTo(ClassificationItem[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ClassificationItem> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public bool Remove(ClassificationItem item)
        {
            if (!inner.Remove(item))
                return false;

            item.Parent = null;
            item.Entity.ReferencedSource = null;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }
    }
}
