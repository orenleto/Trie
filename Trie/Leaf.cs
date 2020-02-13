namespace Trie
{
    public class Leaf<T> : Node
    {
        public Leaf(T data, char[] letters, Node[] children)
            : base(letters, children)
        {
            Data = data;
        }

        public T Data { get; }
    }
}