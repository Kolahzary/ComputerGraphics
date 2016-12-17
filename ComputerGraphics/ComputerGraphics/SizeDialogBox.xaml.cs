using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for NewDialogBox.xaml
    /// </summary>
    public partial class SizeDialogBox : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        private int _width, _height;
        private double _xres, _yres;
        public int Values_Width
        {
            get
            {
                return _width;
            }
            set
            {
                this._width = value;
                this.NotifyPropertyChanged("Values_Width");
            }
        }
        public int Values_Height
        {
            get
            {
                return _height;
            }
            set
            {
                this._height = value;
                this.NotifyPropertyChanged("Values_Height");
            }
        }

        public double Values_XResolution
        {
            get
            {
                return _xres;
            }
            set
            {
                this._xres = value;
                this.NotifyPropertyChanged("Values_XResolution");
            }
        }
        public double Values_YResolution
        {
            get
            {
                return _yres;
            }
            set
            {
                this._yres = value;
                this.NotifyPropertyChanged("Values_YResolution");
            }
        }

        public SizeDialogBox()
        {
            InitializeComponent();
            this.Values_Width = 800;
            this.Values_Height = 600;
            this.Values_XResolution = 96.0;
            this.Values_YResolution = 96.0;
        }


        private void TextBox_NumberValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_FloatValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled =
                regex.IsMatch(e.Text) ||
                (e.Text == "." && (sender as TextBox).Text.Contains('.')); // just one point is allowed in float numbers
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        
    }
}
