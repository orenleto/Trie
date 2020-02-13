using System;

namespace Trie.SearchExtensions
{
    public static class MoveToFrontSearchExtensions
    {
        public static bool MoveToFrontSearch(this Node root, ReadOnlySpan<char> word)
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
                        var letter = node.Letters[0];
                        var temp = node.Children[0];

                        node.Letters[0] = symbol;
                        node.Letters[i] = letter;
                        
                        node.Children[0]  = node.Children[i];
                        node.Children[i]  = temp;
                    }
                    
                    return node.Children[0];
                }
            }

            return null;
        }

    }
}