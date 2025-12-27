using System;
using System.Collections.Generic;
using System.Linq;

namespace EVLibrary.Extensions
{
    public static class DictionaryExtensions
    {
        public static T1 GetKeyFromValue<T1, T2>(this IDictionary<T1, T2> dictionary, T2 value)
        {
            IDictionary<T1, T2> dict = dictionary;

            foreach (KeyValuePair<T1, T2> pair in dict)
            {
                if (pair.Value.Equals(value))
                {
                    return pair.Key;
                }
            }
            return default;
        }

        public static IDictionary<T1, T2> Merge<T1, T2>(this IDictionary<T1, T2> dictionaryTo, IDictionary<T1, T2> dictionaryFrom)
        {
            dictionaryFrom.ToList().ForEach(kvp =>
            {
                if (dictionaryTo.ContainsKey(kvp.Key))
                {
                    dictionaryTo[kvp.Key] = kvp.Value;
                }
                else
                {
                    dictionaryTo.Add(kvp.Key, kvp.Value);
                }
            });
            return dictionaryTo;
        }


    }
}