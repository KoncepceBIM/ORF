using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostItem : IfcRootWrapper<IIfcCostItem>, IEntity
    {
        internal CostItem(IIfcCostItem item) : base(item)
        {
            Children = new CostChildrenCollection(this, true);
            Quantities = new QuantityCollection(this);
            UnitValues = new ValuesCollection(this);
        }

        public CostItem(CostModel model): base(model.Create.CostItem())
        {
            Quantities = new QuantityCollection(this);
            UnitValues = new ValuesCollection(this);
            Children = new CostChildrenCollection(this, false);
        }

        public string Identifier { get => Entity.Identification; set => Entity.Identification = value; }

        
        public CostChildrenCollection Children { get; }

        public QuantityCollection Quantities { get; }

        public double TotalQuantity => Quantities.Sum(q => q.Value);

        public ValuesCollection UnitValues { get; }

        public double TotalUnitValue => UnitValues.Sum(q => q.Value ?? 0);

        public double TotalCost => TotalQuantity * TotalUnitValue;

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
                .Select(i => new CostItem(i));

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
}
