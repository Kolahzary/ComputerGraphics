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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_File_New_Click(object sender, RoutedEventArgs e)
        {
            /*
            var tool= new Bgra32BitmapTool(10, 10);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tool.SetPixel(i,j, Colors.LightGray);
                    //tool.SetPixel(i, j, 200, 0, 200);
                }
            }

             tool.SetPixel(9,9, Colors.Gold);
            var c = tool.GetPixel(0, 0);
            // tool.SetPixel(1, 0, Colors.Red);
            tool.Apply();
            imgMain.Source = tool.WritableBitmap;
            */
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
