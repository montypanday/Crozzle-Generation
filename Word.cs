using System.Drawing;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crozzle
{
    class Word
    {
        public int Startx { get; set; }
        public int Starty { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }


        public Boolean IsProcessed { get; set; }
        public Word(string value, string type, int startx, int starty, bool isprocessed)
        {
            Value = value;
            Type = type;
            Startx = startx;
            Starty = starty;
            IsProcessed = isprocessed;
        }
    }
}
