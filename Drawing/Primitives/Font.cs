using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Drawing
{
    public class Font
    {
        public string FontFamily;
        public float FontSize;
        public Bitmap FontAtlasBmp;

        public float[] charPositionX;
        private int charAmount;
        private byte startingChar;

        public Font(Stream fontAtlasStream, float fontSize)
        {
            FontSize = fontSize;
            FontAtlasBmp = parseStream(fontAtlasStream);
            FontAtlasBmp = scaleFont(fontSize);
        }

        public RectangleF GetCharacterRect(char c)
        {
            int index = (int)c - startingChar + 1;
            if (index < charAmount - 1)
            {
                return new RectangleF(charPositionX[index], 0, charPositionX[index + 1] - charPositionX[index], FontAtlasBmp.Height);
            }
            else
            {
                return new RectangleF(charPositionX[(int)Graphics.Clamp(index, 0, charAmount - 1)], 0, FontAtlasBmp.Width - charPositionX[(int)Graphics.Clamp(index, 0, charAmount - 1)], FontAtlasBmp.Height);
            }
        }

        private Bitmap parseStream(Stream fontAtlas)
        {
            BinaryReader reader = new BinaryReader(fontAtlas);
            string str = new string(reader.ReadChars(3));
            float version = reader.ReadSingle();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            charAmount = reader.ReadInt32();
            startingChar = reader.ReadByte();
            FontFamily = reader.ReadString();

            if (str != "FTA")
            {
                throw new Exception("Not an FTA file");
            }

            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color col = new Color();
                    col.R = reader.ReadByte();
                    col.G = reader.ReadByte();
                    col.B = reader.ReadByte();
                    col.A = col.R;

                    bmp.PixelData[x, y] = col;
                }
            }

            charPositionX = new float[charAmount];
            float currentPos = 0;
            for (int i = 0; i < charAmount; i++)
            {
                charPositionX[i] = currentPos;
                currentPos += reader.ReadSingle();
            }
            return bmp;
        }

        private Bitmap scaleFont(float fontSize)
        {
            float factor = fontSize / FontAtlasBmp.Height;

            for (int i = 0; i < charAmount; i++)
            {
                charPositionX[i] *= factor;
            }

            return Graphics.ResizeImage(FontAtlasBmp, Convert.ToInt32(FontAtlasBmp.Width * factor), Convert.ToInt32(fontSize));
        }
    }
}
