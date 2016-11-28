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
using System.Windows.Shapes;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
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
            cColorPreview.Background = new SolidColorBrush(this.PickedColor);
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
