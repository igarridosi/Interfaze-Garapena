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

namespace Ariketa10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboImagenes.SelectedIndex = -1; // Ez erakutri irudiak hasieran
        }

        // ComboBox
        private void comboImagenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            img1.Visibility = Visibility.Collapsed;
            img2.Visibility = Visibility.Collapsed;
            img3.Visibility = Visibility.Collapsed;

            switch (comboImagenes.SelectedIndex)
            {
                case 0:
                    img1.Visibility = Visibility.Visible;
                    break;
                case 1:
                    img2.Visibility = Visibility.Visible;
                    break;
                case 2:
                    img3.Visibility = Visibility.Visible;
                    break;
            }
        }

        // CheckBox
        private void chkImg_Checked(object sender, RoutedEventArgs e)
        {
            img4.Visibility = chkImg4.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            img5.Visibility = chkImg5.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            img6.Visibility = chkImg6.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}