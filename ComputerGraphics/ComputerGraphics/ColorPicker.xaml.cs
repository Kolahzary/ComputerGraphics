using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }
        public Brush PickedColor_Brush => new SolidColorBrush(this.PickedColor);
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.PickedColor = Colors.Black;
        }

        private void Sliders_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.NotifyPropertyChanged_Colors();
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

        private void cbKnownColors_Selected(object sender, RoutedEventArgs e)
        {
            this.PickedColor = ((Color)ColorConverter.ConvertFromString((string)cbKnownColors.SelectedValue));
        }
    }
}
