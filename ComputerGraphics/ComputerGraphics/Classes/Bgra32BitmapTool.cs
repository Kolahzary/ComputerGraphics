using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Classes
{
    public partial class Bgra32BitmapTool
    {
        private WriteableBitmap wb;

        public WriteableBitmap WritableBitmap => this.wb;
        public int Width => wb.PixelWidth;
        public int Height => wb.PixelHeight;
        public double Resolution => wb.DpiX;
        public Bgra32BitmapTool(Uri filePath)
            : this(new BitmapImage(filePath) { CreateOptions = BitmapCreateOptions.None })
        {

        }
        public Bgra32BitmapTool(BitmapSource bs)
        {
            this.wb = new WriteableBitmap(bs);
        }
        public Bgra32BitmapTool(int width, int height)
            : this(width,height,96.0)
        {

        }
        public Bgra32BitmapTool(int width, int height, double resolutionDPI)
            : this(new WriteableBitmap(
                pixelWidth: width,
                pixelHeight: height,
                dpiX: resolutionDPI,
                dpiY: resolutionDPI,
                pixelFormat: PixelFormats.Bgra32,
                palette: null))
        {

        }
        public Bgra32BitmapTool(WriteableBitmap writableBitmap)
        {
            this.wb = writableBitmap;
        }

        public void TrySetPixel(int x, int y, Color color)
            => this.TrySetPixel(x, y, ColorTool.ColorToInt(color));
        public void TrySetPixel(int x, int y, byte red, byte green, byte blue)
            => this.TrySetPixel(x, y, ColorTool.ArgbToInt(byte.MaxValue, red, green, blue));
        public bool TrySetPixel(float x, float y, byte alpha, byte red, byte green, byte blue)
            => this.TrySetPixel((int)Math.Round(x), (int)Math.Round(y), alpha, red, green, blue);
        public bool TrySetPixel(int x, int y, byte alpha, byte red, byte green, byte blue)
            => this.TrySetPixel(x, y, ColorTool.ArgbToInt(alpha, red, green, blue));
        public bool TrySetPixel(float x, float y, int color)
            => this.TrySetPixel((int)Math.Round(x), (int)Math.Round(y), color);
        public bool TrySetPixel(int x, int y, int color)
        {
            if (this.IsAllowd(x,y))
            {
                this.SetPixel(x, y, color);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if coordinate is inside canvas
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <returns>true if x,y is inside canvas else false</returns>
        public bool IsAllowd(int x, int y)
            => !(x < 0 || y < 0 || this.Width <= x || this.Height <= y);

        public void SetPixel(int x, int y, Color color)
            => this.SetPixel(x, y, ColorTool.ColorToInt(color));
        public void SetPixel(int x, int y, byte red, byte green, byte blue)
            => this.SetPixel(x, y, ColorTool.ArgbToInt(byte.MaxValue, red, green, blue));
        public unsafe void SetPixel(int x, int y, byte alpha, byte red, byte green, byte blue)
            => this.SetPixel(x, y, ColorTool.ArgbToInt(alpha, red, green, blue));
        public unsafe void SetPixel(int x, int y, int color)
        {
            ((int*)wb.BackBuffer)[(y * wb.PixelWidth + x)] = color;
        }

        public Color GetPixel(int x, int y)
        {
            int c = this.GetPixeli(x, y);
            return Color.FromArgb(
                a: (byte)(c >> 24),
                r: (byte)((c >> 16) & 0xFF),
                g: (byte)((c >> 8) & 0xFF),
                b: (byte)(c & 0xFF));
        }
        public unsafe int GetPixeli(int x, int y)
        {
            return ((int*)wb.BackBuffer)[y * wb.PixelWidth + x];
        }


        public void Apply()
        {
            wb.Lock();
            wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
            wb.Unlock();
        }
    }
}
