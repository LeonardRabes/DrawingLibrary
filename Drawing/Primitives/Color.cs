using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public int ToInt32()
        {
            return BitConverter.ToInt32(new byte[] { R, G, B, A }, 0);
        } 

        public byte[] ToBit24BGR()
        {
            return new byte[] { B, G, R };
        }

        public static Color FromRgbA(byte r, byte g, byte b, byte a)
        {
            Color col = new Color();
            col.R = r;
            col.G = g;
            col.B = b;
            col.A = a;

            return col;
        }

        public static Color FromInt32(int col)
        {
            byte[] bytes = BitConverter.GetBytes(col);
            return new Color() { R = bytes[0], G = bytes[1], B = bytes[2], A = bytes[3] };
        }

        public static Color Red {get => new Color() { R = 255, G = 0, B = 0, A = 255 }; }
        public static Color Green { get => new Color() { R = 0, G = 255, B = 0, A = 255 }; }
        public static Color Blue { get => new Color() { R = 0, G = 0, B = 255, A = 255 }; }
        public static Color Black { get => new Color() { R = 0, G = 0, B = 0, A = 255 }; }
        public static Color White { get => new Color() { R = 255, G = 255, B = 255, A = 255 }; }
    }
}
