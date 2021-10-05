using System;
using System.Collections.Generic;

namespace AutoEdge2Slice.Editor
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TResult> PreScan<TSource, TResult>(this IEnumerable<TSource> source,
            TResult startValue, Func<TSource, TResult, TResult> result)
        {
            var preValue = startValue;
            foreach (var item in source)
            {
                yield return preValue;
                preValue = result(item, preValue);
            }
        }
    }
}