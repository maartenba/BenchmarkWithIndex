using System.Collections.Generic;
using System.Linq;

namespace BenchmarkWithIndex.WithIndexAsStruct
{
    public struct WithIndex<T>
    {
        public T Value;
        public int Index;

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