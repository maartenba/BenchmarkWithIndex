using System.Collections.Generic;
using System.Linq;

namespace BenchmarkWithIndex.WithIndexAsStruct
{
    public struct WithIndex<T>
    {
        public T Value;
        public int Index;

        public void Deconstruct(out int index, out T value)
            => (index, value) = (Index, Value);
    }

    public static class ListExtensions
    {
        /// <summary>
        /// As suggested in https://twitter.com/maartenballiauw/status/1291044097235484674,
        /// the <see cref="BenchmarkWithIndex.WithIndexAsStruct.WithIndex{T}"/> class has been
        /// made a struct so it is not allocated on the managed heap (and does not incur GC).
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