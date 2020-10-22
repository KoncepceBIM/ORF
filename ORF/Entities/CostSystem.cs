using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostSystem : IfcWrapper<IIfcDocumentReference>, IEntity
    {
        public CostSystem(IIfcDocumentReference entity) : base(entity)
        {
        }

        public string Name { get => Entity.Name; set => Entity.Name = value; }
        public string Description { get => Entity.Description; set => Entity.Description = value; }
        public string Location { get => Entity.Location; set => Entity.Location = value; }
        public string Identification { get => Entity.Identification; set => Entity.Identification = value; }
    }
}
