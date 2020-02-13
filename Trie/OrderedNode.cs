using System;

namespace Trie
{
    public class OrderedNode : Node
    {
        public OrderedNode(bool eow, char[] letters, OrderedNode[] children)
            : base(eow, letters, children)
        {

        }

        public override void Insert(ReadOnlySpan<char> word)
        {
            var node = this;
            for (var i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                var next = FindNext(node, letter);
                if (next == null)
                {
                    next = new OrderedNode(i + 1 == word.Length, Array.Empty<char>(), Array.Empty<OrderedNode>());
                    node = node.AddLetter(letter, next);
                }
                else
                {
                    node = next;
                }
            }

            OrderedNode FindNext(Node node, char ch)
            {
                var letters = node.Letters;
                if (letters.Length == 0)
                {
                    return null;
                }
                var left = 0;
                var right = letters.Length;

                while (left + 1 < right)
                {
                    var index = (left + right) >> 1;
                    if (letters[index] < ch)
                    {
                        right = index;
                    }
                    else
                    {
                        left = index;
                    }
                }

                return letters[left] == ch
                    ? node.Children[left] as OrderedNode
                    : null;
            }
        }

        private OrderedNode AddLetter(char letter, OrderedNode node)
        {
            var length = Letters.Length;
            var index = 0;

            for (; index < length; ++index)
            {
                if (letter < Letters[index])
                    continue;
                break;
            }

            var letters = new char[length + 1];
            var children = new Node[length + 1];

            Array.Copy(Letters, 0, letters, 0, index);
            letters[index] = letter;
            Array.Copy(Letters, index, letters, index + 1, length - index);

            Array.Copy(Children, 0, children, 0, index);
            children[index] = node;
            Array.Copy(Children, index, children, index + 1, length - index);

            Letters = letters;
            Children = children;

            return node;
        }
    }
}