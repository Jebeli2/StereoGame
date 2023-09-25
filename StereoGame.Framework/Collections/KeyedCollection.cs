using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Collections
{
    public class KeyedCollection<TKey, TValue> : ICollection<TValue> where TKey : notnull
    {
        private readonly Func<TValue, TKey> getKey;
        private readonly Dictionary<TKey, TValue> dictionary = new();
        public KeyedCollection(Func<TValue, TKey> getKey)
        {
            this.getKey = getKey;
        }

        public TValue this[TKey key] => dictionary[key];
        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<TValue> GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        public void AddRange(IEnumerable<TValue> items)
        {
            foreach (TValue item in items)
            {
                dictionary.Add(getKey(item), item);
            }
        }

        public void Add(TValue item)
        {
            dictionary.Add(getKey(item), item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(TValue item)
        {
            return dictionary.ContainsKey(getKey(item));
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {

        }

        public bool Remove(TValue item)
        {
            return dictionary.Remove(getKey(item));
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }
    }
}
