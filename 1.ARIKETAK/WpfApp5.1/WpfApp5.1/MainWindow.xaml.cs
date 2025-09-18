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

namespace WpfApp5._1
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

        private void ChangeFontComicSans(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontFamily = new FontFamily("Comic Sans MS");
        }

        private void ChangeFontBold(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontWeight = FontWeights.Bold;
        }

        private void ChangeFontStrikethrough(object sender, RoutedEventArgs e)
        {
            TextDisplay.TextDecorations = TextDecorations.Strikethrough;
        }

        private void IncreaseFontSize(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontSize += 2;
        }

        private void DecreaseFontSize(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontSize -= 2;
        }

        private void ChangeFontCourier(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontFamily = new FontFamily("Courier New");
        }

        private void ChangeFontItalic(object sender, RoutedEventArgs e)
        {
            TextDisplay.FontStyle = FontStyles.Italic;
        }

        private void ChangeFontUnderline(object sender, RoutedEventArgs e)
        {
            TextDisplay.TextDecorations = TextDecorations.Underline;
        }

        private void SelectText(object sender, RoutedEventArgs e)
        {
            string selectedText = InputTextBox.SelectedText;
            int textLength = InputTextBox.Text.Length;
            TextInfo.Text = $"El texto tiene {textLength} caracteres, y el texto seleccionado es: {selectedText}";
        }
    }
}