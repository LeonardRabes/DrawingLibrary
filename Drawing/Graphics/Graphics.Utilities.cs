using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public partial class Graphics
    {
        public static float Clamp(float val, int min, int max)
        {
            if (val < min)
            {
                return min;
            }
            if (val > max)
            {
                return max;
            }

            return val;
        }

        public static PointF GetMaxPoint(PointF[] points)
        {
            float xmax = 0;
            float ymax = 0;
            foreach (var pt in points)
            {
                if (pt.X > xmax)
                {
                    xmax = pt.X;
                }

                if (pt.Y > ymax)
                {
                    ymax = pt.Y;
                }
            }

            return new PointF(xmax, ymax);
        }

        public static PointF GetMinPoint(PointF[] points)
        {
            float xmin = points[0].X;
            float ymin = points[0].Y;
            foreach (var pt in points)
            {
                if (pt.X < xmin)
                {
                    xmin = pt.X;
                }

                if (pt.Y < ymin)
                {
                    ymin = pt.Y;
                }
            }

            return new PointF(xmin, ymin);
        }

        public static Func<float, float> GetFunction(PointF point1, PointF point2)
        {
            float m = (point1.Y - point2.Y) / (point1.X - point2.X);
            float n = point1.Y - m * point1.X;

            if (!float.IsInfinity(m) && !float.IsNaN(m))
            {
                return x => m * x + n;
            }
            else
            {
                return x => -1;
            }
        }

        public static Color AlphaBlend(Color lower, Color upper)
        {
            float alpha = upper.A / 255F;
            byte r = Convert.ToByte(lower.R * (1 - alpha) + upper.R * alpha);
            byte g = Convert.ToByte(lower.G * (1 - alpha) + upper.G * alpha);
            byte b = Convert.ToByte(lower.B * (1 - alpha) + upper.B * alpha);

            return Color.FromRgbA(r, g, b, 255);
        }

        public static Bitmap ResizeImage(Bitmap bitmap, int newWidth, int newHeight)
        {
            Bitmap newBmp = new Bitmap(newWidth, newHeight, Color.FromInt32(0));
            float distX = newWidth / (float)bitmap.Width;
            float distY = newHeight / (float)bitmap.Height;

            float currentY = 0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                float currentX = 0;
                int newY = (int)Clamp(Convert.ToInt32(currentY), 0, newHeight - 1);

                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color old = bitmap.PixelData[x, y];
                    currentX += distX;

                    int newX = (int)Clamp(Convert.ToInt32(currentX), 0, newWidth - 1);


                    Color current = newBmp.PixelData[newX, newY];

                    if (current.ToInt32() != 0)
                    {
                        old.A /= 2;
                        current = AlphaBlend(current, old);
                    }
                    else
                    {
                        current = old;
                    }

                    newBmp.PixelData[newX, newY] = current;
                }
                currentY += distY;
            }


            return newBmp;
        }

        public SizeF MeasureString(string str, Font font)
        {
            float posX = 0;
            for (int i = 0; i < str.Length; i++)
            {
                RectangleF charRect = font.GetCharacterRect(str[i]);
                posX += charRect.Width;
            }

            return new SizeF(posX, font.FontSize);
        }
    }
}
