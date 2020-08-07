using System.Collections.Generic;

namespace BenchmarkWithIndex.WithValueTupleAndForEach
{
    public static class ListExtensions
    {
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                yield return (Index: index++, Value: item);
            }
        }
    }
}