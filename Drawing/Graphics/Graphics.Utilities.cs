using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Limits a value between two others.
        /// </summary>
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

        /// <summary>
        /// Searches in a point array for the maximum X and Y value and returns them as PointF.
        /// </summary>
        /// <param name="points">Point array</param>
        /// <returns></returns>
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

        /// <summary>
        /// Searches in a point array for the minimum X and Y value and returns them as PointF.
        /// </summary>
        /// <param name="points">Point array</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a linear function, which is calculated by given points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>If x1 - x2 = 0 the linear function will always return -1.</returns>
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

        /// <summary>
        /// Blends two colors together by the upper colors alpha value.
        /// </summary>
        /// <param name="lower">Color on lower layer.</param>
        /// <param name="upper">Color on upper layer.</param>
        /// <returns></returns>
        public static Color AlphaBlend(Color lower, Color upper)
        {
            float alpha = upper.A / 255F;
            byte r = Convert.ToByte(lower.R * (1 - alpha) + upper.R * alpha);
            byte g = Convert.ToByte(lower.G * (1 - alpha) + upper.G * alpha);
            byte b = Convert.ToByte(lower.B * (1 - alpha) + upper.B * alpha);

            return Color.FromRgbA(r, g, b, 255);
        }

        /// <summary>
        /// Resizes a bitmap to new size.
        /// </summary>
        /// <param name="bitmap">Bitmap to resize.</param>
        /// <param name="newWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap bitmap, int newWidth, int newHeight)
        {
            Bitmap newBmp = new Bitmap(newWidth, newHeight, Color.FromInt32(0));
            float distX = newWidth / (float)bitmap.Width;
            float distY = newHeight / (float)bitmap.Height;

            float currentY = 0;
            float prevY = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                float currentX = 0;
                float prevX = 0;
                int newY = (int)Clamp((float)Math.Ceiling(currentY), 0, newHeight - 1);

                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color old = bitmap.PixelData[x, y];


                    int newX = (int)Clamp((float)Math.Ceiling(currentX), 0, newWidth - 1);


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

                    if (newWidth > bitmap.Width || newHeight > bitmap.Height)
                    {
                        for (int fillY = 0; fillY <= currentY - prevY; fillY++)
                        {
                            for (int fillX = 0; fillX <= currentX - prevX; fillX++)
                            {
                                newBmp.PixelData[newX - fillX, newY - fillY] = current;
                            }
                        }
                    }

                    prevX = currentX;
                    currentX += distX;   
                }
                prevY = currentY;
                currentY += distY;
            }


            return newBmp;
        }

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="str">String to measure.</param>
        /// <param name="font">Font to measure with.</param>
        /// <returns></returns>
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
