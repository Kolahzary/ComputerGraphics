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
        public void FillBackgroundColor(System.Drawing.Color color) => this.FillBackgroundColor(color.A, color.R, color.G, color.B);
        public void FillBackgroundColor(byte alpha, byte red, byte green, byte blue)
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    this.SetPixel(x, y, alpha, red, green, blue);
                }
            }
        }
        public void Line_DDA(IntPoint source, IntPoint destination, Color color) => this.Line_DDA(source.X, source.Y, destination.X, destination.Y, color);
        public void Line_DDA(int x0, int y0, int x1, int y1, Color color) => this.Line_DDA(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Line_DDA(int x0,int y0, int x1,int y1, byte alpha, byte red, byte green, byte blue)
        {
            int 
                dx = x1 - x0,
                dy = y1 - y0;

            int step = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy); //if (Math.Abs(dx) > Math.Abs(dy)) step = Math.Abs(dx); else step = Math.Abs(dy);

            float 
                x=x0,
                y=y0,
                incX = dx / (float)step,
                incY = dy / (float)step;

            for (int i = 0; i < step; i++)
            {
                this.SetPixel((int)Math.Round(x), (int)Math.Round(y), alpha, red, green, blue);
                x += incX;
                y += incY;
            }
        }


    }
}
