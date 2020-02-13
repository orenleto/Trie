using System;

namespace Trie.SearchExtensions
{
    public static class IncrementalMoveToFrontSearchExtensions
    {
        public static bool IncrementalMoveToFrontSearch(this Node root, ReadOnlySpan<char> word)
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
            for (var i = 0; i < node.Letters.Length; ++i)
            {
                if (node.Letters[i] == symbol)
                {
                    if (i != 0)
                    {
                        var letter = node.Letters[i];
                        var temp = node.Children[i];

                        node.Letters[i] = node.Letters[i - 1];
                        node.Letters[i - 1] = letter;
                        
                        node.Children[i]  = node.Children[i - 1];
                        node.Children[i - 1]  = temp;
                        
                        return node.Children[i - 1];
                    }

                    return node.Children[0];
                }
            }

            return null;
        }
    }
}