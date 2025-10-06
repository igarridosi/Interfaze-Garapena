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

namespace Ariketa11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] savedData;

        string data1;
        string data2;
        string data3;
        string data4;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void accept(object sender, RoutedEventArgs e)
        {
            var inputs = new[] { nombre.Text, apellido1.Text, apellido2.Text, dni.Text };

            if (inputs.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Por favor, rellena todos los campos.");
                return;
            }

            data1 = inputs[0];
            data2 = inputs[1];
            data3 = inputs[2];
            data4 = inputs[3];

            savedData = inputs;
        }
        private void visualize(object sender, RoutedEventArgs e)
        {
            if (savedData == null)
            {
                MessageBox.Show("No hay datos para mostrar. Por favor, rellena y acepta los datos primero.");
                return;
            }
            else
            {
                WelcomeWindow welcomeScreen = new WelcomeWindow(savedData);
                welcomeScreen.Show();
                this.Close();
            }
            
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}