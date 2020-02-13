using Trie.SearchExtensions;
using Xunit;

namespace Trie.Units
{
    public class SearchTests : IClassFixture<TrieSetup>
    {
        private Node _root;

        public SearchTests(TrieSetup setup)
        {
            _root = setup.Root;
        }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void LinearSearchTest(string word, bool expected)
        {
            var actual = _root.LinearSearch(word);
            Assert.Equal(expected, actual);
        }
    }
}