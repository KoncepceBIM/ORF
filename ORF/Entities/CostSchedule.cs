using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostSchedule : IfcRootWrapper<IIfcCostSchedule>
    {
        internal CostSchedule(IIfcCostSchedule schedule) : base(schedule)
        {
            var rels = schedule.Controls.ToList();
            _rel = rels.FirstOrDefault();

            var items = rels
                .SelectMany(r => r.RelatedObjects)
                .OfType<IIfcCostItem>()
                .Select(i => new CostItem(i))
                .ToList();
            _costItems = new HashSet<CostItem>(items);

            _relDocs = schedule.HasAssociations
                .OfType<IIfcRelAssociatesDocument>()
                .Where(r => r.RelatingDocument is IIfcDocumentReference)
                .FirstOrDefault();
            if (_relDocs != null)
                _costSystem = new CostSystem(_relDocs.RelatingDocument as IIfcDocumentReference);

        }

        private CostSystem _costSystem;
        public CostSystem CostSystem { 
            get => _costSystem;
            set
            {
                if (value != null && value.Equals(_costSystem))
                    return;

                if (RelDocuments.RelatedObjects.Contains(Entity))
                { 
                    RelDocuments.RelatedObjects.Remove(Entity);
                    _relDocs = null;
                }

                _costSystem = value;
                if (value != null)
                    RelDocuments.RelatingDocument = value.Entity;
                
            }
        }

        private IIfcRelAssociatesDocument _relDocs;
        private IIfcRelAssociatesDocument RelDocuments => _relDocs ?? 
            (_relDocs = Create.RelAssociatesDocument(r => r.RelatedObjects.Add(Entity)));

        private IIfcRelAssignsToControl _rel;
        private IIfcRelAssignsToControl Assignments => _rel ?? (_rel = Create.RelAssignsToControl(r => r.RelatingControl = Entity));

        private readonly HashSet<CostItem> _costItems = new HashSet<CostItem>();
        public IEnumerable<CostItem> CostItemRoots => _costItems;

        public void AddCostItemRoot(CostItem item)
        {
            if (_costItems.Add(item))
                Assignments.RelatedObjects.Add(item.Entity);
        }

        public void RemoveCostItemRoot(CostItem item)
        {
            if (_costItems.Remove(item))
            {
                Entity.Controls.ToList()
                    .ForEach(r => r.RelatedObjects.Remove(item.Entity));
            }
        }

        public double TotalCost => _costItems.Sum(i => i.TotalCost);
    }
}
