using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostSchedule : IfcObjectWrapper<IIfcCostSchedule>
    {

        internal CostSchedule(IIfcCostSchedule schedule, bool init) : base(schedule, init)
        {
            CostItems = new RootItemsCollection(this, init);
            Actors = new ActorsCollection(this, init);
            ClassificationItems = new ClassificationCollection(Entity, init);
        }

        public CostSchedule(CostModel model, string name) : this(model.Create.CostSchedule(s => s.Name = name), false)
        {
            model.Project.CostSchedules.Add(this);
        }

        public ActorsCollection Actors { get; }
        public ClassificationCollection ClassificationItems { get; }
        public RootItemsCollection CostItems { get; }

        public double TotalCost => CostItems.Sum(i => i.TotalCost);
    }

    public class RootItemsCollection : ICollection<CostItem>
    {
        private readonly HashSet<CostItem> _items = new HashSet<CostItem>();
        private readonly IList<IIfcRelAssignsToControl> _native = new List<IIfcRelAssignsToControl>();
        private readonly Create create;
        private readonly CostSchedule _schedule;

        internal RootItemsCollection(CostSchedule schedule, bool init)
        {
            _schedule = schedule;
            create = new Create(schedule.Entity.Model);

            if (!init)
                return;

            var rels = schedule.Entity.Controls.ToList();
            var items = rels
                .SelectMany(r => r.RelatedObjects)
                .OfType<IIfcCostItem>()
                .Select(i => new CostItem(i, init))
                .ToList();

            _items = new HashSet<CostItem>(items);
            _native = rels;
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

    public class ActorsCollection : ICollection<Actor>
    {
        private readonly CostSchedule schedule;
        private readonly HashSet<Actor> inner = new HashSet<Actor>();
        private readonly List<IIfcRelAssignsToActor> rels = new List<IIfcRelAssignsToActor>();

        public ActorsCollection(CostSchedule schedule, bool init)
        {
            this.schedule = schedule;

            if (!init)
                return;

            rels = schedule.Entity.HasAssignments.OfType<IIfcRelAssignsToActor>().ToList();
            inner = new HashSet<Actor>(rels.Select(r => new Actor(r.RelatingActor, true)));
        }

        public int Count => inner.Count;

        public bool IsReadOnly => false;

        public void Add(Actor item, string role)
        {
            if (!inner.Add(item))
                return;

            var create = new Create(schedule.Entity.Model);
            var rel = rels.FirstOrDefault();
            if (rel == null)
            {
                rel = create.RelAssignsToActor(r => r.RelatingActor = item.Entity);
                rels.Add(rel);
            }


            rel.RelatedObjects.Add(schedule.Entity);
            rel.ActingRole = create.ActorRole(r => {
                r.Role = IfcRoleEnum.USERDEFINED;
                r.UserDefinedRole = role;
            });
        }

        public void Add(Actor item, IfcRoleEnum role)
        {
            if (!inner.Add(item))
                return;

            var create = new Create(schedule.Entity.Model);
            var rel = rels.FirstOrDefault();
            if (rel == null)
            {
                rel = create.RelAssignsToActor(r => r.RelatingActor = item.Entity);
                rels.Add(rel);
            }


            rel.RelatedObjects.Add(schedule.Entity);
            rel.ActingRole = create.ActorRole(r => r.Role = role);
        }

        public void Add(Actor item)
        {
            if (!inner.Add(item))
                return;

            var rel = rels.FirstOrDefault();
            if (rel == null)
            {
                var c = new Create(schedule.Entity.Model);
                rel = c.RelAssignsToActor(r => r.RelatingActor = item.Entity);
                rels.Add(rel);
            }

            rel.RelatedObjects.Add(schedule.Entity);
        }

        public void Clear()
        {
            foreach (var rel in rels)
            {
                rel.RelatedObjects.Remove(schedule.Entity);

                // purge
                if (!rel.RelatedObjects.Any())
                    schedule.Entity.Model.Delete(rel);
            }

            inner.Clear();
        }

        public bool Contains(Actor item)
        {
            return inner.Contains(item);
        }

        public void CopyTo(Actor[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Actor> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public bool Remove(Actor item)
        {
            if (!inner.Remove(item))
                return false;

            foreach (var rel in rels.Where(r => r.RelatingActor == item.Entity))
            {
                rel.RelatedObjects.Remove(schedule.Entity);

                // purge
                if (!rel.RelatedObjects.Any())
                    schedule.Entity.Model.Delete(rel);
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }
    }
}
