using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace BenchmarkWithIndex
{
    class Program
    {
        static void Main() => BenchmarkRunner.Run(typeof(Program).Assembly);
    }
    
    [MemoryDiagnoser]
    public class WithIndexBenchmark
    {
        private readonly string[] _school =
        {
            "shark", "salmon", "minnow", "tuna", "albacore", "stingray",
            "swordfish", "barracuda", "snapper", "marlin", "trout",
            "sturgeon", "cod", "carp", "bass", "eel", "piranha",
            "catfish", "dogfish", "fishsticks", "cannedfish", "crab",
            
            "shark", "salmon", "minnow", "tuna", "albacore", "stingray",
            "swordfish", "barracuda", "snapper", "marlin", "trout",
            "sturgeon", "cod", "carp", "bass", "eel", "piranha",
            "catfish", "dogfish", "fishsticks", "cannedfish", "crab"
        };

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void FakeConsoleWriteLine(int index, string value)
        {
            // noop
            // https://twitter.com/SergioPedri/status/1291667443245625344
        }
        
        [Benchmark(Baseline = true)]
        public void Original()
        {
            foreach (var (index, value) in BenchmarkWithIndex.Original.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void WithIndexAsStruct()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithIndexAsStruct.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void WithValueTuple()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTuple.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndForEach()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndForEach.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndEnumerator()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndEnumerator.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void WithCustomArrayEnumerator()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithCustomArrayEnumerator.ListExtensions.WithIndex(_school))
            {
                FakeConsoleWriteLine(index, value);
            }
        }
        
        [Benchmark]
        public void MicrosoftHighPerf()
        {
            foreach (var item in ArrayExtensions.Enumerate(_school))
            {
                FakeConsoleWriteLine(item.Index, item.Value);
            }
        }
    }
}