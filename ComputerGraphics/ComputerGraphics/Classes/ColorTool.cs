using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics.Classes
{
    public static class ColorTool
    {
        public static Color IntToColor(int c)
        {
            return Color.FromArgb(
                a: (byte)(c >> 24),
                r: (byte)((c >> 16) & 0xFF),
                g: (byte)((c >> 8) & 0xFF),
                b: (byte)(c & 0xFF));
        }
        public static int ToInt(this Color c)
            => ColorToInt(c);
        public static int ColorToInt(Color c)
            => ArgbToInt(c.A, c.R, c.G, c.B);
        public static int ArgbToInt(byte alpha, byte red, byte green, byte blue)
            => (alpha << 24) | (red << 16) | (green << 8) | blue;
    }
}
