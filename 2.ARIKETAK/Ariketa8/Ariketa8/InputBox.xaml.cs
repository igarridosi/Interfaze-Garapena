using System.Windows;

namespace Ariketa8
{
    public partial class InputBox : Window
    {
        public string ResponseText { get; set; }
        public InputBox(string prompt, string title)
        {
            InitializeComponent();
            lblPrompt.Content = prompt;
            Title = title;
        }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = txtInput.Text;
            DialogResult = true;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}