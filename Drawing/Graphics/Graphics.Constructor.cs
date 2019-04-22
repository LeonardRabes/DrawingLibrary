using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    /// <summary>
    /// Contains basic drawing functionality for bitmaps.
    /// </summary>
    public partial class Graphics
    {
        private Bitmap targetBitmap;

        /// <summary>
        /// Initializes graphics by a bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap to be edited</param>
        public Graphics(Bitmap bitmap)
        {
            targetBitmap = bitmap;
        }
    }
}
