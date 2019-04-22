using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    /// <summary>
    /// Basic rectangle with location and size.
    /// </summary>
    public class RectangleF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public PointF Location { get => new PointF(X, Y); set { X = value.X; Y = value.Y; } }
        public SizeF Size { get => new SizeF(Width, Height); set { Width = value.Width; Height = value.Height; } }
        
        /// <summary>
        /// Initialize a rectangle with zero.
        /// </summary>
        public RectangleF()
        {
            Location = new PointF();
            Size = new SizeF();
        }

        /// <summary>
        /// Initialize a rectangle with location and size.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public RectangleF(PointF location, SizeF size)
        {
            Location = location;
            Size = size;
        }

        /// <summary>
        /// Initialize a rectangle with location and size.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
