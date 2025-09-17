using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrushPot
{
    public class ConcurrentList<T> : IEnumerable<T>, ICollection<T> where T : notnull
    {
        ConcurrentDictionary<T, object?> innerDictionary = new();

        public int Count => innerDictionary.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            innerDictionary.TryAdd(item, null);
        }

        public void Clear()
        {
            innerDictionary.Clear();
        }

        public bool Contains(T item)
        {
            return innerDictionary.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerDictionary.Keys.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return innerDictionary.Keys.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return innerDictionary.TryRemove(item, out _);
        }

        public void RemoveAll(Predicate<T> predicate)
        {
            foreach (var item in innerDictionary.Keys)
            {
                if (predicate(item))
                    Remove(item);
            }
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            var span = new List<T>();
            foreach (var item in innerDictionary.Keys)
            {
                if (predicate(item))
                    span.Add(item);
            }

            return span;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int i]
        {
            get
            {
                if (i < 0 || i >= innerDictionary.Count)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index is out of range.");
                return innerDictionary.Keys.ElementAt(i);
            }
            set
            {
                if (i < 0 || i >= innerDictionary.Count)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index is out of range.");
                Remove(innerDictionary.Keys.ElementAt(i));
                Add(value);
            }
        }
    }
}
