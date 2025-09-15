using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents; 
using System.Windows.Media;      

namespace WpfApp5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Botoien eta RichTextBox-en gertaerak harpidetu
            // Jatorrizko XAML-n definituta ez daudenez, hemen gehitzen ditugu.
            btnComicSans.Click += btnComicSans_Click;
            btnNegrita.Click += btnNegrita_Click;
            btnTachado.Click += btnTachado_Click;
            btnAumentarTamano.Click += btnAumentarTamano_Click;

            btnCourier.Click += btnCourier_Click;
            btnCursiva.Click += btnCursiva_Click;
            btnSubrayado.Click += btnSubrayado_Click;
            btnReducirTamano.Click += btnReducirTamano_Click;

            btnSeleccionar.Click += btnSeleccionar_Click;
            btnSalir.Click += btnSalir_Click;

            rtbTextoPrincipal.TextChanged += rtbTextoPrincipal_TextChanged;
            rtbTextoPrincipal.SelectionChanged += rtbTextoPrincipal_SelectionChanged;

            this.Loaded += MainWindow_Loaded;

            // RichTextBox-aren hasierako edukia ezarri
            SetInitialRichTextBoxContent("Este será el texto seleccionado");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // "texto" hitzaren hautaketa simulatu leihoa kargatzean,
            string textToSelect = "texto";
            TextRange tr = new TextRange(rtbTextoPrincipal.Document.ContentStart, rtbTextoPrincipal.Document.ContentEnd);
            string fullText = tr.Text;
            int startIndex = fullText.IndexOf(textToSelect);

            if (startIndex != -1)
            {
                TextPointer startPointer = GetTextPointerAtOffset(rtbTextoPrincipal.Document.ContentStart, startIndex);
                TextPointer endPointer = GetTextPointerAtOffset(startPointer, textToSelect.Length);
                rtbTextoPrincipal.Selection.Select(startPointer, endPointer);
            }
            UpdateStatusText(); // Ziurtatu egoera eguneratzen dela kargatzean
        }

        // RichTextBox-aren hasierako edukia ezartzeko laguntza-metodoa
        private void SetInitialRichTextBoxContent(string content)
        {
            rtbTextoPrincipal.Document.Blocks.Clear();
            rtbTextoPrincipal.Document.Blocks.Add(new Paragraph(new Run(content)));
        }

        // TextPointer bat offset zehatz batean lortzeko laguntza-metodoa
        private TextPointer GetTextPointerAtOffset(TextPointer start, int offset)
        {
            TextPointer current = start;
            int currentOffset = 0;
            while (current != null && currentOffset < offset)
            {
                if (current.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    int runLength = current.GetTextRunLength(LogicalDirection.Forward);
                    if (currentOffset + runLength > offset)
                    {
                        return current.GetPositionAtOffset(offset - currentOffset);
                    }
                    currentOffset += runLength;
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
            return current;
        }

        // RichTextBox-en testua aldatzen denean gertaera
        private void rtbTextoPrincipal_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStatusText();
        }

        // RichTextBox-en hautapena aldatzen denean gertaera
        private void rtbTextoPrincipal_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateStatusText();
        }

        // Egoera-TextBlock (tblkInfo) eguneratzeko metodoa
        private void UpdateStatusText()
        {
            TextRange textRange = new TextRange(rtbTextoPrincipal.Document.ContentStart, rtbTextoPrincipal.Document.ContentEnd);
            // .Trim() FlowDocument-ek gehitu ditzakeen lerro-jauzi gehigarriak kentzeko
            string fullText = textRange.Text.Trim();
            string selectedText = rtbTextoPrincipal.Selection.Text;

            tblkInfo.Text = $"Testuak {fullText.Length} karaktere ditu, eta hautatutako testua hauxe da: {selectedText}";
        }

        //  --- Estilo eta tamainaren botoietarako gertaeren kudeatzaileak ---

        private void btnComicSans_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily("Comic Sans MS"));
        }

        private void btnCourier_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily("Courier New"));
        }

        private void btnNegrita_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            object currentWeight = rtbTextoPrincipal.Selection.GetPropertyValue(TextElement.FontWeightProperty);

            if (currentWeight is FontWeight && (FontWeight)currentWeight == FontWeights.Bold)
            {
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }

        private void btnCursiva_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            object currentStyle = rtbTextoPrincipal.Selection.GetPropertyValue(TextElement.FontStyleProperty);

            if (currentStyle is FontStyle && (FontStyle)currentStyle == FontStyles.Italic)
            {
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
            else
            {
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
        }

        private void btnSubrayado_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            TextDecorationCollection currentDecorations = (TextDecorationCollection)rtbTextoPrincipal.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

            if (currentDecorations != null && currentDecorations.Contains(TextDecorations.Underline[0]))
            {
                TextDecorationCollection newDecorations = currentDecorations.Where(td => td != TextDecorations.Underline[0]).ToTextDecorationCollection();
                rtbTextoPrincipal.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
            }
            else
            {
                TextDecorationCollection newDecorations = currentDecorations != null ? currentDecorations.Clone() : new TextDecorationCollection();
                newDecorations.Add(TextDecorations.Underline);
                rtbTextoPrincipal.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
            }
        }

        private void btnTachado_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            TextDecorationCollection currentDecorations = (TextDecorationCollection)rtbTextoPrincipal.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

            
            if (currentDecorations != null && currentDecorations.Contains(TextDecorations.Strikethrough[0]))
            {
                TextDecorationCollection newDecorations = currentDecorations.Where(td => td != TextDecorations.Strikethrough[0]).ToTextDecorationCollection();
                rtbTextoPrincipal.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
            }
            else
            {
                TextDecorationCollection newDecorations = currentDecorations != null ? currentDecorations.Clone() : new TextDecorationCollection();
                newDecorations.Add(TextDecorations.Strikethrough);
                rtbTextoPrincipal.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
            }
        }

        private void btnAumentarTamano_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            object currentSize = rtbTextoPrincipal.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            if (currentSize is double)
            {
                double newSize = (double)currentSize + 2; // 2 puntu handitu
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newSize);
            }
            else
            {
                // Tamaina esplizituki ezarrita ez badago, lehenetsitako bat erabili hasteko
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, 16.0);
            }
        }

        private void btnReducirTamano_Click(object sender, RoutedEventArgs e)
        {
            if (rtbTextoPrincipal.Selection.IsEmpty) return;
            object currentSize = rtbTextoPrincipal.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            if (currentSize is double)
            {
                double newSize = (double)currentSize - 2; // 2 puntu murriztu
                if (newSize > 4) // Ziurtatu tamaina ez dela txikiegia
                {
                    rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newSize);
                }
            }
            else
            {
                // Tamaina esplizituki ezarrita ez badago, lehenetsitako bat erabili hasteko
                rtbTextoPrincipal.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, 10.0);
            }
        }

        // --- Ekintza-botoietarako kudeatzaileak ---

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Hautatutako testua: '{rtbTextoPrincipal.Selection.Text}'", "Hautapena", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // TextDecorationCollection manipulazioa errazteko luzapen klasea
    public static class TextDecorationCollectionExtensions
    {
        public static TextDecorationCollection ToTextDecorationCollection(this IEnumerable<TextDecoration> decorations)
        {
            TextDecorationCollection collection = new TextDecorationCollection();
            foreach (var decoration in decorations)
            {
                collection.Add(decoration);
            }
            return collection;
        }
    }
}