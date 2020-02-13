using System;

namespace Trie.SearchExtensions
{
    public static class LinearSearchExtensions
    {
        public static bool LinearSearch(this Node root, ReadOnlySpan<char> word)
        {
            var node = root;
            foreach (var ch in word)
            {
                node = TryNext(node, ch);
                if (node == null)
                {
                    return false;
                }
            }

            return node?.Eow ?? false;
        }

        private static Node TryNext(Node node, char c)
        {
            for (var i = 0; i < node.Letters.Length; ++i)
            {
                if (node.Letters[i] == c)
                {
                    return node.Children[i];
                }
            }

            return null;
        }
    }
}