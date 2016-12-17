using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Classes
{
    public static class Bgra32BitmapExtensions
    {
        public static Color GetPixel(this WriteableBitmap wb, int x, int y)
        {
            int c = wb.GetPixeli(x, y);
            return Color.FromArgb(
                a: (byte)(c >> 24),
                r: (byte)((c >> 16) & 0xFF),
                g: (byte)((c >> 8) & 0xFF),
                b: (byte)(c & 0xFF));
        }
        public static unsafe int GetPixeli(this WriteableBitmap wb, int x, int y)
        {
            return ((int*)wb.BackBuffer)[y * wb.PixelWidth + x];
        }

        public static unsafe void SetPixel(this WriteableBitmap wb, int x, int y, int color)
        {
            ((int*)wb.BackBuffer)[(y * wb.PixelWidth + x)] = color;
        }
    }
}
