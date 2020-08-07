using System.Collections.Generic;

namespace BenchmarkWithIndex.WithValueTupleAndForEach
{
    public static class ListExtensions
    {
        /// <summary>
        /// Version of <see cref="WithValueTuple.ListExtensions.WithIndex{T}"/>, where instead of
        /// using a .Select(), a foreach loop is used instead. Assumption is there are fewer
        /// allocations happening. 
        /// </summary>
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