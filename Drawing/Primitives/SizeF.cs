using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class SizeF
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public SizeF()
        {
            Width = 0;
            Height = 0;
        }

        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
