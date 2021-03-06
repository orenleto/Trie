using Trie.SearchExtensions;
using Xunit;

namespace Trie.Units
{
    public class SearchTests : IClassFixture<TrieSetup>
    {
        private Node _root;
        private OrderedNode _orderedRoot;

        public SearchTests(TrieSetup setup)
        {
            _root = setup.Root;
            _orderedRoot = setup.OrderedRoot;
        }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void LinearSearchTest(string word, bool expected)
        {
            var actual = _root.LinearSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void BinarySearchTest(string word, bool expected)
        {
            var actual = _orderedRoot.BinarySearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void MoveToFrontSearchTest(string word, bool expected)
        {
            var actual = _root.MoveToFrontSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void IncrementalMoveToFrontSearchTest(string word, bool expected)
        {
            var actual = _root.IncrementalMoveToFrontSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void NumericsSearchTest(string word, bool expected)
        {
            var actual = _root.NumericsSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void OptimizedAVXSearchTest(string word, bool expected)
        {
            var actual = _root.OptimizedAvxSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void AlignedOptimizedAVXSearchTest(string word, bool expected)
        {
            var actual = _root.AlignedOptimizedAvxSearch(word);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void PipelinedOptimizedAVXSearchTest(string word, bool expected)
        {
            var actual = _root.PipelinedOptimizedAvxSearch(word);
            Assert.Equal(expected, actual);
        }
    }
}