using System.Collections;
using System.Collections.Generic;

namespace Trie.Units
{
    public class TestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {"gamelans", true},
            new object[] {"eesome", true},
            new object[] {"octan", true},
            new object[] {"breards", true},
            new object[] {"wicket", true},
            new object[] {"hysterectomized", true},
            new object[] {"abstrusenesses", true},
            new object[] {"cylindromatous", true},
            new object[] {"tenementization", true},
            new object[] {"zoopharmacological", true},
            new object[] {"abacaba", false},
            new object[] {"abacabadabacaba", false},
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}