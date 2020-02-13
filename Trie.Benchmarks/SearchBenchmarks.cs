using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Trie.SearchExtensions;

namespace Trie.Benchmarks
{
    [MemoryDiagnoser]
    public class SearchBenchmarks
    {
        private readonly Node _root = new Node(false, Array.Empty<char>(), Array.Empty<Node>());
        private readonly OrderedNode _orderedRoot = new OrderedNode(false, Array.Empty<char>(), Array.Empty<OrderedNode>());

        [GlobalSetup]
        public void GlobalSetup()
        {
            using var fileStream = File.OpenRead("./vocabulary.txt");
            using var reader = new StreamReader(fileStream);

            while (!reader.EndOfStream)
            {
                var word = reader.ReadLine();
                _root.Insert(word);
                _orderedRoot.Insert(word);
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void LinearSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.LinearSearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void BinarySearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _orderedRoot.BinarySearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void MoveToFrontSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.MoveToFrontSearch(word);
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void IncrementalMoveToFrontSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.IncrementalMoveToFrontSearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void NumericsSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.NumericsSearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void OptimizedAvxSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.OptimizedAvxSearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void AlignmentOptimizedAvxSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.AlignedOptimizedAvxSearch(word);
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GenerateData))]
        public void PipelinedOptimizedAvxSearch(string[] words, string title)
        {
            foreach (var word in words)
            {
                _root.PipelinedOptimizedAvxSearch(word);
            }
        }
        
        public static IEnumerable<object[]> GenerateData()
        {
            var words = new List<string>(20000);
            using var fileStream = File.OpenRead("./words.txt");
            using var reader = new StreamReader(fileStream);
            for (var i = 0; i < 20000 && !reader.EndOfStream; i++)
            {
                var word = reader.ReadLine();
                words.Add(word);
            }

            yield return new object[]
            {
                words.ToArray(),
                "Набор из 20000 слов"
            };
        }
    }
}