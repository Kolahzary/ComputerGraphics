using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Classes
{
    public static class Point2IntPoint
    {
        public static IntPoint ToIntPoint(this System.Windows.Point p,double resolution) => new IntPoint(p);
        public static IntPoint ToIntPointWithResolution(this System.Windows.Point p, double resolution) => new IntPoint(p, resolution);
    }
    public struct IntPoint
    {
        public IntPoint(System.Windows.Point p, double resolution)
            : this((int)(p.X / 96.0 * resolution), (int)(p.Y / 96.0 * resolution))
        {
        }
        public IntPoint(System.Windows.Point p)
            : this((int)Math.Round(p.X), (int)Math.Round(p.Y))
        {
        }
        public IntPoint(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
