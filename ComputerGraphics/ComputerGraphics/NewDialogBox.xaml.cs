using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewDialogBox.xaml
    /// </summary>
    public partial class NewDialogBox : Window
    {
        private ushort _width, _height;
        private float _resolution;
        public ushort Values_Width { get { return _width; } }
        public ushort Values_Height { get { return _height; } }
        public float Values_Resolution { get { return _resolution; } }
        public NewDialogBox()
        {
            InitializeComponent();
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
            if (!ushort.TryParse(txtWidth.Text, out _width))
            {
                MessageBox.Show("Width must be a positive integer less than 65535", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if (!ushort.TryParse(txtHeight.Text, out _height))
            {
                MessageBox.Show("Height must be a positive integer less than 65535", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if (!float.TryParse(txtResolution.Text, out _resolution))
            {
                MessageBox.Show("Resolution must be a float number", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if (_resolution < 0)
            {
                MessageBox.Show("Resolution should be positive", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
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
