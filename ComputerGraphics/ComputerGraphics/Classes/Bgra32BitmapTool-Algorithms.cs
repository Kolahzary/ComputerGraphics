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
        public void FillBackgroundColor(Color color)
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
            => this.Line_Naive(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Line_Naive(int x0, int y0, int x1, int y1, Color color)
            => this.Line_Naive(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Line_Naive(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Line_Naive(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Line_Naive(int x0, int y0, int x1, int y1, int color)
        {
            int y,
                dx = x1 - x0,
                dy = y1 - y0;

            if (dx == 0) return; // Prevent division by zero

            for (int x = x0; x <= x1; x++)
            {
                y = y0 + dy * (x - x0) / dx;
                this.SetPixel(x, y, color);
            }
        }

        public void Line_DDA(IntPoint source, IntPoint destination, Color color)
            => this.Line_DDA(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Line_DDA(int x0, int y0, int x1, int y1, Color color)
            => this.Line_DDA(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Line_DDA(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Line_DDA(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Line_DDA(int x0, int y0, int x1, int y1, int color)
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
                this.TrySetPixel((int)Math.Round(x), (int)Math.Round(y), color);
                x += incX;
                y += incY;
            }
        }
        
        public void Line_Bresenham(IntPoint source, IntPoint destination, Color color)
            => this.Line_Bresenham(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Line_Bresenham(int x0, int y0, int x1, int y1, Color color)
            => this.Line_Bresenham(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Line_Bresenham(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Line_Bresenham(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Line_Bresenham(int x0, int y0, int x1, int y1, int color)
        {
            // TODO
        }
        

        public void Square_Empty(IntPoint source, IntPoint destination, Color color)
            => this.Square_Empty(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Square_Empty(int x0, int y0, int x1, int y1, Color color)
            => this.Square_Empty(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Square_Empty(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Square_Empty(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Square_Empty(int x0, int y0, int x1, int y1, int color)
        {
            int dx = x1 - x0,
                dy = y1 - y0;

            if (Math.Abs(dx) >= Math.Abs(dy))
                this.Rectangle_Empty(x0, y0, x1, y0 + dx, color);
            else
                this.Rectangle_Empty(x0, y0, x0 + dy, y1, color);
        }
        

        public void Square_Filled(IntPoint source, IntPoint destination, Color color)
            => this.Square_Filled(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Square_Filled(int x0, int y0, int x1, int y1, Color color)
            => this.Square_Filled(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Square_Filled(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Square_Filled(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Square_Filled(int x0, int y0, int x1, int y1, int color)
        {
            int dx = x1 - x0,
                dy = y1 - y0;

            if (Math.Abs(dx) >= Math.Abs(dy))
                this.Rectangle_Filled(x0, y0, x1, y0 + dx, color);
            else
                this.Rectangle_Filled(x0, y0, x0 + dy, y1, color);
        }
        
        public void Rectangle_Empty(IntPoint source, IntPoint destination, Color color)
            => this.Rectangle_Empty(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Rectangle_Empty(int x0, int y0, int x1, int y1, Color color)
            => this.Rectangle_Empty(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Rectangle_Empty(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Rectangle_Empty(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Rectangle_Empty(int x0, int y0, int x1, int y1, int color)
        {
            this.Line_DDA(x0, y0, x1, y0, color);
            this.Line_DDA(x0, y0, x0, y1, color);

            this.Line_DDA(x1, y1, x1, y0, color);
            this.Line_DDA(x1, y1, x0, y1, color);
        }

        public void Rectangle_Filled(IntPoint source, IntPoint destination, Color color)
            => this.Rectangle_Filled(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Rectangle_Filled(int x0, int y0, int x1, int y1, Color color)
            => this.Rectangle_Filled(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Rectangle_Filled(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Rectangle_Filled(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Rectangle_Filled(int x0, int y0, int x1, int y1, int color)
        {
            if (x0 > x1)
            {
                int tmp = x0;
                x0 = x1;
                x1 = tmp;
            }
            for (int x = x0; x <= x1; x++)
            {
                this.Line_DDA(x, y0, x, y1, color);
            }
        }

        
        public void Triangle(IntPoint first, IntPoint second, IntPoint third, Color color)
            => this.Triangle(first.X, first.Y, second.X, second.Y, third.X, third.Y, ColorTool.ColorToInt(color));
        public void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
            => this.Triangle(x0, y0, x1, y1, x2, y2, ColorTool.ColorToInt(color));
        public void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, byte alpha, byte red, byte green, byte blue)
            => this.Triangle(x0, y0, x1, y1, x2, y2, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, int color)
        {
            this.Line_DDA(x0, y0, x1, y1, color);
            this.Line_DDA(x1, y1, x2, y2, color);
            this.Line_DDA(x2, y2, x0, y0, color);
        }

        public void Triangle_Equilateral(IntPoint center, int radius, Color color)
            => this.Triangle_Equilateral(center.X, center.Y, radius, ColorTool.ColorToInt(color));
        public void Triangle_Equilateral(int xc, int yc, int radius, Color color)
            => this.Triangle_Equilateral(xc, yc, radius, ColorTool.ColorToInt(color));
        public void Triangle_Equilateral(int xc, int yc, int radius, byte alpha, byte red, byte green, byte blue)
            => this.Triangle_Equilateral(xc, yc, radius, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Triangle_Equilateral(int xc, int yc, int radius, int color)
        {
            this.Triangle(
                xc-radius, yc+radius,
                xc+radius, yc+radius,
                xc, yc-radius,
                color);
        }

        public void Triangle_Isosceles(IntPoint source, IntPoint destination, Color color)
            => this.Triangle_Isosceles(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Triangle_Isosceles(int x0, int y0, int x1, int y1, Color color)
            => this.Triangle_Isosceles(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Triangle_Isosceles(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Triangle_Isosceles(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Triangle_Isosceles(int x0, int y0, int x1, int y1, int color)
        {
            int dx = x1 - x0;
            this.Triangle(
                x0, y0,
                x0 + dx, y1,
                x0 - dx, y1,
                color);
        }
        
        public void Triangle_Right(IntPoint source, IntPoint destination, Color color)
            => this.Triangle_Right(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Triangle_Right(int x0, int y0, int x1, int y1, Color color)
            => this.Triangle_Right(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Triangle_Right(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Triangle_Right(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Triangle_Right(int x0, int y0, int x1, int y1, int color)
        {
            this.Triangle(
                x0, y0,
                x1, y1,
                x0, y1,
                color);
        }
        
        public void Circle_Midpoint(IntPoint center, int radius, Color color)
            => this.Circle_Midpoint(center.X, center.Y, radius, ColorTool.ColorToInt(color));
        public void Circle_Midpoint(int xc, int yc, int radius, Color color)
            => this.Circle_Midpoint(xc, yc, radius, ColorTool.ColorToInt(color));
        public void Circle_Midpoint(int xc, int yc, int radius, byte alpha, byte red, byte green, byte blue)
            => this.Circle_Midpoint(xc, yc, radius, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Circle_Midpoint(int xc, int yc, int radius, int color)
        {
            int x = radius;
            int y = 0;
            int err = 0;

            while (x >= y)
            {
                this.TrySetPixel(xc + x, yc + y, color);
                this.TrySetPixel(xc + y, yc + x, color);
                this.TrySetPixel(xc - y, yc + x, color);
                this.TrySetPixel(xc - x, yc + y, color);
                this.TrySetPixel(xc - x, yc - y, color);
                this.TrySetPixel(xc - y, yc - x, color);
                this.TrySetPixel(xc + y, yc - x, color);
                this.TrySetPixel(xc + x, yc - y, color);

                y += 1;
                err += 1 + 2 * y;
                if (2 * (err - x) + 1 > 0)
                {
                    x -= 1;
                    err += 1 - 2 * x;
                }
            }
        }
        

        public void Circle_Bresenham(IntPoint center, int radius, Color color)
            => this.Circle_Bresenham(center.X, center.Y, radius, ColorTool.ColorToInt(color));
        public void Circle_Bresenham(int xc, int yc, int radius, Color color)
            => this.Circle_Bresenham(xc, yc, radius, ColorTool.ColorToInt(color));
        public void Circle_Bresenham(int xc, int yc, int radius, byte alpha, byte red, byte green, byte blue)
            => this.Circle_Bresenham(xc, yc, radius, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Circle_Bresenham(int xc, int yc, int radius, int color)
        {
            // TODO
        }

        public void Ellipse_Midpoint(IntPoint center, int radiusX, int radiusY, Color color)
            => this.Ellipse_Midpoint(center.X, center.Y, radiusX, radiusY, ColorTool.ColorToInt(color));
        public void Ellipse_Midpoint(int xc, int yc, int radiusX, int radiusY, Color color)
            => this.Ellipse_Midpoint(xc, yc, radiusX, radiusY, ColorTool.ColorToInt(color));
        public void Ellipse_Midpoint(int xc, int yc, int radiusX, int radiusY, byte alpha, byte red, byte green, byte blue)
            => this.Ellipse_Midpoint(xc, yc, radiusX, radiusY, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Ellipse_Midpoint(int xc, int yc, int radiusX, int radiusY, int color)
        {
            float rxSq = radiusX * radiusX;
            float rySq = radiusY * radiusY;
            float x = 0, y = radiusY, p;
            float px = 0, py = 2 * rxSq * y;

            this.TrySetPixel(xc + x, yc + y, color);
            this.TrySetPixel(xc - x, yc + y, color);
            this.TrySetPixel(xc + x, yc - y, color);
            this.TrySetPixel(xc - x, yc - y, color);

            //Region 1
            p = rySq - (rxSq * radiusY) + ((float)0.25 * rxSq);
            while (px < py)
            {
                x++;
                px = px + 2 * rySq;
                if (p < 0)
                    p = p + rySq + px;
                else
                {
                    y--;
                    py = py - 2 * rxSq;
                    p = p + rySq + px - py;
                }

                this.TrySetPixel(xc + x, yc + y, color);
                this.TrySetPixel(xc - x, yc + y, color);
                this.TrySetPixel(xc + x, yc - y, color);
                this.TrySetPixel(xc - x, yc - y, color);
            }

            //Region 2
            p = rySq * (x + (float)0.5) * (x + (float)0.5) + rxSq * (y - 1) * (y - 1) - rxSq * rySq;
            while (y > 0)
            {
                y--;
                py = py - 2 * rxSq;
                if (p > 0)
                    p = p + rxSq - py;
                else
                {
                    x++;
                    px = px + 2 * rySq;
                    p = p + rxSq - py + px;
                }

                this.TrySetPixel(xc + x, yc + y, color);
                this.TrySetPixel(xc - x, yc + y, color);
                this.TrySetPixel(xc + x, yc - y, color);
                this.TrySetPixel(xc - x, yc - y, color);
            }
        }
        
        public void Ellipse_Bresenham(IntPoint center, int radiusX, int radiusY, Color color)
            => this.Ellipse_Bresenham(center.X, center.Y, radiusX, radiusY, ColorTool.ColorToInt(color));
        public void Ellipse_Bresenham(int xc, int yc, int radiusX, int radiusY, Color color)
            => this.Ellipse_Bresenham(xc, yc, radiusX, radiusY, ColorTool.ColorToInt(color));
        public void Ellipse_Bresenham(int xc, int yc, int radiusX, int radiusY, byte alpha, byte red, byte green, byte blue)
            => this.Ellipse_Bresenham(xc, yc, radiusX, radiusY, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Ellipse_Bresenham(int xc, int yc, int radiusX, int radiusY, int color)
        {
            // TODO
        }

        public void FloodFill_BF4_Recursive(int x, int y, Color border, Color fill)
            => this.FloodFill_BF4_Recursive(x, y, ColorTool.ColorToInt(border), ColorTool.ColorToInt(fill));
        public void FloodFill_BF4_Recursive(int x, int y, byte border_alpha, byte border_red, byte border_green, byte border_blue, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.FloodFill_BF4_Recursive(x, y, ColorTool.ArgbToInt(border_alpha, border_red, border_green, border_blue), ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void FloodFill_BF4_Recursive(int x, int y, int border_color,int fill_color)
        {
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int present_color = this.GetPixeli(x, y);
            
            if (present_color != border_color && present_color != fill_color)
            {
                this.SetPixel(x, y, fill_color);
                this.FloodFill_BF4_Recursive(x + 1, y, border_color, fill_color);
                this.FloodFill_BF4_Recursive(x - 1, y, border_color, fill_color);
                this.FloodFill_BF4_Recursive(x, y + 1, border_color, fill_color);
                this.FloodFill_BF4_Recursive(x, y - 1, border_color, fill_color);
            }
        }
    }
}
