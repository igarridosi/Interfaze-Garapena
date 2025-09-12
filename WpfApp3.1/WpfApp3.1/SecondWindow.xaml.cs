using System;
using System.Windows;

namespace WpfApp3
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            TxtResultado.Text = string.Empty;
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}