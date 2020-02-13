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

        [GlobalSetup]
        public void GlobalSetup()
        {
            using var fileStream = File.OpenRead("./vocabulary.txt");
            using var reader = new StreamReader(fileStream);

            while (!reader.EndOfStream)
            {
                var word = reader.ReadLine();
                _root.Insert(word);
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