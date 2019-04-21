using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class Pen
    {
        public Color Color { get; set; }
        public int Width { get; set; }

        public Pen(Color color, int width)
        {
            Color = color;
            Width = width;
        }
    }
}
