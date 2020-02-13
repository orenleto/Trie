using System;
using System.Linq;
using Xunit;

namespace Trie.Units
{
    public class BuildTrieTests
    {
        [Fact]
        public void ShouldAddLetters()
        {
            var root = new Node(false, Array.Empty<char>(), Array.Empty<Node>());
            foreach (var word in new[] {"a", "b"})
            {
                root.Insert(word);
            }

            Assert.Equal(root.Letters, new[] {'a', 'b'});
        }

        [Fact]
        public void ShouldNotDuplicatesLetters()
        {
            var root = new Node(false, Array.Empty<char>(), Array.Empty<Node>());
            foreach (var word in new[] {"ab", "ac"})
            {
                root.Insert(word);
            }

            Assert.Equal(root.Letters, new[] {'a'});
        }

        [Fact]
        public void ShouldSetEow()
        {
            var root = new Node(false, Array.Empty<char>(), Array.Empty<Node>());
            foreach (var word in new[] {"ab", "ac"})
            {
                root.Insert(word);
            }

            Assert.All(root.Children, node => Assert.False(node.Eow));
            Assert.All(root.Children.SelectMany(node => node.Children), node => Assert.True(node.Eow));
        }
    }

}