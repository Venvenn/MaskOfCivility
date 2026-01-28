using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Escalon
{
    [Serializable]
    public partial class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
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

        public List<KeyValue> KeysAndValues = new List<KeyValue>();
        public bool KeyCollision = false;

        private List<KeyValue> _duplicateKeys = new List<KeyValue>();

        public SerializableDictionary()
        {
        }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
        {
            KeysAndValues = (List<KeyValue>)info.GetValue("KeysAndValues", typeof(List<KeyValue>));
            _duplicateKeys = (List<KeyValue>)info.GetValue("_duplicateKeys", typeof(List<KeyValue>));

            OnAfterDeserialize();
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            OnBeforeSerialize();
            info.AddValue("KeysAndValues", KeysAndValues);
            info.AddValue("_duplicateKeys", _duplicateKeys);
        }

        public void OnBeforeSerialize()
        {
            foreach (var pair in _duplicateKeys)
            {
                if (!KeysAndValues.Contains(pair))
                {
                    KeysAndValues.Add(pair);
                }
            }

            _duplicateKeys.Clear();
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var keyValue in KeysAndValues)
            {
                if (!ContainsKey(keyValue.Key))
                {
                    Add(keyValue.Key, keyValue.Value);
                }
                else
                {
                    // Detect collision and optionally handle or log it
                    _duplicateKeys.Add(keyValue);
                }
            }

            KeyCollision = _duplicateKeys.Count > 0;
        }
    }
}