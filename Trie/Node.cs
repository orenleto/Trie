using System;

namespace Trie
{
    public class Node
    {
        public Node(char[] letters, Node[] children)
        {
            Letters = letters;
            Children = children;
        }

        public char[] Letters { get; private set; }
        public Node[] Children { get; private set; }
        
        private Node AddLetter(char letter, Node node)
        {
            var length = Letters.Length;

            var letters = new char[length + 1];
            Array.Copy(Letters, letters, length);
            var children = new Node[length + 1];
            Array.Copy(Children, children, length);

            letters[length] = letter;
            children[length] = node;

            Letters = letters;
            Children = children;

            return node;
        }

        public virtual void Insert(ReadOnlySpan<char> word)
        {
            var node = this;
            for (var i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                var next = FindNext(node, letter);
                if (next == null)
                {
                    next = new Node(Array.Empty<char>(), Array.Empty<Node>());
                    node = node.AddLetter(letter, next);
                }
                else
                {
                    node = next;
                }
            }

            Node FindNext(Node node, char ch)
            {
                for (var j = 0; j < node.Letters.Length; j++)
                {
                    if (node.Letters[j] == ch)
                    {
                        return node.Children[j];
                    }
                }

                return null;
            }
        }
    }
}