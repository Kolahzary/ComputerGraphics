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
        
        public void SetPixel(int x, int y, Color color) => this.SetPixel(x, y, color.A, color.R, color.G, color.B);
        public void SetPixel(int x, int y, byte red, byte green, byte blue) => this.SetPixel(x, y, byte.MaxValue, red, green, blue);
        public unsafe void SetPixel(int x, int y, byte alpha, byte red, byte green, byte blue)
        {
            ((int*)wb.BackBuffer)[(y * wb.PixelWidth + x)] = (alpha << 24) | (red << 16) | (green << 8) | blue;

            //((byte*)wb.BackBuffer)[(y * wb.BackBufferStride + x * 4)] = alpha;
            //((byte*)wb.BackBuffer)[(y * wb.BackBufferStride + x * 4) + 1] = red;
            //((byte*)wb.BackBuffer)[(y * wb.BackBufferStride + x * 4) + 2] = green;
            //((byte*)wb.BackBuffer)[(y * wb.BackBufferStride + x * 4) + 3] = blue;
        }
        public unsafe Color GetPixel(int x, int y)
        {
            int c = ((int*)wb.BackBuffer)[y * wb.PixelWidth + x];
            return Color.FromArgb(
                a: (byte)(c >> 24),
                r: (byte)((c >> 16) & 0xFF),
                g: (byte)((c >> 8) & 0xFF),
                b: (byte)(c & 0xFF));


            //byte alpha = (byte)(c >> 24);
            //byte red = (byte)((c >> 16) & 0xFF);
            //byte green = (byte)((c >> 8) & 0xFF);
            //byte blue = (byte)(c & 0xFF);
            //return Color.FromArgb(alpha, red, green, blue);
        }

        public void Apply()
        {
            wb.Lock();
            wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
            wb.Unlock();
        }
    }
}
