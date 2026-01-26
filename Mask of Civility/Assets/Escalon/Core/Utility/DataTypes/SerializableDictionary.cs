
using System;
using System.Collections.Generic;

namespace Escalon.Utility
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        [Serializable]
        public struct KeyValue
        {
            public TKey Key;
            public TValue Value;

            public KeyValue(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private List<KeyValue> _keysAndValues = new List<KeyValue>();

        private bool _keyCollision = false;

        private List<KeyValue> _duplicateKeys = new List<KeyValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            _keysAndValues.Clear();
            foreach (var pair in this)
            {
                _keysAndValues.Add(new KeyValue(pair.Key, pair.Value));
            }

            foreach (var pair in _duplicateKeys)
            {
                _keysAndValues.Add(pair);
            }

            _duplicateKeys.Clear();
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            Clear();

            for (var i = 0; i < _keysAndValues.Count; i++)
            {
                if (!ContainsKey(_keysAndValues[i].Key))
                {
                    Add(_keysAndValues[i].Key, _keysAndValues[i].Value);
                }
                else
                {
                    _duplicateKeys.Add(new KeyValue(_keysAndValues[i].Key, _keysAndValues[i].Value));
                }
            }

            _keyCollision = _duplicateKeys.Count > 0;
        }
    }
}