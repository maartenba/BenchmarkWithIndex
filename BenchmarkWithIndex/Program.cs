using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkWithIndex.WithIndexAsStruct;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace BenchmarkWithIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

            Console.WriteLine(summary);
            Console.ReadLine();
        }
    }
    
    [MemoryDiagnoser]
    public class WithIndexBenchmark
    {
        private readonly string[] _school =
        {
            "shark", "salmon", "minnow", "tuna", "albacore", "stingray",
            "swordfish", "barracuda", "snapper", "marlin", "trout",
            "sturgeon", "cod", "carp", "bass", "eel", "piranha",
            "catfish", "dogfish", "fishsticks", "cannedfish", "crab"
        };
        
        [Benchmark(Baseline = true)]
        public void Original()
        {
            foreach (var (index, value) in BenchmarkWithIndex.Original.ListExtensions.WithIndex(_school))
            {
                Console.WriteLine($"Item at {index} is {value}.");
            }
        }
        
        [Benchmark]
        public void WithIndexAsStruct()
        {
            foreach (var (index, value) in ListExtensions.WithIndex(_school))
            {
                Console.WriteLine($"Item at {index} is {value}.");
            }
        }
        
        [Benchmark]
        public void WithValueTuple()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTuple.ListExtensions.WithIndex(_school))
            {
                Console.WriteLine($"Item at {index} is {value}.");
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndForLoop()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndForEach.ListExtensions.WithIndex(_school))
            {
                Console.WriteLine($"Item at {index} is {value}.");
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndEnumerator()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndEnumerator.ListExtensions.WithIndex(_school))
            {
                Console.WriteLine($"Item at {index} is {value}.");
            }
        }
        
        [Benchmark]
        public void MicrosoftHighPerf()
        {
            foreach (var item in ArrayExtensions.Enumerate(_school))
            {
                Console.WriteLine($"Item at {item.Index} is {item.Value}.");
            }
        }
    }
}