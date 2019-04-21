using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class RectangleF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public PointF Location { get => new PointF(X, Y); set { X = value.X; Y = value.Y; } }
        public SizeF Size { get => new SizeF(Width, Height); set { Width = value.Width; Height = value.Height; } }
        
        public RectangleF()
        {
            Location = new PointF();
            Size = new SizeF();
        }

        public RectangleF(PointF location, SizeF size)
        {
            Location = location;
            Size = size;
        }

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
