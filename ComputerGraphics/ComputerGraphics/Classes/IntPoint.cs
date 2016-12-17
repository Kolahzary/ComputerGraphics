using System;

namespace ComputerGraphics.Classes
{
    public static class Point2IntPoint
    {
        public static IntPoint ToIntPoint(this System.Windows.Point p) => new IntPoint(p);
        public static IntPoint ToIntPointWithResolution(this System.Windows.Point p, double xResolution, double yResolution) => new IntPoint(p, xResolution, yResolution);
    }
    public struct IntPoint
    {
        public IntPoint(System.Windows.Point p, double xResolution, double yResolution)
            : this((int)(p.X / 96.0 * xResolution), (int)(p.Y / 96.0 * yResolution))
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
