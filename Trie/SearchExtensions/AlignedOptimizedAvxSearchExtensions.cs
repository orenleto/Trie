using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Trie.SearchExtensions
{
    public static class AlignedOptimizedAvxSearchExtensions
    {
        public static bool AlignedOptimizedAvxSearch(this Node root, ReadOnlySpan<char> word)
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

        private static unsafe Node TryNext(Node node, char ch)
        {
            var length = node?.Letters?.Length ?? 0;
            if (length == 0)
                return null;

            var letters = node.Letters;
            var itemCount = Vector256<ushort>.Count;
            var pattern = Vector256.Create(ch);
            fixed (char* s = &letters[0])
            {
                var aligned = (ushort*)(((ulong)s + 15UL) & ~15UL);
                var pos = Math.Min((int) (aligned - (ushort*) s), length);
                var ptr = 0;
                for (; ptr < pos; ptr++)
                {
                    if (letters[ptr] != ch) continue;
                    return node.Children[ptr];
                }

                for (; ptr + itemCount < length; ptr += itemCount)
                {
                    var sample = Avx.LoadVector256((ushort*) (s + ptr));
                    var result = Avx2.CompareEqual(pattern, sample).AsByte();
                    var mask = Avx2.MoveMask(result);
                    if (mask == 0) continue;

                    var index = Bmi1.TrailingZeroCount((uint) mask) >> 1;
                    return node.Children[ptr + index];
                }

                for (; ptr < length; ptr++)
                {
                    if (letters[ptr] != ch) continue;
                    return node.Children[ptr];
                }
            }

            return null;
        }
    }
}