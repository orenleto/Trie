using BenchmarkDotNet.Running;

namespace Trie.Benchmarks
{
    public static class Program
    {
        private static void Main() => BenchmarkRunner.Run<SearchBenchmarks>();
    }
}