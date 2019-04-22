using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    /// <summary>
    /// Contains information to draw.
    /// </summary>
    public class Pen
    {
        public Color Color { get; set; }
        public int Width { get; set; }

        /// <summary>
        /// Initializes the drawing pen.
        /// </summary>
        /// <param name="color">Color of the pen</param>
        /// <param name="width">Width of the pen</param>
        public Pen(Color color, int width)
        {
            Color = color;
            Width = width;
        }
    }
}
