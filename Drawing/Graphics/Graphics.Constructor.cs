using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public partial class Graphics
    {
        private Bitmap targetBitmap;
        public Graphics(Bitmap bitmap)
        {
            targetBitmap = bitmap;
        }
    }
}
