using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace ORF.Entities
{
    public class CostSubject : IfcObjectWrapper<IIfcObject>
    {
        internal CostSubject(IIfcObject root, bool initPsets) : base(root, initPsets)
        {
            // TODO: keep cache of these wrappers in the CostModel to avoid duplicities
        }

        // TODO: Quantity sets collection
        public QuantitySetsCollection QuantitySets { get; set; }

    }

    public class QuantitySetsCollection : ICollection<QuantitySet>
    {
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(QuantitySet item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(QuantitySet item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(QuantitySet[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<QuantitySet> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(QuantitySet item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
