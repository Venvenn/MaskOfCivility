using System.Collections.Generic;

namespace Escalon
{
    public class NonBoxingDictionary<K, V>
    {
        private SerializableDictionary<K, V> _items;

        public int Count => _items.Count;
        
        public NonBoxingDictionary()
        {
            _items = new SerializableDictionary<K,V>();
        }
        
        public NonBoxingDictionary(SerializableDictionary<K,V> dictionary)
        {
            _items = dictionary;
        }

        public bool TryAdd<KT, VT>(KT key, VT value) where KT : K where VT : V
        {
            return _items.TryAdd(key, value);
        }
        
        public void Add<KT, VT>(KT key, VT value) where KT : K where VT : V
        {
            if (!_items.TryAdd(key, value))
            {
                _items[key] = value;
            }
        }

        public void Remove<KT>(KT key) where KT : K
        {
            _items.Remove(key);
        }
        
        public void Remove<KT, VT>(KT key, out VT value) where KT : K where VT : V
        {
            _items.Remove(key, out V v);
            value = (VT)v;
        }

        public bool TryGetValue<KT, VT>(KT key, out VT value) where KT : K where VT : V
        {
            bool success = _items.TryGetValue(key, out V v);
            value = success ? (VT) v : default;
            return success;
        }
        
        public bool ContainsKey<KT>(KT key) where KT : K 
        {
            return _items.ContainsKey(key);
        }
        
        public void Clear()
        {
            _items.Clear();
        }

        public SerializableDictionary<K, V> ToDictionary()
        {
            return _items;
        }
    }
}
