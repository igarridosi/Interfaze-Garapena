using System;
using System.Windows;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private int step = 1;
        private double num1, num2, num3, num4;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TxtNumero.Text, out double currentNumber))
            {
                switch (step)
                {
                    case 1:
                        num1 = currentNumber;
                        LblNumero.Content = "Numero 2";
                        break;
                    case 2:
                        num2 = currentNumber;
                        LblNumero.Content = "Numero 3";
                        break;
                    case 3:
                        num3 = currentNumber;
                        LblNumero.Content = "Numero 4";
                        break;
                    case 4:
                        num4 = currentNumber;
                        double result = num1 + (num1 * num2) + (num2 * num3) + (num3 * num4) / 4;
                        SecondWindow secondWindow = new SecondWindow();
                        secondWindow.TxtResultado.Text = result.ToString();
                        secondWindow.Show();
                        this.Close();
                        return;
                }
                step++;
                TxtNumero.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}