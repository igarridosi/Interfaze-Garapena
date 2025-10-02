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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ariketa11
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        public WelcomeWindow(string[] userData)
        {
            InitializeComponent();

            ShowData(userData);
        }

        public void ShowData(string[] userData)
        {
            if (userData == null || userData.Length < 4)
            {
                nombreWW.Text = "ERROR: Faltan datos";
                return;
            }

            nombreWW.Text = "Nombre: " + userData[0];
            apellido1WW.Text = "1º Apellido: " + userData[1];
            apellido2WW.Text = "2º Apellido: " + userData[2];
            dniWW.Text = "DNI: " + userData[3];
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
