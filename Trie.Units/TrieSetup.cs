using System;
using System.IO;

namespace Trie.Units
{
    public class TrieSetup : IDisposable
    {
        public Node Root { get; }


        public TrieSetup()
        {
            using var fileStream = File.OpenRead("./words.txt");
            using var reader = new StreamReader(fileStream);

            Root = new Node(false, Array.Empty<char>(), Array.Empty<Node>());
            while (!reader.EndOfStream)
            {
                var word = reader.ReadLine();
                Root.Insert(word);
            }
        }

        public void Dispose()
        {
        }
    }
}