
using System.Collections.Generic;

namespace Escalon.Utility
{
    /// <summary>
    /// A queue that can be exhausted and then reloaded 
    /// </summary>
    public class LoadableKeyedPriorityQueue<K, V, P> where V : class
    {
        private KeyedPriorityQueue<K, V, P> _entityActionQueue = new KeyedPriorityQueue<K, V, P>();
        private Queue<V> _loadedEntityActionQueue = new Queue<V>();

        public int LoadedCount => _loadedEntityActionQueue.Count;
        public int TotalCount => _entityActionQueue.Count;
    
        public void Enqueue(K key, V value, P priority)
        {
            _entityActionQueue.Enqueue(key, value, priority);
        }

        public void Remove(K key)
        {
            _entityActionQueue.Remove(key);
        }
        
        public void Remove(V value)
        {
            _entityActionQueue.Remove(value);
        }
        
        public V Dequeue()
        {
            return _loadedEntityActionQueue.Dequeue();
        }
    
        public void Load(K key, K specialKey)
        {
            _loadedEntityActionQueue = _entityActionQueue.CopyToQueue(key, specialKey, !Equals(key, specialKey));
        }
    }
}