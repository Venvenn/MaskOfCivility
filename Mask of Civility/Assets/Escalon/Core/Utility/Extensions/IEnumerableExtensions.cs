using System;
using System.Collections.Generic;
using System.Linq;

namespace Escalon
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            var grouped = source.GroupBy(selector);
            var moreThen1 = grouped.Where(i => i.IsMultiple());

            return moreThen1.SelectMany(i => i);
        }

        public static bool IsMultiple<T>(this IEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext() && enumerator.MoveNext();
        }

        public static IEnumerable<T> ToIEnumarable<T>(this T item)
        {
            yield return item;
        }
    }
}