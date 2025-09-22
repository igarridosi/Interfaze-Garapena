using System;
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

namespace Ariketa8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var orainTextBox = OrainTextBox;
            var gaurTextBox = GaurTextBox;
            var gaurkoOrduaTextBox = GaurkoOrduaTextBox;
            var sumaFechasTextBox = SumaFechasTextBox;
            var diferenciaFechasTextBox = DiferenciaFechasTextBox;

            // Orain: Oraingo orduarekin eta datarekin
            orainTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            // Gaur: Oraingo data
            gaurTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy");

            // Gaurko ordua: Oraingo ordua
            gaurkoOrduaTextBox.Text = DateTime.Now.ToString("HH:mm:ss");

            // Daten Gehiketa
            var inputFecha = new InputBox("Ingrese una fecha de la forma dd/MM/yyyy:", "Función SumaFechas");
            if (inputFecha.ShowDialog() == true)
            {
                if (DateTime.TryParseExact(inputFecha.ResponseText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                {
                    var inputMeses = new InputBox("Ingrese el número de meses que se agrega a la fecha:", "Meses");
                    if (inputMeses.ShowDialog() == true && int.TryParse(inputMeses.ResponseText, out int meses))
                    {
                        var nuevaFecha = fecha.AddMonths(meses);
                        sumaFechasTextBox.Text = $"Fecha inicio: {fecha:dd/MM/yyyy}, Meses a sumar: {meses}. Nueva Fecha: {nuevaFecha:dd/MM/yyyy}";
                    }
                    else
                    {
                        ShowErrorAndReset();
                        return;
                    }
                }
                else
                {
                    ShowErrorAndReset();
                    return;
                }
            }
            else
            {
                sumaFechasTextBox.Text = "";
            }

            // Daten arte desberdintasuna (egunetan)
            var inputFechaInicial = new InputBox("Ingrese fecha inicial de la forma dd/MM/yyyy:", "Función DiferenciaFechas");
            if (inputFechaInicial.ShowDialog() == true)
            {
                if (DateTime.TryParseExact(inputFechaInicial.ResponseText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaInicial))
                {
                    var inputFechaFinal = new InputBox("Ingrese fecha final de la forma dd/MM/yyyy:", "Función DiferenciaFechas");
                    if (inputFechaFinal.ShowDialog() == true && DateTime.TryParseExact(inputFechaFinal.ResponseText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaFinal))
                    {
                        var diferencia = (fechaFinal - fechaInicial).Days;
                        diferenciaFechasTextBox.Text = $"Desde {fechaInicial:dd/MM/yyyy} hasta {fechaFinal:dd/MM/yyyy} hay {diferencia} dias";
                    }
                    else
                    {
                        ShowErrorAndReset();
                        return;
                    }
                }
                else
                {
                    ShowErrorAndReset();
                    return;
                }
            }
            else
            {
                diferenciaFechasTextBox.Text = "";
            }
        }

        private void ShowErrorAndReset()
        {
            MessageBox.Show("Introduzca los datos correctamente edo irten sakatu", "Data okerra", MessageBoxButton.OK, MessageBoxImage.Error);
            OrainTextBox.Text = "";
            GaurTextBox.Text = "";
            GaurkoOrduaTextBox.Text = "";
            SumaFechasTextBox.Text = "";
            DiferenciaFechasTextBox.Text = "";
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            OrainTextBox.Text = "";
            GaurTextBox.Text = "";
            GaurkoOrduaTextBox.Text = "";
            SumaFechasTextBox.Text = "";
            DiferenciaFechasTextBox.Text = "";
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}