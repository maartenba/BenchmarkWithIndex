using System.Collections.Generic;
using System.Linq;

namespace BenchmarkWithIndex.Original
{
    public class WithIndex<T>
    {
        public T Value { get; set; }
        public int Index { get; set; }

        public void Deconstruct(out T value, out int index)
            => (index, value) = (Index, Value);
    }

    public static class ListExtensions
    {
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