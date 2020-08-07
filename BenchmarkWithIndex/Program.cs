﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkWithIndex.WithIndexAsStruct;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace BenchmarkWithIndex
{
    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly);
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
        
        [Benchmark(Baseline = true)]
        public void Original()
        {
            foreach (var (index, value) in BenchmarkWithIndex.Original.ListExtensions.WithIndex(_school))
            {
                // noop
            }
        }
        
        [Benchmark]
        public void WithIndexAsStruct()
        {
            foreach (var (index, value) in ListExtensions.WithIndex(_school))
            {
                // noop
            }
        }
        
        [Benchmark]
        public void WithValueTuple()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTuple.ListExtensions.WithIndex(_school))
            {
                // noop
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndForEach()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndForEach.ListExtensions.WithIndex(_school))
            {
                // noop
            }
        }
        
        [Benchmark]
        public void WithValueTupleAndEnumerator()
        {
            foreach (var (index, value) in BenchmarkWithIndex.WithValueTupleAndEnumerator.ListExtensions.WithIndex(_school))
            {
                // noop
            }
        }
        
        [Benchmark]
        public void MicrosoftHighPerf()
        {
            foreach (var item in ArrayExtensions.Enumerate(_school))
            {
                // noop
            }
        }
    }
}