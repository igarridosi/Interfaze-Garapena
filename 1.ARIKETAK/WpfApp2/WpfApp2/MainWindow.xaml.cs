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

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private string[] frases = new string[5];
        private int fraseActual = 0;

        public MainWindow()
        {
            InitializeComponent();
            InicializarEstadoInicial();

            // Izendu ebentu botoiak
            buttonFrase1.Click += ButtonFrase_Click;
            buttonFrase2.Click += ButtonFrase_Click;
            buttonFrase3.Click += ButtonFrase_Click;
            buttonFrase4.Click += ButtonFrase_Click;
            buttonFrase5.Click += ButtonFrase_Click;
            buttonUnir.Click += ButtonUnir_Click;
            buttonLimpiar.Click += ButtonLimpiar_Click;
            buttonSalir.Click += ButtonSalir_Click;
        }

        private void InicializarEstadoInicial()
        {
            // Desgaitu butoiak hasierako egoeran
            buttonFrase1.IsEnabled = true;
            buttonFrase2.IsEnabled = false;
            buttonFrase3.IsEnabled = false;
            buttonFrase4.IsEnabled = false;
            buttonFrase5.IsEnabled = false;
            buttonUnir.IsEnabled = false;

            // Garbitu testu-koadroa eta aldatu aldagaien balioak
            textBoxFrase.Clear();
            frases = new string[5];
            fraseActual = 0;

            // Fokua testu-koadroan jarri
            textBoxFrase.Focus();
        }

        private void ButtonFrase_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFrase.Text))
            {
                MessageBox.Show("Por favor, introduce una frase.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBoxFrase.Focus();
                return;
            }

            Button botonPulsado = sender as Button;
            if (botonPulsado == null) return;

            // Gordea esaldia
            frases[fraseActual] = textBoxFrase.Text;

            // Desgaitu sakatu den botoia
            botonPulsado.IsEnabled = false;

            fraseActual++;

            // Aktibatu hurrengo botoia
            switch (fraseActual)
            {
                case 1:
                    buttonFrase2.IsEnabled = true;
                    break;
                case 2:
                    buttonFrase3.IsEnabled = true;
                    break;
                case 3:
                    buttonFrase4.IsEnabled = true;
                    break;
                case 4:
                    buttonFrase5.IsEnabled = true;
                    break;
                case 5:
                    buttonUnir.IsEnabled = true;
                    break;
            }

            textBoxFrase.Clear();
            textBoxFrase.Focus();
        }

        private void ButtonUnir_Click(object sender, RoutedEventArgs e)
        {
            string fraseCompleta = string.Join(" ", frases);
            textBoxFrase.Text = fraseCompleta;
            buttonUnir.IsEnabled = false;
        }

        private void ButtonLimpiar_Click(object sender, RoutedEventArgs e)
        {
            InicializarEstadoInicial();
        }

        private void ButtonSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}