using System.Collections.Generic;
using System.Linq;
using BenchmarkWithIndex.WithIndexAsStruct;

namespace BenchmarkWithIndex.WithValueTuple
{
    public static class ListExtensions
    {
        /// <summary>
        /// Improves <see cref="WithIndexAsStruct.ListExtensions.WithIndex{T}"/> by returning
        /// a ValueTuple directly. Cut out the middle person!
        /// </summary>
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => (Index: index, Value: item));
        }
    }
}