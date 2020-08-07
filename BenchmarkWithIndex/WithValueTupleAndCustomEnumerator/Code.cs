using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BenchmarkWithIndex.WithValueTupleAndCustomEnumerator
{
    public struct WithIndexEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerable<T> _enumerable;
        
        private int _currentIndex; 
        private IEnumerator<T> _currentEnumerator; 

        public WithIndexEnumerator(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
            _currentIndex = 0;
            _currentEnumerator = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureEnumerator()
        {
            if (_currentEnumerator == null)
            {
                _currentIndex = 0;
                _currentEnumerator = _enumerable.GetEnumerator();
            }
        }
        
        public bool MoveNext()
        {
            EnsureEnumerator();
            if (_currentEnumerator.MoveNext())
            {
                ++_currentIndex;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            EnsureEnumerator();
            _currentIndex = 0;
            _currentEnumerator.Reset();
        }

        public int Index => _currentIndex;
        public T Current => _currentEnumerator.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _currentEnumerator?.Dispose();
        }
    }
    
    public static class ListExtensions
    {
        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            using var enumerator = new WithIndexEnumerator<T>(enumerable);
            while (enumerator.MoveNext())
            {
                yield return (Index: enumerator.Index, Value: enumerator.Current);
            }
        }
    }
}