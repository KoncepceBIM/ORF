using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ORF.Entities
{
    public class QuantitySet
    {
        public QuantitiesCollection Quantities { get; set; }
    }

    public class QuantitiesCollection : ICollection<Quantity>
    {
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(Quantity item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Quantity item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Quantity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Quantity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Quantity item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
