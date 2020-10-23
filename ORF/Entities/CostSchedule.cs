using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostSchedule : IfcRootWrapper<IIfcCostSchedule>
    {
        internal CostSchedule(IIfcCostSchedule schedule) : base(schedule)
        {
            _relDocs = schedule.HasAssociations
                .OfType<IIfcRelAssociatesDocument>()
                .Where(r => r.RelatingDocument is IIfcDocumentReference)
                .FirstOrDefault();
            if (_relDocs != null)
                _costSystem = new CostSystem(_relDocs.RelatingDocument as IIfcDocumentReference);

            CostItemRoots = new RootItemsCollection(this);
        }

        private CostSystem _costSystem;
        public CostSystem CostSystem
        {
            get => _costSystem;
            set
            {
                if (value != null && value.Equals(_costSystem))
                    return;

                if (_relDocs != null && _relDocs.RelatedObjects.Contains(Entity))
                {
                    _relDocs.RelatedObjects.Remove(Entity);
                    _relDocs = null;
                }

                _costSystem = value;
                if (value == null)
                    return;

                _relDocs = Create.RelAssociatesDocument(r =>
                {
                    r.RelatedObjects.Add(Entity);
                    r.RelatingDocument = value.Entity;
                });
            }
        }

        private IIfcRelAssociatesDocument _relDocs;


        public RootItemsCollection CostItemRoots { get; }

        public double TotalCost => CostItemRoots.Sum(i => i.TotalCost);
    }

    public class RootItemsCollection : ICollection<CostItem>
    {
        private readonly HashSet<CostItem> _items;
        private readonly IList<IIfcRelAssignsToControl> _native;
        private readonly Create create;
        private readonly CostSchedule _schedule;

        internal RootItemsCollection(CostSchedule schedule)
        {
            var rels = schedule.Entity.Controls.ToList();
            var items = rels
                .SelectMany(r => r.RelatedObjects)
                .OfType<IIfcCostItem>()
                .Select(i => new CostItem(i))
                .ToList();

            _schedule = schedule;
            _items = new HashSet<CostItem>(items);
            _native = rels;
            create = new Create(schedule.Entity.Model);
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        private IList<IIfcObjectDefinition> GetNative()
        {
            if (_native.Any())
                return _native[0].RelatedObjects;

            var rel = create.RelAssignsToControl(r => r.RelatingControl = _schedule.Entity);
            _native.Add(rel);
            return rel.RelatedObjects;
        }

        public void Add(CostItem item)
        {
            if (!_items.Add(item))
                return;

            GetNative().Add(item.Entity);
        }

        public void Clear()
        {
            _items.Clear();
            foreach (var rel in _native)
            {
                rel.RelatedObjects.Clear();
            }
        }

        public bool Contains(CostItem item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(CostItem[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CostItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(CostItem item)
        {
            if (!_items.Remove(item))
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
