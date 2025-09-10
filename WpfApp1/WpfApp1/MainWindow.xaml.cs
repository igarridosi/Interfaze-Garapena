using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOperar(object sender, RoutedEventArgs e)
        {
            try
            {
                double numero1 = double.Parse(txtNumero1.Text, CultureInfo.InvariantCulture);
                double numero2 = double.Parse(txtNumero2.Text, CultureInfo.InvariantCulture);
                double numero3 = double.Parse(txtNumero3.Text, CultureInfo.InvariantCulture);
                double numero4 = double.Parse(txtNumero4.Text, CultureInfo.InvariantCulture);

                double resultado = (numero1 + 2 * numero2 + 3 * numero3 + 4 * numero4) / 4;

                txtResultado.Text = resultado.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor, introduzca valores numéricos válidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLimpiar(object sender, RoutedEventArgs e)
        {
            txtNumero1.Clear();
            txtNumero2.Clear();
            txtNumero3.Clear();
            txtNumero4.Clear();
            txtResultado.Clear();

            txtNumero1.Focus();
        }

        private void btnSalir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return double.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _);
        }

        private void txtNumero1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}