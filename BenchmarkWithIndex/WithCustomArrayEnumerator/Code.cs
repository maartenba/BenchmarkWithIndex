using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BenchmarkWithIndex.WithCustomArrayEnumerator
{
    public readonly struct IndexedArrayEnumerable<T> : IEnumerable<(int Index, T Value)>
    {
        private readonly T[] _inner;

        public IndexedArrayEnumerable(T[] inner)
        {
            _inner = inner;
        }
        
        public IEnumerator<(int Index, T Value)> GetEnumerator()
        {
            return new IndexedArrayEnumerator(_inner);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct IndexedArrayEnumerator : IEnumerator<(int Index, T Value)>
        {
            private int _index;
            private readonly T[] _array;

            public IndexedArrayEnumerator(T[] inner)
            {
                _index = -1;
                _array = inner;
            }

            public bool MoveNext()
            {
                ++_index;
                return _index < _array.Length;
            }

            public void Reset()
            {
                _index = -1;
            }

            public (int Index, T Value) Current => (_index, _array[_index]);

            object IEnumerator.Current => (_index, _array[_index]);

            public void Dispose()
            {
            }
        }
    }
    
    public static class ListExtensions
    {
        /// <summary>
        /// This version returns an enumerable of ValueTuple structs. However, when the original
        /// is an array, we return a custom, lightweight <see cref="IndexedArrayEnumerable{T}"/>.
        /// This bypasses the compiler-generated IEnumerable that is slightly heavier.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is T[] array)
            {
                return new IndexedArrayEnumerable<T>(array);
            }

            return WithIndexInternal(enumerable);
        }
        
        private static IEnumerable<(int Index, T Value)> WithIndexInternal<T>(this IEnumerable<T> enumerable)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                yield return (Index: index++, Value: item);
            }
        }
    }
}