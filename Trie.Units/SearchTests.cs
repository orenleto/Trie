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
    }
}