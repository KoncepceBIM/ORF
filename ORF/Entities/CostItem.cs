using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostItem : IfcObjectWrapper<IIfcCostItem>
    {
        internal CostItem(IIfcCostItem item, bool init) : base(item, init)
        {
            Children = new CostChildrenCollection(this, init);
            ClassificationItems = new ClassificationCollection(this, init);
            Quantities = new QuantityCollection(this);
            UnitValues = new ValuesCollection(this);
        }

        public string Type { 
            get => Entity.ObjectType; 
            set 
            {
                Entity.ObjectType = value;
                if (!string.IsNullOrWhiteSpace(value))
                    Entity.PredefinedType = IfcCostItemTypeEnum.USERDEFINED;
                else
                    Entity.PredefinedType = IfcCostItemTypeEnum.NOTDEFINED;
            } 
        }

        public CostItem(CostModel model): this(model.Create.CostItem(), false)
        {
            
        }

        public string Identifier { get => Entity.Identification; set => Entity.Identification = value; }

        
        public ClassificationCollection ClassificationItems { get; }

        public CostChildrenCollection Children { get; }

        public QuantityCollection Quantities { get; }

        public double TotalQuantity => Quantities.Sum(q => q.Value);

        public ValuesCollection UnitValues { get; }

        public double TotalUnitValue => UnitValues.Sum(q => q.Value ?? 0);

        public double TotalCost => Quantities.Any() ? TotalQuantity * TotalUnitValue : TotalUnitValue;

    }

    public class QuantityCollection : ICollection<Quantity>
    {
        private readonly HashSet<Quantity> _quantities = new HashSet<Quantity>();
        private readonly IList<IIfcPhysicalQuantity> _native;
        private readonly Create create;


        internal QuantityCollection(CostItem item)
        {
            _quantities = new HashSet<Quantity>(item.Entity.CostQuantities
                .OfType<IIfcPhysicalSimpleQuantity>()
                .Select(q => new Quantity(q)));
            _native = item.Entity.CostQuantities;
            create = new Create(item.Entity.Model);
        }

        public int Count => _quantities.Count;

        public bool IsReadOnly => false;


        public void Add(Quantity item)
        {
            if (_quantities.Any() && _quantities.First().Type != item.Type)
                throw new ArgumentException("All quantities should be of the same type", nameof(item));

            if (_quantities.Add(item))
                _native.Add(item.Entity);
        }

        public Quantity AddArea(string name)
        {
            var value = new Quantity(create.QuantityArea(q => q.Name = name));
            Add(value);
            return value;
        }

        public Quantity AddCount(string name)
        {
            var value = new Quantity(create.QuantityCount(q => q.Name = name));
            Add(value);
            return value;
        }

        public Quantity AddLength(string name)
        {
            var value = new Quantity(create.QuantityLength(q => q.Name = name));
            Add(value);
            return value;
        }

        public Quantity AddTime(string name)
        {
            var value = new Quantity(create.QuantityTime(q => q.Name = name));
            Add(value);
            return value;
        }

        public Quantity AddVolume(string name)
        {
            var value = new Quantity(create.QuantityVolume(q => q.Name = name));
            Add(value);
            return value;
        }

        public Quantity AddWeight(string name)
        {
            var value = new Quantity(create.QuantityWeight(q => q.Name = name));
            Add(value);
            return value;
        }

        public void Clear()
        {
            _quantities.Clear();
            _native.Clear();
        }

        public bool Contains(Quantity item)
        {
            return _quantities.Contains(item);
        }

        public void CopyTo(Quantity[] array, int arrayIndex)
        {
            _quantities.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Quantity> GetEnumerator()
        {
            return _quantities.GetEnumerator();
        }

        public bool Remove(Quantity item)
        {
            if (_quantities.Remove(item))
                return _native.Remove(item.Entity);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ValuesCollection : ICollection<CostValue>
    {
        private readonly HashSet<CostValue> _values;
        private readonly IList<IIfcCostValue> _native;
        private readonly Create create;


        internal ValuesCollection(CostItem item)
        {
            _values = new HashSet<CostValue>(item.Entity.CostValues
                .Select(q => new CostValue(q)));
            _native = item.Entity.CostValues;
            create = new Create(item.Entity.Model);
        }

        public int Count => _values.Count;

        public bool IsReadOnly => false;

        public CostValue Add()
        {
            var value = new CostValue(create.CostValue());
            Add(value);
            return value;
        }

        public void Add(CostValue item)
        {
            if (_values.Add(item))
                _native.Add(item.Entity);
        }

        public void Clear()
        {
            _values.Clear();
            _native.Clear();
        }

        public bool Contains(CostValue item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(CostValue[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CostValue> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public bool Remove(CostValue item)
        {
            if (_values.Remove(item))
                return _native.Remove(item.Entity);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CostChildrenCollection : ICollection<CostItem>
    {
        private readonly HashSet<CostItem> _children = new HashSet<CostItem>();
        private readonly IList<IIfcRelNests> _native = new List<IIfcRelNests>();
        private readonly Create create;
        private readonly CostItem _item;

        internal CostChildrenCollection(CostItem item, bool init)
        {
            create = new Create(item.Entity.Model);
            _item = item;

            if (!init)
                return;

            var rels = item.Entity.IsNestedBy.ToList();
            var children = rels.SelectMany(r => r.RelatedObjects)
                .OfType<IIfcCostItem>()
                .Select(i => new CostItem(i, init));

            _children = new HashSet<CostItem>(children);
            _native = rels;
        }

        public int Count => _children.Count;

        public bool IsReadOnly => false;

        public void Add(CostItem item)
        {
            if (!_children.Add(item))
                return;

            if (_native.Any())
            { 
                _native[0].RelatedObjects.Add(item.Entity);
                return;
            }

            var rel = create.RelNests(r => r.RelatingObject = _item.Entity);
            _native.Add(rel);
            rel.RelatedObjects.Add(item.Entity);
        }

        public void Clear()
        {
            _children.Clear();
            foreach (var rel in _native)
                rel.RelatedObjects.Clear();
        }

        public bool Contains(CostItem item)
        {
            return _children.Contains(item);
        }

        public void CopyTo(CostItem[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CostItem> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public bool Remove(CostItem item)
        {
            if (!_children.Remove(item))
                return false;

            foreach (var rel in _native)
                rel.RelatedObjects.Remove(item.Entity);

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ClassificationCollection : ICollection<ClassificationItem>
    {
        private readonly HashSet<ClassificationItem> inner = new HashSet<ClassificationItem>();
        private readonly List<IIfcRelAssociatesClassification> rels = new List<IIfcRelAssociatesClassification>();
        private readonly CostItem costItem;

        public ClassificationCollection(CostItem costItem, bool init)
        {
            this.costItem = costItem;

            if (!init)
                return;

            rels = costItem.Entity.HasAssociations.OfType<IIfcRelAssociatesClassification>().ToList();
            inner = new HashSet<ClassificationItem>(rels.Where(r => r.RelatingClassification is IIfcClassificationReference)
                .Select(r => GetOrCreate(r.RelatingClassification as IIfcClassificationReference)));
        }

        private ClassificationItem GetOrCreate(IIfcClassificationReference item)
        {
            var model = costItem.CostModel;
            foreach (var classification in model.Classifications)
            {
                var items = new Stack<ClassificationItem>(classification.Children);
                while (items.Count != 0)
                {
                    var c = items.Pop();
                    if (c.Entity.Equals(item))
                        return c;

                    if (c.Children.Count == 0)
                        continue;

                    foreach (var child in c.Children)
                    {
                        items.Push(child);
                    }
                }
            }

            // create new if it was not found
            return new ClassificationItem(item, true);
        }

        public int Count => inner.Count;

        public bool IsReadOnly => false;

        public void Add(ClassificationItem item)
        {
            if (!inner.Add(item))
                return;

            var rel = rels.FirstOrDefault();
            if (rel == null)
            { 
                var create = new Create(item.Entity.Model);
                rel = create.RelAssociatesClassification(r => r.RelatingClassification = item.Entity);
                rels.Add(rel);
            }
            rel.RelatedObjects.Add(costItem.Entity);
        }

        public void Clear()
        {
            foreach (var rel in rels)
            {
                rel.RelatedObjects.Remove(costItem.Entity);
                if (!rel.RelatedObjects.Any())
                { 
                    rel.Model.Delete(rel);
                    rels.Remove(rel);
                }
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

            foreach (var rel in rels.Where(r => r.RelatingClassification == item.Entity))
            {
                rel.RelatedObjects.Remove(costItem.Entity);
                if (!rel.RelatedObjects.Any())
                { 
                    rel.Model.Delete(rel);
                    rels.Remove(rel);
                }
            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }
    }
}
