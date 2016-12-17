using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Classes
{
    public partial class Bgra32BitmapTool
    {
        public void Scale(int newWidth, int newHeight, double newDpiX, double newDpiY)
        {
            double
                x_ratio = this.Width / (double)newWidth,
                y_ratio = this.Height / (double)newHeight;

            WriteableBitmap wbOld = this.wb;
            this.wb = new WriteableBitmap(newWidth, newHeight, newDpiX, newDpiY, wbOld.Format, wbOld.Palette);

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    this.SetPixel(x, y, wbOld.GetPixeli((int)Math.Floor(x * x_ratio), (int)Math.Floor(y * y_ratio)));
                }
            }

        }
        public void ChangeCanvasSize(int newWidth, int newHeight)
        {
            int minWidth = Math.Min(this.Width, newWidth),
                minHeight = Math.Min(this.Height, newHeight);

            WriteableBitmap wbOld = this.wb;
            this.wb = new WriteableBitmap(newWidth, newHeight, wbOld.DpiX, wbOld.DpiY, wbOld.Format, wbOld.Palette);
            
            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    this.SetPixel(x, y, wbOld.GetPixeli(x,y));
                }
            }
        }

        public void Rotate_90C()
        {
            var wbOld = this.wb;
            this.wb = new WriteableBitmap(
                pixelWidth: wbOld.PixelHeight,
                pixelHeight: wbOld.PixelWidth,
                dpiX: wbOld.DpiY,
                dpiY: wbOld.DpiX,
                pixelFormat: wbOld.Format,
                palette: wbOld.Palette
                );

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.SetPixel(x, y, wbOld.GetPixeli(y, this.Width-1-x));
                }
            }
        }
        public void Rotate_90CC()
        {
            var wbOld = this.wb;
            this.wb = new WriteableBitmap(
                pixelWidth: wbOld.PixelHeight,
                pixelHeight: wbOld.PixelWidth,
                dpiX: wbOld.DpiY,
                dpiY: wbOld.DpiX,
                pixelFormat: wbOld.Format,
                palette: wbOld.Palette
                );

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.SetPixel(x, y, wbOld.GetPixeli(this.Width-1-y, x));
                }
            }
        }
        public void Rotate_180()
        {
            var wbOld = this.wb;
            this.wb = new WriteableBitmap(
                pixelWidth: wbOld.PixelHeight,
                pixelHeight: wbOld.PixelWidth,
                dpiX: wbOld.DpiY,
                dpiY: wbOld.DpiX,
                pixelFormat: wbOld.Format,
                palette: wbOld.Palette
                );

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.SetPixel(x, y, wbOld.GetPixeli(this.Width - 1 - y, this.Height - 1 - x));
                }
            }
        }

        public void Flip_Horizontal()
        {
            int tmp;
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width/2; x++)
                {
                    tmp = this.GetPixeli(x, y);
                    this.SetPixel(x, y, this.GetPixeli(this.Width - 1 - x, y));
                    this.SetPixel(this.Width - 1 - x, y, tmp);
                }
            }
        }
        public void Flip_Vertical()
        {
            int tmp;
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height/2; y++)
                {
                    tmp = this.GetPixeli(x, y);
                    this.SetPixel(x, y, this.GetPixeli(x, this.Height - 1 - y));
                    this.SetPixel(x, this.Height - 1 - y, tmp);
                }
            }
        }
    }
}
