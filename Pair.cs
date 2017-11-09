using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crozzle
{
    class Pair
    {
        public int Index { get; set; }
        public string Word { get; set; }
        public Pair(int index, string word)
        {
            Index = index;
            Word = word;
        }

    }
}
