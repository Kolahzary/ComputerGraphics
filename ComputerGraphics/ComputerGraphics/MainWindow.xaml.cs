using ComputerGraphics.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string CurrentFilePath;
        public string CurrentFileExtension => Path.GetExtension(this.CurrentFilePath);

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private Bgra32BitmapTool _bmp;
        private Bgra32BitmapTool bmp
        {
            get
            {
                return _bmp;
            }
            set
            {
                this._bmp = value;
                imgMain.Source = bmp.WritableBitmap;
                this.NotifyPropertyChanged("StringImageSize");
            }
        }

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
            Ellipse_BresenhamRect,

            Fill_BF4_Recursive,
            Fill_BF8_Recursive,

            Fill_FF4_Recursive,
            Fill_FF8_Recursive,
        }

        public void MenuItem_Tools_ToolSelected(object sender, RoutedEventArgs e)
            => this.CurrentTool = (ToolType)Enum.Parse(typeof(ToolType), (string)(sender as MenuItem).Tag);

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
            {ToolType.Ellipse_BresenhamRect,"Circle -> Bresenham Rect" },

            {ToolType.Fill_BF4_Recursive,"Fill -> 4-Way Boundary Fill Recursive" },
            {ToolType.Fill_BF8_Recursive,"Fill -> 8-Way Boundary Fil Recursive" },

            {ToolType.Fill_FF4_Recursive,"Fill -> 4-Way Flood Fill Recursive" },
            {ToolType.Fill_FF8_Recursive,"Fill -> 8-Way Flood Fill Recursive" },
        };

        private ToolType _CurrentTool;
        public string CurrentToolName => ToolNames[this.CurrentTool];
        private ToolType CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                this._CurrentTool = value;
                this.NotifyPropertyChanged("CurrentToolName");
            }
        }

        public IntPoint MousePosition => Mouse.GetPosition(imgMain).ToIntPointWithResolution(bmp.Resolution);
        public string StringImageSize => $"{bmp.Width} * {bmp.Height} px";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bmp = new Bgra32BitmapTool(100, 100, 10);

            this.CurrentBackColor = Colors.White;
            this.CurrentForeColor = Colors.Black;

            this.CurrentTool = ToolType.Freehand_DrawLine;
        }


        private void MenuItem_Help_GitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Kolahzary/ComputerGraphics");
        }

        private void imgMain_MouseMove(object sender, MouseEventArgs e)
        {
            this.NotifyPropertyChanged("MousePosition");
            var mouse = this.MousePosition;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (CurrentTool)
                {
                    case ToolType.Freehand_PutPixels:
                        bmp.SetPixel(mouse.X, mouse.Y, this.CurrentForeColor);
                        break;
                    case ToolType.Freehand_DrawLine:
                        bmp.Line_DDA(SourcePoint ?? mouse, mouse, this.CurrentForeColor);
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
                    case ToolType.Ellipse_BresenhamRect:
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
            int radius, radiusX, radiusY;
            if (e.ChangedButton == MouseButton.Left)
            {
                switch (CurrentTool)
                {
                    case ToolType.Fill_BF4_Recursive:
                        bmp.Fill_BF4_Recursive(mouse.X, mouse.Y, this.CurrentForeColor, this.CurrentBackColor);
                        bmp.Apply();
                        break;
                    case ToolType.Fill_BF8_Recursive:
                        bmp.Fill_BF8_Recursive(mouse.X, mouse.Y, this.CurrentForeColor, this.CurrentBackColor);
                        bmp.Apply();
                        break;
                    case ToolType.Fill_FF4_Recursive:
                        bmp.Fill_FF4_Recursive(mouse.X, mouse.Y, this.CurrentBackColor);
                        bmp.Apply();
                        break;
                    case ToolType.Fill_FF8_Recursive:
                        bmp.Fill_FF8_Recursive(mouse.X, mouse.Y, this.CurrentBackColor);
                        bmp.Apply();
                        break;
                    default:
                        break;
                }
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
                    case ToolType.Ellipse_BresenhamRect:
                        bmp.Ellipse_BresenhamRect(this.SourcePoint.Value, mouse, this.CurrentForeColor);
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
            this.NotifyPropertyChanged("MousePosition");

        }
        private void imgMain_MouseLeave(object sender, MouseEventArgs e)
        {
            this.NotifyPropertyChanged("MousePosition");
        }


        private void MenuItem_Tools_ForegroundColorPicker_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker cp = new ColorPicker();
            cp.Owner = this;
            var res = cp.ShowDialog();
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.CurrentForeColor = cp.PickedColor;
                }
            }
        }

        private void MenuItem_Tools_BackgroundColorPicker_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker cp = new ColorPicker();
            cp.Owner = this;
            var res = cp.ShowDialog();
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.CurrentBackColor = cp.PickedColor;
                }
            }
        }

        private void MenuItem_Fill_BackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            bmp.FillBackgroundColor(this.CurrentBackColor);
            bmp.Apply();
        }

        private void CommandBinding_New_Executed(object sender, ExecutedRoutedEventArgs e)
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
        private void CommandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                bmp = new Bgra32BitmapTool(new Uri(ofd.FileName, UriKind.RelativeOrAbsolute));
                this.CurrentFilePath = ofd.FileName;
            }
        }
        private void CommandBinding_Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.CurrentFilePath))
            {
                this.CommandBinding_SaveAs_Executed(sender, e);
            }
            else
            {
                do
                {
                    try
                    {
                        switch (this.CurrentFileExtension)
                        {
                            case ".bmp":
                                bmp.SaveBmp(this.CurrentFilePath);
                                break;
                            case ".gif":
                                bmp.SaveGif(this.CurrentFilePath);
                                break;
                            case ".jpg":
                            case ".jpeg":
                                bmp.SaveJpeg(this.CurrentFilePath);
                                break;
                            case ".png":
                                bmp.SavePng(this.CurrentFilePath);
                                break;
                            case ".tiff":
                                bmp.SaveTiff(this.CurrentFilePath);
                                break;
                            case ".wmp":
                                bmp.SaveWmp(this.CurrentFilePath);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxResult mbr = MessageBox.Show(ex.Message, "Try again?", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        if (mbr == MessageBoxResult.Yes) continue;
                    }
                    break;
                } while (true);
            }
        }
        private void CommandBinding_SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Supported image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                this.CurrentFilePath = sfd.FileName;
                this.CommandBinding_Save_Executed(sender, e);
            }
        }
        private void CommandBinding_Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CommandBinding_Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO
        }
        private void CommandBinding_Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO
        }
    }
}
