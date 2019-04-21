using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public partial class Graphics
    {
        public void DrawLine(Pen pen, PointF point1, PointF point2)
        {
            if ((point2 - point1).X == 0 && pen.Width > 1) //if vertical line
            {
                PointF maxPoint = GetMaxPoint(new PointF[] { point1, point2 });
                PointF minPoint = GetMinPoint(new PointF[] { point1, point2 });

                minPoint -= new PointF(pen.Width / 2, 0);
                maxPoint += new PointF(pen.Width / 2, 0);

                FillRectangle(pen.Color, new RectangleF(minPoint, (maxPoint - minPoint).ToSize()));
            }
            else if ((point2 - point1).Y == 0 && pen.Width > 1) //if horizontal line
            {
                PointF maxPoint = GetMaxPoint(new PointF[] { point1, point2 });
                PointF minPoint = GetMinPoint(new PointF[] { point1, point2 });

                minPoint -= new PointF(0, pen.Width / 2);
                maxPoint += new PointF(0, pen.Width / 2);

                FillRectangle(pen.Color, new RectangleF(minPoint, (maxPoint - minPoint).ToSize()));
            }
            else if (pen.Width > 1) //if any line width > 1
            {
                PointF direction = (point2 - point1);
                float orthX = (-direction.Y * 2) / direction.X;
                PointF orthDir = new PointF(orthX, 2);
                orthDir /= orthDir.Length;

                Func<float, PointF, PointF> function = (x, suppV) => suppV + orthDir * x;

                PointF pt1 = function(pen.Width / -2F, point1);
                PointF pt2 = function(pen.Width / 2F, point1);
                PointF pt3 = function(pen.Width / 2F, point2);
                PointF pt4 = function(pen.Width / -2F, point2);

                FillPolygon(pen.Color, new PointF[] { pt1, pt2, pt3, pt4 });
            }
            else // width <= 1
            {
                int x0 = Convert.ToInt32(point1.X);
                int y0 = Convert.ToInt32(point1.Y);
                int x1 = Convert.ToInt32(point2.X);
                int y1 = Convert.ToInt32(point2.Y);

                int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                int err = (dx > dy ? dx : -dy) / 2, e2;
                for (; ; )
                {
                    targetBitmap.PixelData[(int)Clamp(x0, 0, targetBitmap.Width - 1), (int)Clamp(y0, 0, targetBitmap.Height - 1)] = pen.Color;
                    if (x0 == x1 && y0 == y1) break;
                    e2 = err;
                    if (e2 > -dx) { err -= dy; x0 += sx; }
                    if (e2 < dy) { err += dx; y0 += sy; }
                }
            }
        }

        public void DrawRectangle(Pen pen, RectangleF rectangle)
        {
            PointF upperLeft = new PointF(rectangle.X, rectangle.Y); ;
            PointF upperRight = new PointF(rectangle.X + rectangle.Width, rectangle.Y);
            PointF lowerLeft = new PointF(rectangle.X, rectangle.Y + rectangle.Height);
            PointF lowerRight = new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);

            for (int y = (int)upperLeft.Y - pen.Width / 2; y < (int)lowerLeft.Y + pen.Width / 2; y++)
            {
                for (int x = (int)upperLeft.X - pen.Width / 2; x < (int)upperRight.X + pen.Width / 2; x++)
                {
                    if (pen.Width > 1)
                    {
                        if ((y < upperLeft.Y + pen.Width / 2 || x < upperLeft.X + pen.Width / 2) || (y >= lowerLeft.Y - pen.Width / 2 || x >= upperRight.X - pen.Width / 2))
                        {
                            targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = pen.Color;
                        }
                    }
                    else
                    {
                        if ((y == upperLeft.Y || x == upperLeft.X) || (y == lowerLeft.Y - 1 || x == upperRight.X - 1))
                        {
                            targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = pen.Color;
                        }
                    }

                }
            }
        }

        public void DrawImage(Bitmap bitmap, PointF point)
        {
            DrawImage(bitmap, point, new RectangleF(new PointF(), bitmap.Size));
        }

        public void DrawImage(Bitmap bitmap, PointF point, RectangleF rectangle)
        {
            for (int y = (int)point.Y; y < (int)point.Y + rectangle.Height; y++)
            {
                for (int x = (int)point.X; x < (int)point.X + rectangle.Width; x++)
                {
                    int upperX = Convert.ToInt32(x - point.X + rectangle.X);
                    int upperY = Convert.ToInt32(y - point.Y + rectangle.Y);
                    Color upper = bitmap.PixelData[(int)Clamp(upperX, 0, bitmap.Width - 1), (int)Clamp(upperY, 0, targetBitmap.Height - 1)];
                    Color lower = targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)];

                    Color newCol = AlphaBlend(lower, upper);

                    targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = newCol;
                }
            }
        }

        public void DrawImage(Bitmap bitmap, PointF point, RectangleF rectangle, Color color)
        {
            for (int y = (int)point.Y; y < (int)point.Y + rectangle.Height; y++)
            {
                for (int x = (int)point.X; x < (int)point.X + rectangle.Width; x++)
                {
                    int upperX = Convert.ToInt32(x - point.X + rectangle.X);
                    int upperY = Convert.ToInt32(y - point.Y + rectangle.Y);
                    Color upper = bitmap.PixelData[(int)Clamp(upperX, 0, bitmap.Width - 1), (int)Clamp(upperY, 0, targetBitmap.Height - 1)];
                    Color lower = targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)];

                    color.A = upper.A;
                    Color newCol = AlphaBlend(lower, color);

                    targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = newCol;
                }
            }
        }

        public void DrawString(string str, Font font, Color fontColor, PointF location)
        {
            float posX = 0;
            for (int i = 0; i < str.Length; i++)
            {
                PointF newPt = new PointF(location.X + posX, location.Y);
                RectangleF charRect = font.GetCharacterRect(str[i]);

                if (charRect.X > -1 && charRect.Y > -1)
                {
                    DrawImage(font.FontAtlasBmp, newPt, charRect, fontColor);
                }

                posX += charRect.Width;
            }
        }
    }
}
