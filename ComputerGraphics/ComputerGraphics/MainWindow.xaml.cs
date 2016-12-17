using ComputerGraphics.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Timers;
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

        #region Current Back Color
        private Color _CurrentBackColor;
        public Color CurrentBackColor
        {
            get { return _CurrentBackColor; }
            set
            {
                this._CurrentBackColor = value;
                this.NotifyPropertyChanged("CurrentBackColorBrush");
                this.NotifyPropertyChanged("CurrentNegativeBackColorBrush");
            }
        }
        public Brush CurrentBackColorBrush => new SolidColorBrush(this.CurrentBackColor);
        public Color CurrentNegativeBackColor => this.CurrentBackColor.GetNegative();
        public Brush CurrentNegativeBackColorBrush => new SolidColorBrush(this.CurrentNegativeBackColor);
        #endregion
        #region Current Fore Color
        private Color _CurrentForeColor;
        public Color CurrentForeColor
        {
            get { return _CurrentForeColor; }
            set
            {
                this._CurrentForeColor = value;
                this.NotifyPropertyChanged("CurrentForeColorBrush");
                this.NotifyPropertyChanged("CurrentNegativeForeColorBrush");
            }
        }
        public Brush CurrentForeColorBrush => new SolidColorBrush(this.CurrentForeColor);
        public Color CurrentNegativeForeColor => this.CurrentForeColor.GetNegative();
        public Brush CurrentNegativeForeColorBrush => new SolidColorBrush(this.CurrentNegativeForeColor);
        #endregion

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

            Diamond,
            Pentagon,
            Hexagon,

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

            Fill_FF4_Dynamic,
            Fill_FF8_Dynamic,

            Etc_Arrow,
        }

        public void ToolSelected(object sender, RoutedEventArgs e)
            => this.CurrentTool = (ToolType)Enum.Parse(typeof(ToolType), (string)(sender as Control).Tag);

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

            {ToolType.Diamond,"Diamond" },
            {ToolType.Pentagon,"Pentagon" },
            {ToolType.Hexagon,"Hexagon" },

            {ToolType.Triangle_Equilateral,"Equilateral Triangle" },
            {ToolType.Triangle_Isosceles,"Isosceles Triangle" },
            {ToolType.Triangle_Right,"Right Triangle" },

            {ToolType.Circle_Midpoint,"Circle -> Midpoint" },
            {ToolType.Circle_Bresenham,"Circle -> Bresenham" },

            {ToolType.Ellipse_Midpoint,"Circle -> Midpoint" },
            {ToolType.Ellipse_BresenhamRect,"Circle -> Bresenham Rect" },

            {ToolType.Fill_BF4_Recursive,"Fill -> 4-Way Boundary Fill Recursive" },
            {ToolType.Fill_BF8_Recursive,"Fill -> 8-Way Boundary Fil Recursive" },

            {ToolType.Fill_FF4_Recursive,"Fill -> 4-Way Recursive Flood Fill" },
            {ToolType.Fill_FF8_Recursive,"Fill -> 8-Way Recursive Flood Fill" },

            {ToolType.Fill_FF4_Dynamic,"Fill -> 4-Way Dynamic Flood Fill" },
            {ToolType.Fill_FF8_Dynamic,"Fill -> 8-Way Dynamic Flood Fill" },

            {ToolType.Etc_Arrow,"Etc -> Arrow" },

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

        public IntPoint MousePosition => Mouse.GetPosition(imgMain).ToIntPointWithResolution(bmp.XResolution, bmp.YResolution);
        public string StringImageSize => $"{bmp.Width} * {bmp.Height} px";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bmp = new Bgra32BitmapTool(100, 100, 10);
            bmp.PropertyChanged += Bmp_PropertyChanged;
            bmp.SaveCheckpoint();

            this.CurrentBackColor = Colors.White;
            this.CurrentForeColor = Colors.Black;

            this.CurrentTool = ToolType.Freehand_DrawLine;

        }

        private void Bmp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WritableBitmap")
            {
                imgMain.Source = bmp.WritableBitmap;
                this.NotifyPropertyChanged("StringImageSize");
            }
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

                    case ToolType.Diamond:
                    case ToolType.Pentagon:
                    case ToolType.Hexagon:

                    case ToolType.Triangle_Equilateral:
                    case ToolType.Triangle_Isosceles:
                    case ToolType.Triangle_Right:

                    case ToolType.Circle_Midpoint:
                    case ToolType.Circle_Bresenham:

                    case ToolType.Ellipse_Midpoint:
                    case ToolType.Ellipse_BresenhamRect:

                    case ToolType.Etc_Arrow:
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
                        break;
                    case ToolType.Fill_BF8_Recursive:
                        bmp.Fill_BF8_Recursive(mouse.X, mouse.Y, this.CurrentForeColor, this.CurrentBackColor);
                        break;
                    case ToolType.Fill_FF4_Recursive:
                        bmp.Fill_FF4_Recursive(mouse.X, mouse.Y, this.CurrentBackColor);
                        break;
                    case ToolType.Fill_FF8_Recursive:
                        bmp.Fill_FF8_Recursive(mouse.X, mouse.Y, this.CurrentBackColor);
                        break;
                    case ToolType.Fill_FF4_Dynamic:
                        bmp.Fill_FF4_Dynamic(mouse.X, mouse.Y, this.CurrentBackColor);
                        break;
                    case ToolType.Fill_FF8_Dynamic:
                        bmp.Fill_FF8_Dynamic(mouse.X, mouse.Y, this.CurrentBackColor);
                        break;
                    default:
                        break;
                }
                if (!this.SourcePoint.HasValue)
                {
                    bmp.Apply();
                    bmp.SaveCheckpoint(); // for history
                    return;
                }
                switch (CurrentTool)
                {
                    //case ToolType.Freehand_PutPixels:
                    //    break;
                    //case ToolType.Freehand_DrawLine:
                    //    break;

                    case ToolType.Line_Naive:
                        bmp.Line_Naive(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Line_DDA:
                        bmp.Line_DDA(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Line_Bresenham:
                        bmp.Line_Bresenham(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Square_Empty:
                        bmp.Square_Empty(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Square_Filled:
                        bmp.Square_Filled(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Rectangle_Empty:
                        bmp.Rectangle_Empty(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Rectangle_Filled:
                        bmp.Rectangle_Filled(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Diamond:
                        bmp.Diamond(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Pentagon:
                        bmp.Pentagon(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Hexagon:
                        bmp.Hexagon(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Triangle_Equilateral:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Triangle_Equilateral(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        break;
                    case ToolType.Triangle_Isosceles:
                        bmp.Triangle_Isosceles(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Triangle_Right:
                        bmp.Triangle_Right(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;
                    case ToolType.Circle_Midpoint:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Circle_Midpoint(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        break;
                    case ToolType.Circle_Bresenham:
                        radius = Math.Max(Math.Abs(mouse.X - this.SourcePoint.Value.X), Math.Abs(mouse.Y - this.SourcePoint.Value.Y));

                        bmp.Circle_Bresenham(this.SourcePoint.Value, radius, this.CurrentForeColor);
                        break;
                    case ToolType.Ellipse_Midpoint:
                        radiusX = Math.Abs(mouse.X - this.SourcePoint.Value.X);
                        radiusY = Math.Abs(mouse.Y - this.SourcePoint.Value.Y);

                        bmp.Ellipse_Midpoint(this.SourcePoint.Value, radiusX, radiusY, this.CurrentForeColor);
                        break;
                    case ToolType.Ellipse_BresenhamRect:
                        bmp.Ellipse_BresenhamRect(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;

                    case ToolType.Etc_Arrow:
                        bmp.Etc_Arrow(this.SourcePoint.Value, mouse, this.CurrentForeColor);
                        break;

                    default:
                        break;
                }

                bmp.Apply();
                bmp.SaveCheckpoint(); // for history

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
            cp.PickedColor = this.CurrentForeColor;
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
            cp.PickedColor = this.CurrentBackColor;
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
            bmp.SaveCheckpoint();
        }

        private RoutedUICommand GetCommand(string cmdName)
        {
            foreach (CommandBinding item in this.CommandBindings)
                if ((item.Command as RoutedUICommand).Name == cmdName)
                    return item.Command as RoutedUICommand;
            return null;
        }
        private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RoutedUICommand cmd = e.Command as RoutedUICommand;

            switch (cmd.Name)
            {
                case "Undo": bmp.Undo(); break;
                case "Redo": bmp.Redo(); break;
                case "Close": this.Close(); break;
                case "New":
                    SizeDialogBox sdb = new SizeDialogBox()
                    {
                        Values_Width = bmp.Width,
                        Values_Height = bmp.Height,
                        Values_XResolution = bmp.XResolution,
                        Values_YResolution = bmp.YResolution
                    };

                    sdb.Owner = this;
                    var res = sdb.ShowDialog();

                    if (res.HasValue)
                    {
                        if (res.Value)
                        {
                            bmp = new Bgra32BitmapTool(
                                sdb.Values_Width,
                                sdb.Values_Height,
                                sdb.Values_XResolution,
                                sdb.Values_YResolution
                                );
                            bmp.PropertyChanged += Bmp_PropertyChanged;
                            this.CurrentFilePath = null;
                        }
                    }
                    break;
                case "Open":
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Supported image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff;*.ico|All files (*.*)|*.*";
                    if (ofd.ShowDialog() == true)
                    {
                        bmp = new Bgra32BitmapTool(new Uri(ofd.FileName, UriKind.RelativeOrAbsolute));
                        bmp.PropertyChanged += Bmp_PropertyChanged;
                        this.CurrentFilePath = ofd.FileName;
                    }
                    break;
                case "SaveAs":
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Supported image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff;*.ico)|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff;*.ico|All files (*.*)|*.*";
                    if (sfd.ShowDialog() == true)
                    {
                        this.CurrentFilePath = sfd.FileName;
                        this.GetCommand("Save")?.Execute(null, null);
                    }
                    break;
                case "Save":
                    if (string.IsNullOrEmpty(this.CurrentFilePath))
                    {
                        this.GetCommand("SaveAs")?.Execute(null, null);
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
                    break;
                default:
                    break;
            }
        }

        private void wMainWindow_Closed(object sender, EventArgs e)
        {
            bmp.Dispose();
        }

        private void ActionSelected(object sender, RoutedEventArgs e)
        {
            ActionType at = (ActionType)Enum.Parse(typeof(ActionType), (string)(sender as Control).Tag);
            if (at == ActionType.Scale || at == ActionType.ChangeCanvasSize)
            {
                SizeDialogBox sdb = new SizeDialogBox(at == ActionType.Scale)
                {
                    Values_Width = bmp.Width,
                    Values_Height = bmp.Height,
                    Values_XResolution = bmp.XResolution,
                    Values_YResolution = bmp.YResolution
                };
                sdb.Owner = this;
                var res = sdb.ShowDialog();
                if (res.HasValue)
                {
                    if (res.Value)
                    {
                        if (at == ActionType.Scale)
                            bmp.Scale(sdb.Values_Width, sdb.Values_Height, sdb.Values_XResolution, sdb.Values_YResolution);
                        else
                            bmp.ChangeCanvasSize(sdb.Values_Width, sdb.Values_Height);

                        bmp.Apply();
                        bmp.SaveCheckpoint();
                    }
                }
            }
            switch (at)
            {
                case ActionType.Rotate_90C:
                    bmp.Rotate_90C();
                    break;
                case ActionType.Rotate_90CC:
                    bmp.Rotate_90CC();
                    break;
                case ActionType.Rotate_180:
                    bmp.Rotate_180();
                    break;
                case ActionType.Flip_Horizontal:
                    bmp.Flip_Horizontal();
                    break;
                case ActionType.Flip_Vertical:
                    bmp.Flip_Vertical();
                    break;
                default:
                    break;
            }
            bmp.Apply();
            bmp.SaveCheckpoint();
        }
        private enum ActionType
        {
            Scale,
            ChangeCanvasSize,

            Rotate_90C,
            Rotate_90CC,
            Rotate_180,

            Flip_Horizontal,
            Flip_Vertical,
        }

        #region EasterEgg
        private Timer EasterEgg_Timer;
        private bool EasterEgg_isGrowing = true;
        private const int EasterEgg_MaxGrow = 3;
        private int EasterEgg_Counter = 0;
        private void MenuItem_EasterEgg_Click(object sender, RoutedEventArgs e)
        {
            if (EasterEgg_Timer != null)
            {
                EasterEgg_Timer.Stop();
                EasterEgg_Timer = null;
                EasterEgg_Counter = 0;
                EasterEgg_isGrowing = true;
                return;
            }
            bmp = new Bgra32BitmapTool(
                width: 113,
                height: 213,
                xResolution: 96.0,
                yResolution: 96.0);
            bmp.PropertyChanged += Bmp_PropertyChanged;

            bmp.Ellipse_BresenhamRect(10, 10, 100, 100, Colors.Orange);

            bmp.Circle_Bresenham(40, 40, 5, Colors.Orange);
            bmp.Fill_FF4_Dynamic(40, 40, Colors.LightGray);

            bmp.Circle_Bresenham(70, 40, 5, Colors.Orange);
            bmp.Fill_FF4_Dynamic(70, 40, Colors.LightGray);

            bmp.QuadraticBezier(30, 75, 55, 95, 80, 75, Colors.Orange);

            bmp.Rectangle_Empty(1, 100, 111, 210, Colors.Blue);
            bmp.Rectangle_Empty(16, 115, 96, 195, Colors.Blue);
            bmp.Fill_FF4_Dynamic(2, 101, Colors.SteelBlue);

            bmp.Apply();
            bmp.SaveCheckpoint();
            EasterEgg_Timer = new Timer(2 * 1000);
            EasterEgg_Timer.Elapsed += (_sender, _e) =>
            {
                if (EasterEgg_isGrowing)
                    if (EasterEgg_Counter < EasterEgg_MaxGrow)
                        EasterEgg_Counter++;
                    else
                    {
                        EasterEgg_isGrowing = false;
                        EasterEgg_Counter--;
                    }
                else
                    if (EasterEgg_Counter > 0)
                    EasterEgg_Counter--;
                else
                {
                    EasterEgg_isGrowing = true;
                    EasterEgg_Counter++;
                }
                this.Dispatcher.Invoke(delegate
                {
                    bmp.Rotate_90C();
                    if (EasterEgg_isGrowing)
                        bmp.Scale(bmp.Width * 2, bmp.Height * 2, bmp.XResolution, bmp.YResolution);
                    else
                        bmp.Scale(bmp.Width / 2, bmp.Height / 2, bmp.XResolution, bmp.YResolution);
                    bmp.Apply();
                    bmp.SaveCheckpoint();
                });
            };
            EasterEgg_Timer.Start();
        }
        #endregion
    }
}
