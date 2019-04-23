using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Drawing
{
    /// <summary>
    /// Sets the encoding of the Bitmap
    /// </summary>
    public enum BitmapEncoding
    {
        Uncompressed24Bit,
        Uncompressed32Bit
    };

    /// <summary>
    /// Basic image container.
    /// </summary>
    public class Bitmap
    {
        public int Width { get => width; }
        public int Height { get => height; }
        public SizeF Size { get => new SizeF(width, height); }
        public Color[,] PixelData { get => pixelData; }

        private Color[,] pixelData;
        private int width;
        private int height;

        /// <summary>
        /// Initializes a bitmap by a .bmp file without compression.
        /// </summary>
        /// <param name="filename">Filename of a bitmap to load.</param>
        public Bitmap(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            fromStream(stream);
            stream.Close();
        }

        /// <summary>
        /// Initializes a bitmap by a .bmp file without compression.
        /// </summary>
        /// <param name="bmpStream">Stream, which contains .bmp data.</param>
        public Bitmap(Stream bmpStream)
        {
            fromStream(bmpStream);
        }

        /// <summary>
        /// Initializes a bitmap by size.
        /// </summary>
        /// <param name="width">Width of bitmap</param>
        /// <param name="height">Height of bitmap</param>
        public Bitmap(int width, int height)
        {
            pixelData = new Color[width, height];
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Initializes a bitmap by size and color.
        /// </summary>
        /// <param name="width">Width of bitmap</param>
        /// <param name="height">Height of bitmap</param>
        /// /// <param name="initColor">Color of all pixels by initialization.</param>
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

        /// <summary>
        /// Returns a .bmp file as a stream.
        /// </summary>
        public Stream ToStream(BitmapEncoding encoding = BitmapEncoding.Uncompressed24Bit)
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);

            //Settings
            int bytesPerPx = 3;
            if (encoding == BitmapEncoding.Uncompressed24Bit)
            {
                bytesPerPx = 3;
            }
            else if (encoding == BitmapEncoding.Uncompressed32Bit)
            {
                bytesPerPx = 4;
            }

            //padding - if bit amount of a scan line (1x width) cant be divided by 4 padding is needed
            int padding = Convert.ToInt32(Math.Ceiling(width * bytesPerPx / 4F) * 4F - width * bytesPerPx);

            //Header 
            writer.Write(new char[] { 'B', 'M' });                                  //bfType
            writer.Write((int)54 + width * height * bytesPerPx + height * padding); //fSize
            writer.Write((int)0);                                                   //bfReserved
            writer.Write((int)54);                                                  //bfOffBits

            //InfoHeader 
            writer.Write((int)40);                                                  //hSize
            writer.Write(width);                                                    //width
            writer.Write(height);                                                   //height (- => from top to bottom)
            writer.Write((short)1);                                                 //planes
            writer.Write((short)(bytesPerPx * 8));                                  //bits per pixel
            writer.Write((int)0);                                                   //compression (0 = none)
            writer.Write((int)width * height * bytesPerPx);                         //image size
            writer.Write((int)3779);                                                //pixel/m X
            writer.Write((int)3779);                                                //pixel/m Y
            writer.Write((int)0);                                                   //colors used
            writer.Write((int)0);                                                   //important colors (0 = all)

            //pixel data

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (encoding == BitmapEncoding.Uncompressed24Bit)
                    {
                        writer.Write(pixelData[x, y].ToBit24BGR());
                    }
                    else if (encoding == BitmapEncoding.Uncompressed32Bit)
                    {
                        writer.Write(pixelData[x, y].ToBit32BGRA());
                    }
                }

                if (padding > 0)
                {
                    writer.Write(new byte[padding]);
                }
            }

            return mstream;
        }

        private void fromStream(Stream stream)
        {
            stream.Position = 0;
            BinaryReader reader = new BinaryReader(stream);

            //Header 
            string filetype = new string(reader.ReadChars(2));
            int filesize = reader.ReadInt32();
            int reserved = reader.ReadInt32();
            int offset = reader.ReadInt32();

            //InfoHeader 
            int headersize = reader.ReadInt32();
            width = reader.ReadInt32();
            height = reader.ReadInt32();
            int padding = (filesize - width * height * 3) / height;
            short planes = reader.ReadInt16();
            short bitsPerPx = reader.ReadInt16();

            //rest of header
            byte[] hbytes = reader.ReadBytes(headersize - 16);

            stream.Position = offset;
            pixelData = new Color[Math.Abs(width), Math.Abs(height)];

            if (height < 0) // height might be negative
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte[] bytes = reader.ReadBytes(3);
                        pixelData[x, y] = Color.FromRgbA(bytes[2], bytes[1], bytes[0], 255);
                    }
                    stream.Position += padding;
                }
            }
            else
            {
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte[] bytes = reader.ReadBytes(3);
                        pixelData[x, y] = Color.FromRgbA(bytes[2], bytes[1], bytes[0], 255);
                    }
                    stream.Position += padding;
                }
            }

            height = Math.Abs(height);
        }
    }
}
