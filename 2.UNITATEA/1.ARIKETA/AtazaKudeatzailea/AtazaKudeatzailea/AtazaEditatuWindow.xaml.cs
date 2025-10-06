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

namespace AtazaKudeatzailea
{
    public partial class AtazaEditatuWindow : Window
    {
        public AtazaEditatuWindow()
        {
            InitializeComponent();
        }

        private void Gorde_Click(object sender, RoutedEventArgs e)
        {
            // Balidazioa hemen gehi liteke etorkizunean
            this.DialogResult = true;
        }

        private void Utzi_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
