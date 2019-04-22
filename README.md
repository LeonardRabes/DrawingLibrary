# DrawingLibrary
Simple CPU based drawing library with very basic functionality and a syntax similar to System.Drawing.

## Base Classes
### Bitmap
Basic image container.
- load and save uncompressed, 24bit Bitmaps (.bmp)
- create new Bitmaps by size
- contains all pixel data

### Graphics
Contains basic drawing functionality for bitmaps.
- Draw:
  - Line
  - Rectangle
  - Image
  - Strings
- Fill:
  - Rectangle
  - Polygon
- Utilities:
  - Clamp
  - Get max./min. Point
  - Get Function
  - Alpha Blend
  - Resize

### Font
Basic container for character data and can be used to draw strings.
- utilizes a pre generated font atlas (fta) of a wished font family
- can be adjusted to any font size the user likes

### Primitives
- Color
- Pen
- PointF
- RectangleF
- SizeF
