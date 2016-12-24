using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
/// <summary>
/// Thanks to http://members.chello.at/easyfilter/bresenham.html
/// </summary>
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
            if (this.LineWidth != 1)
            {
                this.LineWidth_AA_Bresenham(x0, y0, x1, y1, ColorTool.IntToColor(color));
                return;
            }
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
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2; /* error value e_xy */

            for (;;)
            {  /* loop */
                this.TrySetPixel(x0, y0, color);
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
                if (e2 <= dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
            }
        }
        public float LineWidth { get; set; } = 1;
        public void LineWidth_AA_Bresenham(int x0, int y0, int x1, int y1, Color color)
             => this.LineWidth_AA_Bresenham(x0, y0, x1, y1, color.A, color.R, color.G, color.B);
        public void LineWidth_AA_Bresenham(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.LineWidth_AA_Bresenham(x0, y0, x1, y1, this.LineWidth, alpha, red, green, blue);
        public void LineWidth_AA_Bresenham(IntPoint source, IntPoint destination, Color color)
            => this.LineWidth_AA_Bresenham(source.X, source.Y, destination.X, destination.Y, color.A, color.R, color.G, color.B);
        public void LineWidth_AA_Bresenham(IntPoint source, IntPoint destination, float width, Color color)
            => this.LineWidth_AA_Bresenham(source.X, source.Y, destination.X, destination.Y, width, color.A, color.R, color.G, color.B);
        public void LineWidth_AA_Bresenham(int x0, int y0, int x1, int y1, float width, Color color)
            => this.LineWidth_AA_Bresenham(x0, y0, x1, y1, width, color.A, color.R, color.G, color.B);
        public void LineWidth_AA_Bresenham(int x0, int y0, int x1, int y1, float width, byte alpha, byte red, byte green, byte blue)
        //    => this.LineWidth_AA_Bresenham(x0, y0, x1, y1, width, ColorTool.ArgbToInt(alpha, red, green, blue));
        //public void LineWidth_AA_Bresenham(int x0, int y0, int x1, int y1, float width, int color)
        {
            /* plot an anti-aliased line of width wd */
            float tmp;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx - dy, e2, x2, y2; /* error value e_xy */
            float ed = dx + dy == 0 ? 1 : (float)Math.Sqrt((float)dx * dx + (float)dy * dy);
            for (width = (width + 1) / 2; ;) { /* pixel loop */
                tmp = 255-Math.Max(0, 255 * (Math.Abs(err - dx + dy) / ed - width + 1));
                this.TrySetPixel(x0, y0, (byte)tmp, red, green, blue);
                e2 = err;
                x2 = x0;
                if (2 * e2 >= -dx) { /* x step */
                    for (e2 += dy, y2 = y0; e2 < ed * width && (y1 != y2 || dx > dy); e2 += dx)
                    {
                        tmp = 255-Math.Max(0, 255 * (Math.Abs(e2) / ed - width + 1));
                        this.TrySetPixel(x0, y2 += sy, (byte)tmp, red, green, blue);
                    }
                    if (x0 == x1) break;
                    e2 = err;
                    err -= dy;
                    x0 += sx;
                }
                if (2 * e2 <= dy) { /* y step */
                    for (e2 = dx - e2; e2 < ed * width && (x1 != x2 || dx < dy); e2 += dy)
                    {
                        tmp = 255 - Math.Max(0, 255 * (Math.Abs(e2) / ed - width + 1));
                        this.TrySetPixel(x2 += sx, y0, (byte)tmp, red, green, blue);
                    }
                    if (y0 == y1) break;
                    err += dx;
                    y0 += sy;
                }
            }
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
        public void Diamond(IntPoint source, IntPoint destination, Color color)
            => this.Diamond(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Diamond(int x0, int y0, int x1, int y1, Color color)
            => this.Diamond(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Diamond(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Diamond(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Diamond(int x0, int y0, int x1, int y1, int color)
        {
            int ax = (x0 + x1) / 2,
                ay = (y0 + y1) / 2,
                dx = x1 - x0,
                dy = y1 - y0;

            this.Line_DDA(x0 - dx / 2, ay, ax, y0 - dy / 2, color);
            this.Line_DDA(ax, y0 - dy / 2, x1 + dx / 2, ay, color);

            this.Line_DDA(x1 + dx / 2, ay, ax, y1 + dy / 2, color);
            this.Line_DDA(ax, y1 + dy / 2, x0 - dx / 2, ay, color);
        }

        public void Pentagon(IntPoint source, IntPoint destination, Color color)
            => this.Pentagon(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Pentagon(int x0, int y0, int x1, int y1, Color color)
            => this.Pentagon(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Pentagon(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Pentagon(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Pentagon(int x0, int y0, int x1, int y1, int color)
        {
            int ax = (x0 + x1) / 2,
                dx = x1 - x0,
                dy = y1 - y0;


            this.Line_DDA(x0, y0 + dy / 4, ax, y0 - dy / 2, color);
            this.Line_DDA(ax, y0 - dy / 2, x1, y0 + dy / 4, color);


            this.Line_DDA(x0, y0 + dy / 4, x0 + dx / 4, y1, color);

            this.Line_DDA(x1 - dx / 4, y1, x1, y0 + dy / 4, color);
            this.Line_DDA(x1 - dx / 4, y1, x0 + dx / 4, y1, color);
        }
        public void Hexagon(IntPoint source, IntPoint destination, Color color)
            => this.Hexagon(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Hexagon(int x0, int y0, int x1, int y1, Color color)
            => this.Hexagon(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Hexagon(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Hexagon(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Hexagon(int x0, int y0, int x1, int y1, int color)
        {
            int dx = x1 - x0,
                ay = (y0 + y1) / 2;

            this.Line_DDA(x0, y0, x1, y0, color);

            this.Line_DDA(x1, y0, x1 + dx / 2, ay, color);

            this.Line_DDA(x1 + dx / 2, ay, x1, y1, color);

            this.Line_DDA(x1, y1, x0, y1, color);

            this.Line_DDA(x0, y1, x0 - dx / 2, ay, color);

            this.Line_DDA(x0 - dx / 2, ay, x0, y0, color);
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
                xc - radius, yc + radius,
                xc + radius, yc + radius,
                xc, yc - radius,
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
        public void Etc_Arrow(IntPoint source, IntPoint destination, Color color)
            => this.Etc_Arrow(source.X, source.Y, destination.X, destination.Y, ColorTool.ColorToInt(color));
        public void Etc_Arrow(int x0, int y0, int x1, int y1, Color color)
            => this.Etc_Arrow(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Etc_Arrow(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Etc_Arrow(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Etc_Arrow(int x0, int y0, int x1, int y1, int color)
        {
            int dx = x1 - x0,
                dy = y1 - y0;

            this.Rectangle_Empty(x0, y0, x1, y1, color);

            this.Triangle(
                x1, y1 - 3 * dy / 2,
                x1, y1 + dy / 2,
                x1 + dx / 2, y1 - dy / 2,
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
            int x = -radius, y = 0, err = 2 - 2 * radius; /* II. Quadrant */
            do
            {
                this.TrySetPixel(xc - x, yc + y, color); /*   I. Quadrant */
                this.TrySetPixel(xc - y, yc - x, color); /*  II. Quadrant */
                this.TrySetPixel(xc + x, yc - y, color); /* III. Quadrant */
                this.TrySetPixel(xc + y, yc + x, color); /*  IV. Quadrant */
                radius = err;
                if (radius <= y) err += ++y * 2 + 1;           /* e_xy+e_y < 0 */
                if (radius > x || err > y) err += ++x * 2 + 1; /* e_xy+e_x > 0 or no 2nd y-step */
            } while (x < 0);
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

        public void Ellipse_BresenhamRect(IntPoint source, IntPoint target, Color color)
            => this.Ellipse_BresenhamRect(source.X, source.Y, target.X, target.Y, ColorTool.ColorToInt(color));
        public void Ellipse_BresenhamRect(int x0, int y0, int x1, int y1, Color color)
            => this.Ellipse_BresenhamRect(x0, y0, x1, y1, ColorTool.ColorToInt(color));
        public void Ellipse_BresenhamRect(int x0, int y0, int x1, int y1, byte alpha, byte red, byte green, byte blue)
            => this.Ellipse_BresenhamRect(x0, y0, x1, y1, ColorTool.ArgbToInt(alpha, red, green, blue));
        public void Ellipse_BresenhamRect(int x0, int y0, int x1, int y1, int color)
        {
            int a = Math.Abs(x1 - x0), b = Math.Abs(y1 - y0), b1 = b & 1; /* values of diameter */
            long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; /* error increment */
            long err = dx + dy + b1 * a * a, e2; /* error of 1.step */

            if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
            if (y0 > y1) y0 = y1; /* .. exchange them */
            y0 += (b + 1) / 2; y1 = y0 - b1;   /* starting pixel */
            a *= 8 * a; b1 = 8 * b * b;

            do
            {
                this.TrySetPixel(x1, y0, color); /*   I. Quadrant */
                this.TrySetPixel(x0, y0, color); /*  II. Quadrant */
                this.TrySetPixel(x0, y1, color); /* III. Quadrant */
                this.TrySetPixel(x1, y1, color); /*  IV. Quadrant */
                e2 = 2 * err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */
                if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; } /* x step */
            } while (x0 <= x1);

            while (y0 - y1 < b)
            {  /* too early stop of flat ellipses a=1 */
                this.TrySetPixel(x0 - 1, y0, color); /* -> finish tip of ellipse */
                this.TrySetPixel(x1 + 1, y0++, color);
                this.TrySetPixel(x0 - 1, y1, color);
                this.TrySetPixel(x1 + 1, y1--, color);
            }
        }

        public void Fill_BF4_Recursive(int x, int y, Color border, Color fill)
            => this.Fill_BF4_Recursive(x, y, ColorTool.ColorToInt(border), ColorTool.ColorToInt(fill));
        public void Fill_BF4_Recursive(int x, int y, byte border_alpha, byte border_red, byte border_green, byte border_blue, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_BF4_Recursive(x, y, ColorTool.ArgbToInt(border_alpha, border_red, border_green, border_blue), ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_BF4_Recursive(int x, int y, int border_color, int fill_color)
        {
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int present_color = this.GetPixeli(x, y);

            if (present_color != border_color && present_color != fill_color)
            {
                this.SetPixel(x, y, fill_color);
                foreach (IntPoint adjucent in AdjacentFinder4(x, y))
                {
                    this.Fill_BF8_Recursive(adjucent.X, adjucent.Y, border_color, fill_color);
                }
                //this.Fill_BF4_Recursive(x + 1, y, border_color, fill_color);
                //this.Fill_BF4_Recursive(x - 1, y, border_color, fill_color);
                //this.Fill_BF4_Recursive(x, y + 1, border_color, fill_color);
                //this.Fill_BF4_Recursive(x, y - 1, border_color, fill_color);
            }
        }



        public void Fill_BF8_Recursive(int x, int y, Color border, Color fill)
            => this.Fill_BF8_Recursive(x, y, ColorTool.ColorToInt(border), ColorTool.ColorToInt(fill));
        public void Fill_BF8_Recursive(int x, int y, byte border_alpha, byte border_red, byte border_green, byte border_blue, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_BF8_Recursive(x, y, ColorTool.ArgbToInt(border_alpha, border_red, border_green, border_blue), ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_BF8_Recursive(int x, int y, int border_color, int fill_color)
        {
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int present_color = this.GetPixeli(x, y);

            if (present_color != border_color && present_color != fill_color)
            {
                this.SetPixel(x, y, fill_color);
                foreach (IntPoint adjucent in AdjacentFinder8(x, y))
                {
                    this.Fill_BF8_Recursive(adjucent.X, adjucent.Y, border_color, fill_color);
                }
                //this.Fill_BF8_Recursive(x + 1, y, border_color, fill_color);
                //this.Fill_BF8_Recursive(x - 1, y, border_color, fill_color);

                //this.Fill_BF8_Recursive(x, y + 1, border_color, fill_color);
                //this.Fill_BF8_Recursive(x, y - 1, border_color, fill_color);

                //this.Fill_BF8_Recursive(x + 1, y + 1, border_color, fill_color);
                //this.Fill_BF8_Recursive(x + 1, y - 1, border_color, fill_color);

                //this.Fill_BF8_Recursive(x - 1, y + 1, border_color, fill_color);
                //this.Fill_BF8_Recursive(x - 1, y - 1, border_color, fill_color);
            }
        }

        public void Fill_FF4_Recursive(int x, int y, Color fill)
            => this.Fill_FF4_Recursive(x, y, ColorTool.ColorToInt(fill));
        public void Fill_FF4_Recursive(int x, int y, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_FF4_Recursive(x, y, ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_FF4_Recursive(int x, int y, int fill_color)
            => this.Fill_FF4_Recursive(x, y, this.GetPixeli(x, y), fill_color);
        public void Fill_FF4_Recursive(int x, int y, int old_color, int fill_color)
        {
            if (old_color == fill_color) return; // if old & new are equal, there's nothing to do by this algorithm!
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int present_color = this.GetPixeli(x, y);

            if (present_color == old_color)
            {
                this.SetPixel(x, y, fill_color);

                foreach (IntPoint adjucent in AdjacentFinder4(x, y))
                {
                    this.Fill_FF4_Recursive(adjucent.X, adjucent.Y, old_color, fill_color);
                }
                //this.Fill_FF4_Recursive(x + 1, y, old_color, fill_color);
                //this.Fill_FF4_Recursive(x - 1, y, old_color, fill_color);

                //this.Fill_FF4_Recursive(x, y + 1, old_color, fill_color);
                //this.Fill_FF4_Recursive(x, y - 1, old_color, fill_color);
            }
        }

        public void Fill_FF8_Recursive(int x, int y, Color fill)
            => this.Fill_FF8_Recursive(x, y, ColorTool.ColorToInt(fill));
        public void Fill_FF8_Recursive(int x, int y, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_FF8_Recursive(x, y, ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_FF8_Recursive(int x, int y, int fill_color)
            => this.Fill_FF8_Recursive(x, y, this.GetPixeli(x, y), fill_color);
        public void Fill_FF8_Recursive(int x, int y, int old_color, int fill_color)
        {
            if (old_color == fill_color) return; // if old & new are equal, there's nothing to do by this algorithm!
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int present_color = this.GetPixeli(x, y);

            if (present_color == old_color)
            {
                this.SetPixel(x, y, fill_color);

                foreach (IntPoint adjucent in AdjacentFinder8(x, y))
                {
                    this.Fill_FF8_Recursive(adjucent.X, adjucent.Y, old_color, fill_color);
                }
                //this.Fill_FF8_Recursive(x + 1, y, old_color, fill_color);
                //this.Fill_FF8_Recursive(x - 1, y, old_color, fill_color);

                //this.Fill_FF8_Recursive(x, y + 1, old_color, fill_color);
                //this.Fill_FF8_Recursive(x, y - 1, old_color, fill_color);

                //this.Fill_FF8_Recursive(x + 1, y + 1, old_color, fill_color);
                //this.Fill_FF8_Recursive(x + 1, y - 1, old_color, fill_color);

                //this.Fill_FF8_Recursive(x - 1, y + 1, old_color, fill_color);
                //this.Fill_FF8_Recursive(x - 1, y - 1, old_color, fill_color);
            }
        }


        public void Fill_FF4_Dynamic(int x, int y, Color fill)
            => this.Fill_FF4_Dynamic(x, y, ColorTool.ColorToInt(fill));
        public void Fill_FF4_Dynamic(int x, int y, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_FF4_Dynamic(x, y, ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_FF4_Dynamic(int x, int y, int fill_color)
        {
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int old_color = this.GetPixeli(x, y);
            if (old_color == fill_color) return; // if old & new are equal, there's nothing to do by this algorithm!

            Stack<IntPoint> st = new Stack<IntPoint>();
            st.Push(new IntPoint(x, y));

            IntPoint cp; // current point

            while (st.Count > 0)
            {
                cp = st.Pop();
                this.SetPixel(cp.X, cp.Y, fill_color);

                foreach (IntPoint adjacent in AdjacentFinder4(cp))
                {
                    if (!this.IsAllowd(adjacent.X, adjacent.Y)) continue; // make sure it doesn't get out of canvas

                    if (this.GetPixeli(adjacent.X, adjacent.Y) == old_color)
                    {
                        st.Push(adjacent);
                    }
                }
            }
        }
        public void Fill_FF8_Dynamic(int x, int y, Color fill)
            => this.Fill_FF8_Dynamic(x, y, ColorTool.ColorToInt(fill));
        public void Fill_FF8_Dynamic(int x, int y, byte fill_alpha, byte fill_red, byte fill_green, byte fill_blue)
            => this.Fill_FF8_Dynamic(x, y, ColorTool.ArgbToInt(fill_alpha, fill_red, fill_green, fill_blue));
        public void Fill_FF8_Dynamic(int x, int y, int fill_color)
        {
            if (!this.IsAllowd(x, y)) return; // make sure it doesn't get out of canvas
            int old_color = this.GetPixeli(x, y);
            if (old_color == fill_color) return; // if old & new are equal, there's nothing to do by this algorithm!

            Stack<IntPoint> st = new Stack<IntPoint>();
            st.Push(new IntPoint(x, y));

            IntPoint cp; // current point

            while (st.Count > 0)
            {
                cp = st.Pop();
                this.SetPixel(cp.X, cp.Y, fill_color);

                foreach (IntPoint adjacent in AdjacentFinder8(cp))
                {
                    if (!this.IsAllowd(adjacent.X, adjacent.Y)) continue; // make sure it doesn't get out of canvas

                    if (this.GetPixeli(adjacent.X, adjacent.Y) == old_color)
                    {
                        st.Push(adjacent);
                    }
                }
            }
        }
        private static IEnumerable<IntPoint> AdjacentFinder4(IntPoint center) => AdjacentFinder4(center.X, center.Y);
        private static IEnumerable<IntPoint> AdjacentFinder4(int x, int y)
        {
            yield return new IntPoint(x + 1, y);
            yield return new IntPoint(x, y + 1);
            yield return new IntPoint(x - 1, y);
            yield return new IntPoint(x, y - 1);
        }
        private static IEnumerable<IntPoint> AdjacentFinder8(IntPoint center) => AdjacentFinder8(center.X, center.Y);
        private static IEnumerable<IntPoint> AdjacentFinder8(int x, int y)
        {
            yield return new IntPoint(x + 1, y);
            yield return new IntPoint(x, y + 1);
            yield return new IntPoint(x - 1, y);
            yield return new IntPoint(x, y - 1);

            yield return new IntPoint(x + 1, y + 1);
            yield return new IntPoint(x - 1, y - 1);

            yield return new IntPoint(x - 1, y + 1);
            yield return new IntPoint(x + 1, y - 1);
        }

        public void QuadraticBezier(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
            => this.QuadraticBezier(x0, y0, x1, y1, x2, y2, ColorTool.ColorToInt(color));
        /// <summary>
        /// plot any quadratic Bezier curve
        /// </summary>
        public void QuadraticBezier(int x0, int y0, int x1, int y1, int x2, int y2, int color)
        {
            int x = x0 - x1, y = y0 - y1;
            double t = x0 - 2 * x1 + x2, r;

            if ((long)x * (x2 - x1) > 0) { /* horizontal cut at P4? */
                if ((long)y * (y2 - y1) > 0) /* vertical cut at P6 too? */
                    if (Math.Abs((y0 - 2 * y1 + y2) / t * x) > Math.Abs(y)) { /* which first? */
                        x0 = x2;
                        x2 = x + x1;
                        y0 = y2;
                        y2 = y + y1; /* swap points */
                    } /* now horizontal cut at P4 comes first */
                t = (x0 - x1) / t;
                r = (1 - t) * ((1 - t) * y0 + 2.0 * t * y1) + t * t * y2; /* By(t=P4) */
                t = (x0 * x2 - x1 * x1) * t / (x0 - x1); /* gradient dP4/dx=0 */
                x = (int)Math.Floor(t + 0.5);
                y = (int)Math.Floor(r + 0.5);
                r = (y1 - y0) * (t - x0) / (x1 - x0) + y0; /* intersect P3 | P0 P1 */
                plotQuadBezierSeg(x0, y0, x, (int)Math.Floor(r + 0.5), x, y,color);
                r = (y1 - y2) * (t - x2) / (x1 - x2) + y2; /* intersect P4 | P1 P2 */
                x0 = x1 = x;
                y0 = y;
                y1 = (int)Math.Floor(r + 0.5);  /* P0 = P4, P1 = P8 */
            }
            if ((long)(y0 - y1) * (y2 - y1) > 0) { /* vertical cut at P6? */
                t = y0 - 2 * y1 + y2;
                t = (y0 - y1) / t;
                r = (1 - t) * ((1 - t) * x0 + 2.0 * t * x1) + t * t * x2; /* Bx(t=P6) */
                t = (y0 * y2 - y1 * y1) * t / (y0 - y1);   /* gradient dP6/dy=0 */
                x = (int)Math.Floor(r + 0.5);
                y = (int)Math.Floor(t + 0.5);
                r = (x1 - x0) * (t - y0) / (y1 - y0) + x0; /* intersect P6 | P0 P1 */
                plotQuadBezierSeg(x0, y0, (int)Math.Floor(r + 0.5), y, x, y,color);
                r = (x1 - x2) * (t - y2) / (y1 - y2) + x2; /* intersect P7 | P1 P2 */
                x0 = x;
                x1 = (int)Math.Floor(r + 0.5);
                y0 = y1 = y; /* P0 = P6, P1 = P7 */
            }
            plotQuadBezierSeg(x0, y0, x1, y1, x2, y2,color);  /* remaining part */
        }
        /// <summary>
        /// plot a limited quadratic Bezier segment
        /// </summary>
        private void plotQuadBezierSeg(int x0, int y0, int x1, int y1, int x2, int y2,int color)
        {
            bool tmp;
            int 
                sx = x2 - x1, 
                sy = y2 - y1;
            long 
                xx = x0 - x1, 
                yy = y0 - y1, xy; /* relative values for checks */
            double 
                dx,
                dy, 
                err, 
                cur = xx * sy - yy * sx; /* curvature */

            if (!(xx * sx <= 0 && yy * sy <= 0)) throw new Exception("sign of gradient must not change"); // assert(xx * sx <= 0 && yy * sy <= 0); /* sign of gradient must not change */

            if (sx * (long)sx + sy * (long)sy > xx * xx + yy * yy) { /* begin with longer part */
                x2 = x0;
                x0 = sx + x1;
                y2 = y0;
                y0 = sy + y1;
                cur = -cur; /* swap P0 P2 */
            }
            if (cur != 0) { /* no straight line */
                xx += sx;
                xx *= sx = x0 < x2 ? 1 : -1; /* x step direction */
                yy += sy;
                yy *= sy = y0 < y2 ? 1 : -1; /* y step direction */
                xy = 2 * xx * yy;
                xx *= xx;
                yy *= yy; /* differences 2nd degree */
                if (cur * sx * sy < 0) { /* negated curvature? */
                    xx = -xx;
                    yy = -yy;
                    xy = -xy;
                    cur = -cur;
                }
                dx = 4.0 * sy * cur * (x1 - x0) + xx - xy;  /* differences 1st degree */
                dy = 4.0 * sx * cur * (y0 - y1) + yy - xy;
                xx += xx;
                yy += yy;
                err = dx + dy + xy; /* error 1st step */
                do {
                    this.TrySetPixel(x0, y0,color); /* plot curve */
                    if (x0 == x2 && y0 == y2) return; /* last pixel -> curve finished */
                    tmp = 2 * err < dx; /* save value for test of y step */
                    
                    if (2 * err > dy)
                    {
                        x0 += sx;
                        dx -= xy;
                        err += dy += yy;
                    } /* x step */
                    if (tmp)
                    {
                        y0 += sy;
                        dy -= xy;
                        err += dx += xx;
                    } /* y step */
                } while (dy < 0 && dx > 0); /* gradient negates -> algorithm fails */
            }
            this.Line_Bresenham(x0, y0, x2, y2,color); /* plot remaining part to end */
        }
    }
}
