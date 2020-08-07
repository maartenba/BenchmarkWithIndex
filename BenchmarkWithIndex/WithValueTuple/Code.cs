using System.Collections.Generic;
using System.Linq;

namespace BenchmarkWithIndex.WithValueTuple
{
    public static class ListExtensions
    {
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => (Index: index, Value: item));
        }
    }
}