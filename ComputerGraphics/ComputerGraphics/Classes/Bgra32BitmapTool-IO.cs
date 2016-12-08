using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Classes
{
    public partial class Bgra32BitmapTool
    {
        public void SaveBmp(string savePath)
            => this.Save(new BmpBitmapEncoder(), savePath);
        public void SaveGif(string savePath)
            => this.Save(new GifBitmapEncoder(), savePath);
        public void SaveJpeg(string savePath)
            => this.Save(new JpegBitmapEncoder(), savePath);
        public void SavePng(string savePath)
            => this.Save(new PngBitmapEncoder(), savePath);
        public void SaveTiff(string savePath)
            => this.Save(new TiffBitmapEncoder(), savePath);
        public void SaveWmp(string savePath)
            => this.Save(new WmpBitmapEncoder(), savePath);

        private void Save(BitmapEncoder be, string savePath)
        {
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                this.Save(be, stream);
            }
        }

        private void Save(BitmapEncoder be, FileStream fs)
        {
            be.Frames.Add(BitmapFrame.Create(this.WritableBitmap));
            be.Save(fs);
        }
    }
}
