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
        public void FillBackgroundColor(System.Drawing.Color color)
            => this.FillBackgroundColor(color.A, color.R, color.G, color.B);
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

        public void Line_Naive(IntPoint source, IntPoint destination, Color color)
            => this.Line_Naive(source.X, source.Y, destination.X, destination.Y, color);
        public void Line_Naive(int x0, int y0, int x1, int y1, Color color)
            => this.Line_Naive(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Line_Naive(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            int y,
                dx = x1 - x0,
                dy = y1 - y0;

            if (dx == 0) return; // Prevent division by zero

            for (int x = x0; x <= x1; x++)
            {
                y = y0 + dy * (x - x0) / dx;
                this.SetPixel(x, y, alpha, red, green, blue);
            }
        }

        public void Line_DDA(IntPoint source, IntPoint destination, Color color)
            => this.Line_DDA(source.X, source.Y, destination.X, destination.Y, color);
        public void Line_DDA(int x0, int y0, int x1, int y1, Color color)
            => this.Line_DDA(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Line_DDA(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            int
                dx = x1 - x0,
                dy = y1 - y0;

            int step = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy); //if (Math.Abs(dx) > Math.Abs(dy)) step = Math.Abs(dx); else step = Math.Abs(dy);

            float
                x = x0,
                y = y0,
                incX = dx / (float)step,
                incY = dy / (float)step;

            for (int i = 0; i <= step; i++)
            {
                this.TrySetPixel((int)Math.Round(x), (int)Math.Round(y), alpha, red, green, blue);
                x += incX;
                y += incY;
            }
        }

        public void Line_Bresenham(IntPoint source, IntPoint destination, Color color)
            => this.Line_Bresenham(source.X, source.Y, destination.X, destination.Y, color);
        public void Line_Bresenham(int x0, int y0, int x1, int y1, Color color)
            => this.Line_Bresenham(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Line_Bresenham(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            // TODO
        }

        public void Square_Empty(IntPoint source, IntPoint destination, Color color)
            => this.Square_Empty(source.X, source.Y, destination.X, destination.Y, color);
        public void Square_Empty(int x0, int y0, int x1, int y1, Color color)
            => this.Square_Empty(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Square_Empty(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            int dx = x1 - x0,
                dy = y1 - y0;

            if (Math.Abs(dx) >= Math.Abs(dy))
                this.Rectangle_Empty(x0, y0, x1, y0 + dx, alpha, red, green, blue);
            else
                this.Rectangle_Empty(x0, y0, x0 + dy, y1, alpha, red, green, blue);
        }

        public void Square_Filled(IntPoint source, IntPoint destination, Color color)
            => this.Square_Filled(source.X, source.Y, destination.X, destination.Y, color);
        public void Square_Filled(int x0, int y0, int x1, int y1, Color color)
            => this.Square_Filled(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Square_Filled(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            int dx = x1 - x0,
                dy = y1 - y0;

            if (Math.Abs(dx) >= Math.Abs(dy))
                this.Rectangle_Filled(x0, y0, x1, y0 + dx, alpha, red, green, blue);
            else
                this.Rectangle_Filled(x0, y0, x0 + dy, y1, alpha, red, green, blue);
        }

        public void Rectangle_Empty(IntPoint source, IntPoint destination, Color color)
            => this.Rectangle_Empty(source.X, source.Y, destination.X, destination.Y, color);
        public void Rectangle_Empty(int x0, int y0, int x1, int y1, Color color)
            => this.Rectangle_Empty(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Rectangle_Empty(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            this.Line_DDA(x0, y0, x1, y0, alpha, red, green, blue);
            this.Line_DDA(x0, y0, x0, y1, alpha, red, green, blue);

            this.Line_DDA(x1, y1, x1, y0, alpha, red, green, blue);
            this.Line_DDA(x1, y1, x0, y1, alpha, red, green, blue);
        }

        public void Rectangle_Filled(IntPoint source, IntPoint destination, Color color)
            => this.Rectangle_Filled(source.X, source.Y, destination.X, destination.Y, color);
        public void Rectangle_Filled(int x0, int y0, int x1, int y1, Color color)
            => this.Rectangle_Filled(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Rectangle_Filled(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            if (x0 > x1)
            {
                int tmp = x0;
                x0 = x1;
                x1 = tmp;
            }
            for (int x = x0; x <= x1; x++)
            {
                this.Line_DDA(x, y0, x, y1, alpha, red, green, blue);
            }
        }

        
        public void Triangle(IntPoint first, IntPoint second, IntPoint third, Color color)
            => this.Triangle(first.X, first.Y, second.X, second.Y, third.X, third.Y, color);
        public void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
            => this.Triangle(x0, y0, x1, y1, x2, y2, color.A, color.R, color.G, color.B);
        public void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, byte alpha, byte red, byte green, byte blue)
        {
            this.Line_DDA(x0, y0, x1, y1, alpha, red, green, blue);
            this.Line_DDA(x1, y1, x2, y2, alpha, red, green, blue);
            this.Line_DDA(x2, y2, x0, y0, alpha, red, green, blue);
        }

        public void Triangle_Equilateral(IntPoint source, IntPoint destination, Color color)
            => this.Triangle_Equilateral(source.X, source.Y, destination.X, destination.Y, color);
        public void Triangle_Equilateral(int x0, int y0, int x1, int y1, Color color)
            => this.Triangle_Equilateral(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Triangle_Equilateral(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            //TODO
        }
        public void Triangle_Isosceles(IntPoint source, IntPoint destination, Color color)
            => this.Triangle_Isosceles(source.X, source.Y, destination.X, destination.Y, color);
        public void Triangle_Isosceles(int x0, int y0, int x1, int y1, Color color)
            => this.Triangle_Isosceles(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Triangle_Isosceles(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            int dx = x1 - x0;
            this.Triangle(
                x0, y0,
                x0 + dx, y1,
                x0 - dx, y1,
                alpha, red, green, blue);
        }
        public void Triangle_Right(IntPoint source, IntPoint destination, Color color)
            => this.Triangle_Right(source.X, source.Y, destination.X, destination.Y, color);
        public void Triangle_Right(int x0, int y0, int x1, int y1, Color color)
            => this.Triangle_Right(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void Triangle_Right(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
        {
            this.Triangle(
                x0, y0,
                x1, y1,
                x0, y1,
                alpha, red, green, blue);
        }


        public void Circle_Midpoint(IntPoint center, int radius, Color color) => this.Circle_Midpoint(center.X, center.Y, radius, color);
        public void Circle_Midpoint(int x0, int y0, int radius, Color color) => this.Circle_Midpoint(x0, y0, radius, color.A, color.R, color.G, color.B);
        public void Circle_Midpoint(int x0, int y0, int radius, byte alpha, byte red, byte green, byte blue)
        {
            int x = radius;
            int y = 0;
            int err = 0;

            while (x >= y)
            {
                this.TrySetPixel(x0 + x, y0 + y, alpha, red, green, blue);
                this.TrySetPixel(x0 + y, y0 + x, alpha, red, green, blue);
                this.TrySetPixel(x0 - y, y0 + x, alpha, red, green, blue);
                this.TrySetPixel(x0 - x, y0 + y, alpha, red, green, blue);
                this.TrySetPixel(x0 - x, y0 - y, alpha, red, green, blue);
                this.TrySetPixel(x0 - y, y0 - x, alpha, red, green, blue);
                this.TrySetPixel(x0 + y, y0 - x, alpha, red, green, blue);
                this.TrySetPixel(x0 + x, y0 - y, alpha, red, green, blue);

                y += 1;
                err += 1 + 2 * y;
                if (2 * (err - x) + 1 > 0)
                {
                    x -= 1;
                    err += 1 - 2 * x;
                }
            }
        }

        public void Circle_Bresenham(IntPoint center, int radius, Color color) => this.Circle_Bresenham(center.X, center.Y, radius, color);
        public void Circle_Bresenham(int x0, int y0, int radius, Color color) => this.Circle_Bresenham(x0, y0, radius, color.A, color.R, color.G, color.B);
        public void Circle_Bresenham(int x0, int y0, int radius, byte alpha, byte red, byte green, byte blue)
        {
            // TODO
        }



    }
}
