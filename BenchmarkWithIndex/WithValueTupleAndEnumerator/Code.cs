using System.Collections.Generic;

namespace BenchmarkWithIndex.WithValueTupleAndEnumerator
{
    public static class ListExtensions
    {
        /// <summary>
        /// Version of <see cref="WithValueTupleAndForEach.ListExtensions.WithIndex{T}"/>, where instead of
        /// using compiler and runtime magic, we'll use the enumerator directly.
        /// </summary>
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