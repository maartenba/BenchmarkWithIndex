using System.Collections.Generic;

namespace BenchmarkWithIndex.WithValueTupleAndMoveNext
{
    public static class ListExtensions
    {
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            var index = 0;
            using var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (Index: index++, Value: enumerator.Current);
            }
        }
    }
}