using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Trie.SearchExtensions
{
    public static class NumericsSearchExtensions
    {
        public static bool NumericsSearch(this Node root, ReadOnlySpan<char> word)
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
            var length = node.Letters.Length;
            if (length == 0)
                return null;

            var letters = MemoryMarshal.Cast<char, ushort>(node.Letters);
            var itemCount = Vector<ushort>.Count;
            var pattern = new Vector<ushort>(symbol);
            
            var ptr = 0;
            for (; ptr + itemCount < length; ptr += itemCount)
            {
                var sample = new Vector<ushort>(letters.Slice(ptr, itemCount));
                if (!Vector.EqualsAny(sample, pattern))
                    continue;
                for (var j = ptr; j < ptr + itemCount; ++j)
                {
                    if (node.Letters[j] != symbol) continue;
                    return node.Children[j];
                }
            }

            for (; ptr < length; ptr++)
            {
                if (node.Letters[ptr] != symbol) continue;
                return node.Children[ptr];
            }

            return null;
        }
    }
}