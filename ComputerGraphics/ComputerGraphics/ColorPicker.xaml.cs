using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Linq;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        public void NotifyPropertyChanged_Colors()
        {
            this.NotifyPropertyChanged("PickedColor");
            this.NotifyPropertyChanged("PickedColor_Brush");
            this.NotifyPropertyChanged("PickedColorName"); 
        }
        public Brush PickedColor_Brush => new SolidColorBrush(this.PickedColor);
        public string PickedColorName
        {
            get
            {
                return typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(a => (Color)a.GetValue(typeof(Colors)) == this.PickedColor)?.Name;
            }
            set
            {
                this.PickedColor = (Color)ColorConverter.ConvertFromString(value);
                this.NotifyPropertyChanged_Colors();
            }
        }
        public Color PickedColor
        {
            get
            {
                return Color.FromArgb((byte)sAlpha.Value, (byte)sRed.Value, (byte)sGreen.Value, (byte)sBlue.Value);
            }
            set
            {
                sAlpha.Value = value.A;
                sRed.Value = value.R;
                sGreen.Value = value.G;
                sBlue.Value = value.B;
                this.NotifyPropertyChanged_Colors();
            }
        }
        public ColorPicker()
        {
            InitializeComponent();
            this.PickedColor = Colors.Black;
        }

        private void Sliders_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            => this.NotifyPropertyChanged_Colors();

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
