using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; // Necesario si usas PreviewTextInput

namespace ARIKETA12
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        public void dietasClick(object sender, RoutedEventArgs e)
        {
            RecalcularTotal();
        }


        public void textBoxChanged(object sender, TextChangedEventArgs e)
        {
            RecalcularTotal();
        }

        private double ObtenerNumeroDeTextBox(TextBox textBox)
        {
            string cleanedText = textBox.Text;

            double value = 0.0;
            bool esValido = false;

            if (!string.IsNullOrWhiteSpace(cleanedText))
            {
                esValido = double.TryParse(
                    cleanedText.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );
            }
            else
            {
                esValido = true;
                value = 0.0;
            }


            if (!esValido)
            {
                MessageBox.Show(
                    $"Eremuak zenbakizko balioak bakarrik onartzen ditu.",
                    "Sarrera-errorea",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                textBox.Text = ""; 
                textBox.Focus();
            }

            return Math.Max(0, value); // 0 itzultzen du negatiboak edo hutsak badira
        }

        private void RecalcularTotal()
        {
            const double COSTE_DESAYUNO = 3.0;
            const double COSTE_COMIDA = 9.0;
            const double COSTE_CENA = 15.5;
            const double COSTE_KM = 0.25;
            const double COSTE_HORA_VIAJE = 18.0;
            const double COSTE_HORA_TRABAJO = 42.0;


            // DIETAS
            double subtotalDietas = 0.0;
            if (chkDesayuno.IsChecked == true) subtotalDietas += COSTE_DESAYUNO;
            if (chkComida.IsChecked == true) subtotalDietas += COSTE_COMIDA;
            if (chkCena.IsChecked == true) subtotalDietas += COSTE_CENA;

            txtResultado1.Text = subtotalDietas.ToString("F2");

            // VIAJES
            double subtotalViajes = 0.0;
            double km = ObtenerNumeroDeTextBox(txtKM);
            double horasViaje = ObtenerNumeroDeTextBox(txtHorasViaje);

            subtotalViajes += km * COSTE_KM;
            subtotalViajes += horasViaje * COSTE_HORA_VIAJE;

            txtResultado2.Text = subtotalViajes.ToString("F2");


            // TRABAJO
            double subtotalTrabajo = 0.0;
            double horasTrabajo = ObtenerNumeroDeTextBox(txtHorasTrabajo);

            subtotalTrabajo += horasTrabajo * COSTE_HORA_TRABAJO;

            txtResultado3.Text = subtotalTrabajo.ToString("F2");


            // TOTAL
            double total = subtotalDietas + subtotalViajes + subtotalTrabajo;
            txtTotal.Text = total.ToString("F2");
        }

        public void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void limpiarClick(object sender, RoutedEventArgs e)
        {
            chkDesayuno.IsChecked = false;
            chkComida.IsChecked = false;
            chkCena.IsChecked = false;
            txtKM.Text = "";
            txtHorasViaje.Text = "";
            txtHorasTrabajo.Text = "";
            txtResultado1.Text = "0,00";
            txtResultado2.Text = "0,00";
            txtResultado3.Text = "0,00";
            txtTotal.Text = "0,00";
        }
    }
}