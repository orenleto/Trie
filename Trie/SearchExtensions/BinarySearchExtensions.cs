using System;

namespace Trie.SearchExtensions
{
    public static class BinarySearchExtensions
    {
        public static bool BinarySearch(this Node root, ReadOnlySpan<char> word)
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

        private static Node TryNext(Node node, char symbol)
        {
            var letters = node.Letters;

            var left = 0;
            var right = letters.Length;

            if (left == right)
            {
                return null;
            }

            while (left + 1 < right)
            {
                var index = (left + right) >> 1;
                if (letters[index] < symbol)
                {
                    right = index;
                }
                else
                {
                    left = index;
                }
            }

            return letters[left] == symbol
                ? node.Children[left]
                : null;
        }
    }
}