using System;
using System.Collections.Generic;

namespace Escalon.ActionSystem
{
    public abstract class NonBoxingList<T>
    {
        private T[] _items;

        public int Length => _items.Length;

        public T this[int index] => _items[index];

        protected NonBoxingList()
        {
            _items = Array.Empty<T>();
        }

        public void Add<S>(S item) where S : T
        {
            T[] items = new T[_items.Length + 1];

            for (int i = 0; i < _items.Length; i++)
            {
                items[i] = _items[i];
            }

            items[^1] = item;

            _items = items;
        }

        public void Remove<S>(S item) where S : T
        {
            T[] items = new T[_items.Length - 1];

            int i = 0;
            foreach (T t in _items)
            {
                if (!EqualityComparer<T>.Default.Equals(t, item))
                {
                    items[i] = t;
                    i++;
                }
            }

            _items = items;
        }
        
        public void Remove(int step)
        {
            T[] items = new T[_items.Length - 1];

            int i = 0;
            foreach (T t in _items)
            {
                if (i != step)
                {
                    items[i] = t;
                    i++;
                }
            }

            _items = items;
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
    }
}
