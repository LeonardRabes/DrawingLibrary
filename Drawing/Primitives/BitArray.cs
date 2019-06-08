using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binary.Collections
{
    /// <summary>
    /// Manages a compact array of bit values, which are represented as Booleans.
    /// </summary>
    public class BitArray
    {
        public int Count { get; internal set; }
        public int ByteCount { get => internalBits.Count; }

        private List<byte> internalBits;
        private int byteIndex; // currently active byte, increased every 8 bits
        private byte bitIndex; // currently active bit of a byte [0..7]
        
        /// <summary>
        /// Create BitArray
        /// </summary>
        public BitArray()
        {
            internalBits = new List<byte>();
            internalBits.Add(0);
            bitIndex = 0;
        }

        /// <summary>
        /// Create BitArray
        /// </summary>
        /// <param name="bits">Given bits</param>
        public BitArray(bool[] bits)
        {
            internalBits = new List<byte>();
            internalBits.Add(0);
            bitIndex = 0;

            AddRange(bits);
        }

        /// <summary>
        /// Create BitArray
        /// </summary>
        /// <param name="bytes">Given bytes</param>
        public BitArray(byte[] bytes, int bitCount)
        {
            internalBits = new List<byte>(bytes);
            Count = bitCount;
            bitIndex = 0;
        }

        /// <summary>
        /// Adds a bit to the end of the array.
        /// </summary>
        /// <param name="bit">Bit to be added</param>
        public void Add(bool bit)
        {
            if (bitIndex >= 8)
            {
                bitIndex = 0;
                byteIndex++;
                internalBits.Add(0);
            }

            if (bit)
            {
                internalBits[byteIndex] += (byte)(0b10000000 >> bitIndex); // right shift bit by current bit index and add to active byte
            }

            bitIndex++;
            Count++;
        }

        /// <summary>
        /// Adds a range of bits to the end of the array.
        /// </summary>
        /// <param name="bits">Bits to be added</param>
        public void AddRange(bool[] bits)
        {
            foreach (var bit in bits)
            {
                Add(bit);
            }
        }

        /// <summary>
        /// Returns all bits as an array of booleans
        /// </summary>
        public bool[] GetBits()
        {
            bool[] bits = new bool[Count];

            for (int i = 0; i < Count; i++)
            {
                byte compare = (byte)(0b10000000 >> i % 8);

                bits[i] = (internalBits[i / 8] & compare) == compare;
            }

            return bits;
        }

        /// <summary>
        /// Returns all bits as an array of bytes
        /// </summary>
        public byte[] GetBytes()
        {
            return internalBits.ToArray();
        }

        /// <summary>
        /// Returns bit by index
        /// </summary>
        public bool GetValue(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            byte compare = (byte)(0b10000000 >> index % 8);

            return (internalBits[index / 8] & compare) == compare;
        }

        /// <summary>
        /// Sets bit by index
        /// </summary>
        public void SetValue(int index, bool bit)
        {
            bool val = GetValue(index);

            if (val != bit && bit)
            {
                internalBits[index / 8] += (byte)(0b10000000 >> index % 8);
            }
            else if (val != bit && !bit)
            {
                internalBits[index / 8] -= (byte)(0b10000000 >> index % 8);
            }
        }

        /// <summary>
        /// Sets/Gets bit by index
        /// </summary>
        public bool this[int index]
        {
            get
            {
                return GetValue(index);
            }

            set
            {
                SetValue(index, value);
            }
        }

    }
}
