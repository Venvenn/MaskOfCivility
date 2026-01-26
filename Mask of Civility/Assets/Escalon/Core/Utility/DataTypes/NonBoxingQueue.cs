using System;

namespace Escalon.ActionSystem
{
    [Serializable]
    public class NonBoxingQueue<T>
    {
        protected T[] _items;

        public int Length => _items.Length;

        public NonBoxingQueue()
        {
            _items = Array.Empty<T>();
        }

        public NonBoxingQueue(T[] items)
        {
            _items = items;
        }
        
        public void Enqueue<S>(S item) where S : T
        {
            T[] items = new T[_items.Length + 1];

            for (int i = 0; i < _items.Length; i++)
            {
                items[i] = _items[i];
            }

            items[^1] = item;

            _items = items;
        }

        public T Dequeue()
        {
            T item = default;
            if (_items.Length > 0)
            {
                item = _items[0];
                T[] items = new T[_items.Length - 1];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = _items[i + 1];
                }

                foreach (T t in _items)
                {
                }

                _items = items;
            }

            return item;
        }

        public void Insert<S>(int step, S item) where S : T
        {
            T[] items = new T[_items.Length + 1];

            int i = 0;
            foreach (T t in _items)
            {
                if (i == step)
                {
                    items[i] = item;
                    i++;
                }

                items[i] = t;
                i++;
            }

            _items = items;
        }

        public T[] CopyFrom()
        {
            return _items;
        }

        public void Clear()
        {
            _items = Array.Empty<T>();
        }
    }
}
