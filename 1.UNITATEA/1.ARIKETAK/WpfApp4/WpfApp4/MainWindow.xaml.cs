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

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string UsuarioCorrecto = "Informatica";
        private const string ContraseñaCorrecta = "1234";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtPassword.Password;

            if (usuario == UsuarioCorrecto && contraseña == ContraseñaCorrecta)
            {
                lblMensaje.Content = $"Bienvenido al Sistema, {usuario}";
            }
            else
            {
                lblMensaje.Content = "Usuario no identificado";
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            lblMensaje.Content = string.Empty;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}