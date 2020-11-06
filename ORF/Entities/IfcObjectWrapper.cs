using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public abstract class IfcObjectWrapper<T> : IfcRootWrapper<T> where T : IIfcObject
    {
        private readonly Dictionary<string, IIfcRelDefinesByProperties> _rels = new Dictionary<string, IIfcRelDefinesByProperties>();
        private readonly Dictionary<string, PropertySet> _psets = new Dictionary<string, PropertySet>();

        protected IfcObjectWrapper(T root, bool initPsets) : base(root)
        {
            if (!initPsets)
                return;

            foreach (var rel in root.IsDefinedBy)
            {
                var pset = rel.RelatingPropertyDefinition.PropertySetDefinitions
                    .OfType<IIfcPropertySet>()
                    .FirstOrDefault();
                if (pset == null)
                    continue;

                _rels.Add(pset.Name, rel);
                _psets.Add(pset.Name, new PropertySet(pset));
            }
        }

        public IEnumerable<PropertySet> PropertySets => _psets.Values;

        public PropertySet this[string psetName]
        {
            get
            {
                if (_psets.TryGetValue(psetName, out PropertySet pset))
                    return pset;
                return null;
            }
            set
            {
                if (value == null)
                {
                    if (_psets.TryGetValue(psetName, out PropertySet ps))
                    {
                        _psets.Remove(psetName);
                        var rel = _rels[psetName];
                        rel.RelatedObjects.Remove(Entity);
                        if (!rel.RelatedObjects.Any())
                        {
                            Model.Delete(rel);
                            Model.Delete(ps.Entity);
                        }
                    }
                    return;
                }

                if (_psets.ContainsKey(psetName))
                    throw new ArgumentException();

                if (!string.Equals(value.Name, psetName, StringComparison.OrdinalIgnoreCase))
                {
                    value.Name = psetName;
                }

                var relation = Create.RelDefinesByProperties(r => {
                    r.RelatedObjects.Add(Entity);
                    r.RelatingPropertyDefinition = value.Entity;
                });
                _rels.Add(psetName, relation);
                _psets.Add(psetName, value);

            }
        }
    }
}
