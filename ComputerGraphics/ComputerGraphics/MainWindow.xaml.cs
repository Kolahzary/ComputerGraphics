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
            bmp = new Bgra32BitmapTool(100,100,96.0);
            imgMain.Source = bmp.WritableBitmap;
        }

        private void MenuItem_File_New_Click(object sender, RoutedEventArgs e)
        {
            NewDialogBox ndb = new NewDialogBox();
            ndb.Owner = this;
            var res = ndb.ShowDialog();


            if (res.HasValue)
            {
                if (res.Value)
                {
                    bmp = new Bgra32BitmapTool(
                        ndb.Values_Width, 
                        ndb.Values_Height, 
                        ndb.Values_Resolution
                        );
                    imgMain.Source = bmp.WritableBitmap;
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

        private void MenuItem_ApplyBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            string name=(sender as MenuItem).Header.ToString().Replace("_","");
            var c = System.Drawing.Color.FromName(name);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    bmp.SetPixel(x, y, c.A, c.R, c.G, c.B);
                }
            }

            bmp.Apply();
        }
    }
}
