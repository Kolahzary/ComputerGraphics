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
    /// 
    public partial class MainWindow : Window
    {
        Bgra32BitmapTool bmp;

        private IntPoint? _SourcePoint;
        private IntPoint? SourcePoint
        {
            get
            {
                return _SourcePoint;
            }
            set
            {
                _SourcePoint = value;
                lblSourcePoint.Text = value.HasValue ? $"{value.Value.X}, {value.Value.Y} px" : "";
            }
        }
        private Color CurrentBackColor { get; set; }
        private Color CurrentForeColor { get; set; }
        private enum ToolType
        {
            Freehand_PutPixels,
            Freehand_DrawLine,

            Line_Naive,
            Line_DDA,
            Line_Bresenham,

            Square_Empty,
            Square_Filled,

            Rectangle_Empty,
            Rectangle_Filled,

            Triangle_Equilateral,
            Triangle_Isosceles,
            Triangle_Right,

            Circle_Midpoint,
            Circle_Bresenham,

            Ellipse_Midpoint,
            Ellipse_Bresenham,
        }
        private Dictionary<ToolType, string> ToolNames = new Dictionary<ToolType, string>()
        {
            {ToolType.Freehand_PutPixels,"Freehand -> PutPixels" },
            {ToolType.Freehand_DrawLine,"Freehand -> DrawLine" },

            {ToolType.Line_Naive,"Line -> Naive" },
            {ToolType.Line_DDA,"Line -> DDA" },
            {ToolType.Line_Bresenham,"Line -> Bresenham" },

            {ToolType.Square_Empty,"Empty Square" },
            {ToolType.Square_Filled,"Filled Square" },

            {ToolType.Rectangle_Empty,"Empty Rectangle" },
            {ToolType.Rectangle_Filled,"Filled Rectangle" },

            {ToolType.Triangle_Equilateral,"Equilateral Triangle" },
            {ToolType.Triangle_Isosceles,"Isosceles Triangle" },
            {ToolType.Triangle_Right,"Right Triangle" },

            {ToolType.Circle_Midpoint,"Circle -> Midpoint" },
            {ToolType.Circle_Bresenham,"Circle -> Bresenham" },

            {ToolType.Ellipse_Midpoint,"Circle -> Midpoint" },
            {ToolType.Ellipse_Bresenham,"Circle -> Bresenham" },
        };
        private Dictionary<string, ToolType> ToolTypeByTag = new Dictionary<string, ToolType>()
        {
            {"Freehand_PutPixels",ToolType.Freehand_PutPixels },
            {"Freehand_DrawLine",ToolType.Freehand_DrawLine },

            {"Line_Naive",ToolType.Line_Naive },
            {"Line_DDA",ToolType.Line_DDA },
            {"Line_Bresenham",ToolType.Line_Bresenham },

            {"Square_Empty",ToolType.Square_Empty },
            {"Square_Filled",ToolType.Square_Filled },

            {"Rectangle_Empty",ToolType.Rectangle_Empty },
            {"Rectangle_Filled",ToolType.Rectangle_Filled },

            {"Triangle_Equilateral",ToolType.Triangle_Equilateral },
            {"Triangle_Isosceles",ToolType.Triangle_Isosceles },
            {"Triangle_Right",ToolType.Triangle_Right },

            {"Circle_Midpoint",ToolType.Circle_Midpoint },
            {"Circle_Bresenham",ToolType.Circle_Bresenham },

            {"Ellipse_Midpoint",ToolType.Ellipse_Midpoint },
            {"Ellipse_Bresenham",ToolType.Ellipse_Bresenham },
        };
        private ToolType _CurrentTool;
        private ToolType CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                this._CurrentTool = value;
                lblToolName.Text = ToolNames[value];
            }
        }

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

            this.CurrentForeColor = Colors.Black;
            this.CurrentTool = ToolType.Freehand_DrawLine;
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

        private void imgMain_MouseMove(object sender, MouseEventArgs e)
        {
            var mouse = this.MousePosition;
            this.UpdateStatusBar(mouse);
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                switch (CurrentTool)
                {
                    case ToolType.Freehand_PutPixels:
                        bmp.SetPixel(mouse.X, mouse.Y, this.CurrentForeColor);
                        break;
                    case ToolType.Freehand_DrawLine:
                        bmp.Line_DDA(SourcePoint??mouse, mouse, this.CurrentForeColor);
                        this.SourcePoint = mouse;
                        break;
                    default:
                        break;
                }
                
                bmp.Apply();
            }
        }
        private void imgMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = this.MousePosition;
            if (e.ChangedButton == MouseButton.Left)
            {
                switch (CurrentTool)
                {
                    case ToolType.Freehand_PutPixels:
                        bmp.SetPixel(mouse.X, mouse.Y, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Freehand_DrawLine:
                        bmp.SetPixel(mouse.X, mouse.Y, this.CurrentForeColor);
                        this.SourcePoint = mouse;
                        bmp.Apply();
                        break;

                    case ToolType.Line_Naive:
                    case ToolType.Line_DDA:
                    case ToolType.Line_Bresenham:

                    case ToolType.Square_Empty:
                    case ToolType.Square_Filled:

                    case ToolType.Rectangle_Empty:
                    case ToolType.Rectangle_Filled:

                    case ToolType.Triangle_Equilateral:
                    case ToolType.Triangle_Isosceles:
                    case ToolType.Triangle_Right:

                    case ToolType.Circle_Midpoint:
                    case ToolType.Circle_Bresenham:

                    case ToolType.Ellipse_Midpoint:
                    case ToolType.Ellipse_Bresenham:
                        this.SourcePoint = mouse;
                        break;
                    default:
                        break;
                }
            }
        }

        private void imgMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mouse = this.MousePosition;
            int radius,radiusX,radiusY;
            if (e.ChangedButton == MouseButton.Left)
            {
                if (!this.SourcePoint.HasValue) return;
                switch (CurrentTool)
                {
                    //case ToolType.Freehand_PutPixels:
                    //    break;
                    //case ToolType.Freehand_DrawLine:
                    //    break;

                    case ToolType.Line_Naive:
                        bmp.Line_Naive(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Line_DDA:
                        bmp.Line_DDA(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Line_Bresenham:
                        bmp.Line_Bresenham(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Square_Empty:
                        bmp.Square_Empty(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Square_Filled:
                        bmp.Square_Filled(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Rectangle_Empty:
                        bmp.Rectangle_Empty(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Rectangle_Filled:
                        bmp.Rectangle_Filled(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Triangle_Equilateral:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Triangle_Equilateral(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Triangle_Isosceles:
                        bmp.Triangle_Isosceles(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Triangle_Right:
                        bmp.Triangle_Right(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Circle_Midpoint:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Circle_Midpoint(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Circle_Bresenham:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Circle_Bresenham(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Ellipse_Midpoint:
                        radiusX = Math.Abs(mouse.X - this.SourcePoint.Value.X);
                        radiusY = Math.Abs(mouse.Y - this.SourcePoint.Value.Y);

                        bmp.Ellipse_Midpoint(this.SourcePoint.Value, radiusX, radiusY, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    case ToolType.Ellipse_Bresenham:
                        radiusX = Math.Abs(mouse.X - this.SourcePoint.Value.X);
                        radiusY = Math.Abs(mouse.Y - this.SourcePoint.Value.Y);

                        bmp.Ellipse_Bresenham(this.SourcePoint.Value, radiusX, radiusY, this.CurrentForeColor);
                        bmp.Apply();
                        break;
                    default:
                        break;
                }
                this.SourcePoint = null;
            }
        }
        
        private void imgMain_MouseEnter(object sender, MouseEventArgs e)
        {
            this.UpdateStatusBar();

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
        private void MenuItem_ApplyBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as MenuItem).Header.ToString().Replace("_", "");
            bmp.FillBackgroundColor(System.Drawing.Color.FromName(name));
            bmp.Apply();
        }
        public void MenuItem_Tools_ToolSelected(object sender, RoutedEventArgs e)
            => this.CurrentTool = ToolTypeByTag[(string)(sender as MenuItem).Tag];
    }
}
