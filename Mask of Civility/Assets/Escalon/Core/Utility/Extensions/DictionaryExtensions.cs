
using System;
using System.Collections.Generic;
using System.Linq;

namespace Escalon
{
    public static class DictionaryExtensions 
    {
        public static TValue RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, Random random)
        {
            List<TValue> values = dict.Values.ToList();
            return values.Random(random);
        }
    
        public static TKey RandomKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, Random random)
        {
            List<TKey> values = dict.Keys.ToList();
            return values.Random(random);
        }
    }
}
