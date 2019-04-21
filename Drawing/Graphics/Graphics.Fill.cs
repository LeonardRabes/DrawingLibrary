using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public partial class Graphics
    {
        public void Invalidate(Color fillColor)
        {
            for (int y = 0; y < targetBitmap.Height; y++)
            {
                for (int x = 0; x < targetBitmap.Width; x++)
                {
                    targetBitmap.PixelData[x, y] = fillColor;
                }
            }
        }

        public void FillRectangle(Color fillColor, RectangleF rectangle)
        {
            for (int y = (int)rectangle.Y; y < (int)rectangle.Y + rectangle.Height; y++)
            {
                for (int x = (int)rectangle.X; x < (int)rectangle.X + rectangle.Width; x++)
                {
                    targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = fillColor;
                }
            }
        }

        public void FillPolygon(Color fillColor, PointF[] points)
        {
            //get outer points
            PointF maxPoint = GetMaxPoint(points);
            PointF minPoint = GetMinPoint(points);

            //get functions
            Func<float, float>[] functions = new Func<float, float>[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                //connect 0 up to last -1
                if (i < points.Length - 1)
                {
                    functions[i] = GetFunction(points[i], points[i + 1]);
                }
                //connect last to 0
                else
                {
                    functions[i] = GetFunction(points[i], points[0]);
                }

            }

            for (int x = (int)minPoint.X; x <= (int)maxPoint.X; x++)
            {
                int[] intersec = new int[points.Length];
                bool draw = false;

                for (int y = (int)minPoint.Y; y <= (int)maxPoint.Y; y++)
                {
                    //get intersections
                    for (int i = 0; i < points.Length; i++)
                    {
                        int ptx1;
                        int ptx2;
                        if (i < points.Length - 1)
                        {
                            ptx1 = (int)points[i].X;
                            ptx2 = (int)points[i + 1].X;
                        }
                        else
                        {
                            //because last to first
                            ptx1 = (int)points[i].X;
                            ptx2 = (int)points[0].X;
                        }

                        //check if x are within bounds of the corresponding Point.X values
                        if (x > ptx1 && x <= ptx2 || x > ptx2 && x <= ptx1)
                        {
                            intersec[i] = Convert.ToInt32(functions[i](x));
                        }
                        else
                        {
                            intersec[i] = -1;
                        }
                    }

                    for (int i = 0; i < intersec.Length; i++)
                    {
                        if (y == intersec[i])
                        {
                            //toggle draw if intersection
                            draw = !draw;
                        }
                    }

                    if (draw)
                    {
                        targetBitmap.PixelData[(int)Clamp(x, 0, targetBitmap.Width - 1), (int)Clamp(y, 0, targetBitmap.Height - 1)] = fillColor;
                    }
                }
            }
        }
    }
}
