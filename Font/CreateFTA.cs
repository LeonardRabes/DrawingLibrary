using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace FontAtlas
{
    public class FTA
    {
        private void Create(string fontFamily)
        {
            Graphics g = Graphics.FromImage(new Bitmap(100, 100));
            Font font = new Font(fontFamily, 100, FontStyle.Regular, GraphicsUnit.Pixel);
            string str = "";
            SizeF[] sizes = new SizeF[94];
            float width = 0;

            for (int i = 33; i <= 126; i++)
            {
                str += (char)i;
                sizes[i - 33] = g.MeasureString((char)i + "", font, 0, StringFormat.GenericTypographic);
                width += sizes[i - 33].Width;
            }

            Bitmap bmp = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(sizes[0].Height));
            g.Dispose();
            g = Graphics.FromImage(bmp);
            g.DrawString(str, font, new SolidBrush(Color.White), new PointF(), StringFormat.GenericTypographic);

            FileStream stream = new FileStream("fontatlas.fta", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);

            //header
            writer.Write(new byte[] { (byte)'F', (byte)'T', (byte)'A' }); //filetype
            writer.Write(2.0F);//version
            writer.Write(bmp.Width);//width
            writer.Write(bmp.Height);//height
            writer.Write(str.Length);//char amount
            writer.Write((byte)33); //starting char
            writer.Write(fontFamily);//fontFamily

            BitArray bitArray = new BitArray();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var val = bmp.GetPixel(x, y).R;

                    bitArray.Add(val == 255);
                }
            }

            writer.Write(bitArray.GetBytes());

            foreach (var s in sizes)
            {
                writer.Write(s.Width);
            }

            g.Dispose();
            stream.Close();
            bmp.Dispose();
        }


    }
}
