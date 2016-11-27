using ComputerGraphics.Classes;
using Microsoft.Win32;
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
        public IntPoint MousePosition => Mouse.GetPosition(imgMain).ToIntPointWithResolution(bmp.Resolution);
        private void UpdateStatusBar() => this.UpdateStatusBar(this.MousePosition);
        private void UpdateStatusBar(IntPoint mouse)
        {
            lblCursorPosition.Text = $"{mouse.X}, {mouse.Y} px";
            lblImageSize.Text = $"{bmp.Width} * {bmp.Height} px";
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bmp = new Bgra32BitmapTool(100,100,10);
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
            bmp.FillBackgroundColor(System.Drawing.Color.FromName(name));
            bmp.Apply();
        }
        private void imgMain_MouseMove(object sender, MouseEventArgs e)
        {
            var mouse = this.MousePosition;
            this.UpdateStatusBar(mouse);
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                bmp.SetPixel(mouse.X, mouse.Y, Colors.Black);
                
                bmp.Apply();
            }
        }

        private void imgMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = this.MousePosition;
            if (e.ChangedButton == MouseButton.Left)
            {
                bmp.SetPixel(mouse.X, mouse.Y, Colors.Black);

                bmp.Apply();
            }
        }

        private void imgMain_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void imgMain_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void imgMain_MouseLeave(object sender, MouseEventArgs e)
        {
            this.UpdateStatusBar(new IntPoint(0, 0));
        }

        private void MenuItem_File_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                bmp = new Bgra32BitmapTool(new Uri(ofd.FileName, UriKind.RelativeOrAbsolute));
                imgMain.Source = bmp.WritableBitmap;
            }
        }
    }
}
