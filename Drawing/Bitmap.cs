using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Drawing
{
    public class Bitmap
    {
        public int Width { get => width; }
        public int Height { get => height; }
        public Color[,] PixelData { get => pixelData; }

        private Color[,] pixelData;
        private int width;
        private int height;

        public Bitmap(int width, int height)
        {
            pixelData = new Color[width, height];
            this.width = width;
            this.height = height;
        }

        public Bitmap(int width, int height, Color initColor)
        {
            pixelData = new Color[width, height];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixelData[x, y] = initColor;
                }
            }
        }

        public Stream ToBitmapStream()
        {
            MemoryStream mstream = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(mstream);

            //Header 
            writer.Write(BitConverter.ToInt16(new byte[] { (byte)'B', (byte)'M' }, 0)); //bfType
            writer.Write((int)54 + width * height * 3); //fSize
            writer.Write((int)0); //bfReserved
            writer.Write((int)54); //bfOffBits

            //InfoHeader 
            writer.Write((int)40); //hSize
            writer.Write(width); //width
            writer.Write(-height); //height (- => from top to bottom)
            writer.Write((short)1); //planes
            writer.Write((short)24); //bits per pixel
            writer.Write((int)0); //compression (0 = none)
            writer.Write((int)0); //compressed image size
            writer.Write((int)0); //pixel/m X
            writer.Write((int)0); //pixel/m Y
            writer.Write((int)0); //colors used
            writer.Write((int)0); //important colors (0 = all)

            //pixel data
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    writer.Write(pixelData[x, y].ToBit24BGR());
                }
            }

            return mstream;
        }
    }
}
