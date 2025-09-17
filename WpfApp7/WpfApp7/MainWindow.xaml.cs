using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _currentValue = 0;
        private string _currentOperator = string.Empty;
        private bool _isOperatorPressed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Zenbaki botoiaren kudeaketa
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            if (_isOperatorPressed || ResultBox.Text == "0")
            {
                ResultBox.Text = button.Content.ToString();
                _isOperatorPressed = false;
            }
            else
            {
                ResultBox.Text += button.Content.ToString();
            }
        }

        // Operagailu botoiaren kudeaketa
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            _currentValue = double.Parse(ResultBox.Text);
            _currentOperator = button.Content.ToString();
            _isOperatorPressed = true;
        }

        // Berdinketa botoiaren kudeaketa
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            double newValue = double.Parse(ResultBox.Text);

            switch (_currentOperator)
            {
                case "+":
                    _currentValue += newValue;
                    break;
                case "-":
                    _currentValue -= newValue;
                    break;
                case "*":
                    _currentValue *= newValue;
                    break;
                case "/":
                    _currentValue /= newValue;
                    break;
                case "%":
                    _currentValue %= newValue;
                    break;
            }

            ResultBox.Text = _currentValue.ToString();
            _isOperatorPressed = true;
        }

        // Garbitze botoiaren kudeaketa
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _currentValue = 0;
            _currentOperator = string.Empty;
            ResultBox.Text = "0";
        }

        // Sarrera garbitzeko botoiaren kudeaketa
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            ResultBox.Text = "0";
        }
    }
}