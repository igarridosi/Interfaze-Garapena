using System.Diagnostics;
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

namespace Ariketa9
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

        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string newFriend = NewFriendTextBox.Text.Trim();

            Trace.WriteLine(newFriend);
            if (!string.IsNullOrEmpty(newFriend))
            {
                FriendsListBox.Items.Add(newFriend);
                NewFriendTextBox.Clear();
            }
            else
            {
                ShowErrorAdd();
            }
        }

        private void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            if (FriendsListBox.SelectedItem != null)
            {
                FriendsListBox.Items.Remove(FriendsListBox.SelectedItem);
            }
            else
            {
                ShowErrorRemove();
            }
        }

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            FriendsListBox.Items.Clear();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FriendsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FriendsListBox.SelectedItem != null)
            {
                SelectedFriendTextBox.Text = FriendsListBox.SelectedItem.ToString();
            }
        }

        private void ShowErrorAdd()
        {
            MessageBox.Show("Introduzca datos para poder añadirlos", "Error al Añadir", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowErrorRemove()
        {
            MessageBox.Show("Seleccione un elemento para poder eliminarlo", "Error al Eliminar", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}