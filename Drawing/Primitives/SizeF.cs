using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    /// <summary>
    /// Basic 2D size.
    /// </summary>
    public class SizeF
    {
        public float Width { get; set; }
        public float Height { get; set; }

        /// <summary>
        /// Initializes size with zero.
        /// </summary>
        public SizeF()
        {
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// Initializes with width and height.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
