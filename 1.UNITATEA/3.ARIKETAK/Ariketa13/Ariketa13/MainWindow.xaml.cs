using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Ariketa13
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Menú Archivo

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Menú Editar

        private void Cortar_Click(object sender, RoutedEventArgs e)
        {
            mainTextBox.Cut();
        }

        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            mainTextBox.Copy();
        }

        private void Pegar_Click(object sender, RoutedEventArgs e)
        {
            mainTextBox.Paste();
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        { 
            if (mainTextBox.Selection.Text != "")
            {
                mainTextBox.Selection.Text = string.Empty;
            }
        }

        #endregion

        #region Menú Fuente

        private void Fuente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string fontName = menuItem.Header.ToString();

                FontFamily fontFamily = new FontFamily(fontName);

                if (!mainTextBox.Selection.IsEmpty)
                {
                    mainTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
                }
                else
                {
                    mainTextBox.CaretPosition.Parent.SetValue(TextElement.FontFamilyProperty, fontFamily);
                }
            }
        }

        #endregion

    }
}