using System.Collections.Generic;
using System.Linq;

namespace BenchmarkWithIndex.Original
{
    public class WithIndex<T>
    {
        public T Value { get; set; }
        public int Index { get; set; }

        public void Deconstruct(out int index, out T value)
            => (index, value) = (Index, Value);
    }

    public static class ListExtensions
    {
        /// <summary>
        /// The original implementation from https://twitter.com/buhakmeh/status/1291029712458911752.
        /// </summary>
        public static IEnumerable<WithIndex<T>> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => new WithIndex<T>
            {
                Value = item,
                Index = index
            });
        }
    }
}