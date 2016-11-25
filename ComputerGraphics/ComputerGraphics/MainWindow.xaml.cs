using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bgra32BitmapTool bmp;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_File_New_Click(object sender, RoutedEventArgs e)
        {
            ushort width, height;
            float resolution;

            NewDialogBox ndb = new NewDialogBox();
            ndb.Owner = this;
            var res = ndb.ShowDialog();


            if (res.HasValue)
            {
                if (res.Value)
                {
                    width = ndb.Values_Width;
                    height = ndb.Values_Height;
                    resolution = ndb.Values_Resolution;

                    bmp = new Bgra32BitmapTool(width, height, resolution);


                    imgMain.Width = width;
                    imgMain.Height = height;
                    imgMain.Source = bmp.WritableBitmap;

                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            bmp.SetPixel(i, j, Colors.LightGray);
                            //tool.SetPixel(i, j, 200, 0, 200);
                        }
                    }
                    bmp.Apply();
                }
            }
        }

        private void MenuItem_File_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Help_GitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Kolahzary/ComputerGraphics");
        }

    }
}
