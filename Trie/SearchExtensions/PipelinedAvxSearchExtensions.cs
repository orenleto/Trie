using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Trie.SearchExtensions
{
    public static class PipelinedAvxSearchExtensions
    {
        public static bool PipelinedOptimizedAvxSearch(this Node root, ReadOnlySpan<char> word)
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
                
                var count = 4 * itemCount;
                for (; ptr + count < length; ptr += count)
                {
                    var sample1 = Avx.LoadVector256((ushort*) (s + ptr));
                    var sample2 = Avx.LoadVector256((ushort*) (s + ptr + itemCount));
                    var sample3 = Avx.LoadVector256((ushort*) (s + ptr + 2 * itemCount));
                    var sample4 = Avx.LoadVector256((ushort*) (s + ptr + 3 * itemCount));
                    
                    var result1 = Avx2.CompareEqual(pattern, sample1).AsByte();
                    var result2 = Avx2.CompareEqual(pattern, sample2).AsByte();
                    var result3 = Avx2.CompareEqual(pattern, sample3).AsByte();
                    var result4 = Avx2.CompareEqual(pattern, sample4).AsByte();
                    
                    var mask1 = Avx2.MoveMask(result1);
                    var mask2 = Avx2.MoveMask(result2);
                    var mask3 = Avx2.MoveMask(result3);
                    var mask4 = Avx2.MoveMask(result4);
                    if (mask1 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask1) >> 1;
                        return node.Children[ptr + index];
                    }
                    if (mask2 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask2) >> 1;
                        return node.Children[ptr + itemCount + index];
                    }
                    if (mask3 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask3) >> 1;
                        return node.Children[ptr + 2 * itemCount + index];
                    }
                    if (mask4 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask4) >> 1;
                        return node.Children[ptr + 3 * itemCount + index];
                    }
                }
                
                count = 2 * itemCount;
                for (; ptr + count < length; ptr += count)
                {
                    var sample1 = Avx.LoadVector256((ushort*) (s + ptr));
                    var sample2 = Avx.LoadVector256((ushort*) (s + ptr + itemCount));
                    
                    var result1 = Avx2.CompareEqual(pattern, sample1).AsByte();
                    var result2 = Avx2.CompareEqual(pattern, sample2).AsByte();
                    
                    var mask1 = Avx2.MoveMask(result1);
                    var mask2 = Avx2.MoveMask(result2);
                    if (mask1 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask1) >> 1;
                        return node.Children[ptr + index];
                    }
                    if (mask2 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask2) >> 1;
                        return node.Children[ptr + itemCount + index];
                    }
                }
                
                count = itemCount;
                for (; ptr + count < length; ptr += count)
                {
                    var sample1 = Avx.LoadVector256((ushort*) (s + ptr));
                    var result1 = Avx2.CompareEqual(pattern, sample1).AsByte();
                    
                    var mask1 = Avx2.MoveMask(result1);
                    if (mask1 != 0)
                    {
                        var index = Bmi1.TrailingZeroCount((uint) mask1) >> 1;
                        return node.Children[ptr + index];
                    }
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