using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Classes
{
    public partial class Bgra32BitmapTool : IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private WriteableBitmap wb { get; set; }

        public WriteableBitmap WritableBitmap
        {
            get
            {
                return this.wb;
            }
            set
            {
                this.wb = value;
                this.NotifyPropertyChanged("WritableBitmap");
                //this.NotifyPropertyChanged("Width");
                //this.NotifyPropertyChanged("Height");
                //this.NotifyPropertyChanged("XResolution");
                //this.NotifyPropertyChanged("YResolution");
            }
        }
        public int Width => this.WritableBitmap.PixelWidth;
        public int Height => this.WritableBitmap.PixelHeight;
        public double XResolution => this.WritableBitmap.DpiX;
        public double YResolution => this.WritableBitmap.DpiY;
        public Bgra32BitmapTool(Uri filePath)
            : this(new BitmapImage(filePath) { CreateOptions = BitmapCreateOptions.None })
        {

        }
        public Bgra32BitmapTool(BitmapSource bs)
        {
            this.WritableBitmap = new WriteableBitmap(bs);
        }
        public Bgra32BitmapTool(int width, int height)
            : this(width,height,96.0)
        {

        }
        public Bgra32BitmapTool(int width, int height, double resolutionDPI)
            : this(width,height,resolutionDPI,resolutionDPI)
        {

        }
        public Bgra32BitmapTool(int width, int height, double xResolution, double yResolution)
            : this(new WriteableBitmap(
                pixelWidth: width,
                pixelHeight: height,
                dpiX: xResolution,
                dpiY: yResolution,
                pixelFormat: PixelFormats.Bgra32,
                palette: null))
        {

        }
        public Bgra32BitmapTool(WriteableBitmap writableBitmap)
        {
            this.WritableBitmap = writableBitmap;
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
            this.WritableBitmap.Lock();
            this.WritableBitmap.AddDirtyRect(new Int32Rect(0, 0, this.Width, this.Height));
            this.WritableBitmap.Unlock();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.wb = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                this.History.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Bgra32BitmapTool()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
