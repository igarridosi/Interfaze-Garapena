using Newtonsoft.Json;
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
using Newtonsoft.Json;
using System.IO;

namespace LekuErreserbaSistema
{
    public partial class MainWindow : Window
    {
        private GarraioPlanoa nireAutobusa;
        public List<Eserlekua> Eserlekuak { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            KargatuEdoSortuPlanoa();

            this.DataContext = this;

            // Hasierako koloreak ondo ezarri
            this.Loaded += MainWindow_Loaded;
        }

        private void KargatuEdoSortuPlanoa()
        {
            // 1. Lortu programaren .exe fitxategia dagoen karpetaren bide osoa
            string programarenKarpeta = AppContext.BaseDirectory;

            // 2. Sortu fitxategiaren bide osoa eta fidagarria
            string bideFidagarria = System.IO.Path.Combine(programarenKarpeta, "eserlekuak.json");

            // 3. Erabili beti bide fidagarri hori
            if (File.Exists(bideFidagarria))
            {
                try
                {
                    string jsonText = File.ReadAllText(bideFidagarria);
                    var kargatutakoEserlekuak = JsonConvert.DeserializeObject<List<Eserlekua>>(jsonText);
                    nireAutobusa = new GarraioPlanoa(kargatutakoEserlekuak);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errorea fitxategia kargatzean: " + ex.Message);
                    nireAutobusa = new GarraioPlanoa(10, 4);
                }
            }
            else
            {
                nireAutobusa = new GarraioPlanoa(10, 4);
            }

            Eserlekuak = nireAutobusa.Eserlekuak;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Eguneratu botoi guztien hasierako itxura
            foreach (var eserlekua in Eserlekuak)
            {
                // Bilatu eserleku bakoitzari dagokion botoia
                Button botoia = EserlekuenPlanoa.ItemContainerGenerator.ContainerFromItem(eserlekua) as Button;

                if (botoia != null)
                {
                    EguneratuBotoiBatenItxura(botoia, eserlekua);
                }
            }
        }

        private void EserlekuanKlikEginDa(object sender, RoutedEventArgs e)
        {
            // WPF-n, sakatutako botoiari lotutako Eserlekua zuzenean lor dezakegu bere DataContext-etik.
            Button sakatutakoBotoia = sender as Button;
            if (sakatutakoBotoia == null) return;

            Eserlekua hautatutakoEserlekua = sakatutakoBotoia.DataContext as Eserlekua;
            if (hautatutakoEserlekua == null) return;

            nireAutobusa.AldatuHautapenEgoera(hautatutakoEserlekua.Id);

            EguneratuBotoiBatenItxura(sakatutakoBotoia, hautatutakoEserlekua);
        }

        // Koloreak aldatzeko funtzioa
        private void EguneratuBotoiBatenItxura(Button botoia, Eserlekua eserlekua)
        {
            // Ziurtatu hemen zure enum-aren izen zuzena erabiltzen duzula
            switch (eserlekua.Egoera)
            {
                case EgoeraEserlekua.Libre:
                    botoia.Background = System.Windows.Media.Brushes.White;
                    break;
                case EgoeraEserlekua.Okupatuta:
                    botoia.Background = System.Windows.Media.Brushes.LightCoral;
                    botoia.IsEnabled = false;
                    break;
                case EgoeraEserlekua.Hautatuta:
                    botoia.Background = System.Windows.Media.Brushes.LightBlue;
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Bihurtu gure eserlekuen zerrenda JSON formatuko testu batera.
            // Formatting.Indented jartzen dugu fitxategia irakurgarriagoa izan dadin.
            string jsonText = JsonConvert.SerializeObject(nireAutobusa.Eserlekuak, Formatting.Indented);

            File.WriteAllText("eserlekuak.json", jsonText);
        }

        private void BaieztatuKlikEginda(object sender, RoutedEventArgs e)
        {
            // 1. Deitu logikari datuak aldatzeko (Hautatuta -> Okupatuta)
            nireAutobusa.BaieztatuErreserbak();

            // 2. Orain, interfazea eguneratu behar dugu aldaketak islatzeko.
            //    Botoi guztiak arakatuko ditugu eta haien kolorea eguneratuko dugu.
            foreach (var eserlekua in Eserlekuak)
            {
                // Bilatu eserleku bakoitzari dagokion botoia
                var container = EserlekuenPlanoa.ItemContainerGenerator.ContainerFromItem(eserlekua);

                // ContainerFromItem-ek ez du zuzenean Button itzultzen, bilatu behar da
                if (container is FrameworkElement fe)
                {
                    var botoia = FindVisualChild<Button>(fe);
                    if (botoia != null)
                    {
                        // Deitu lehendik daukagun metodoari botoiaren kolorea eguneratzeko
                        EguneratuBotoiBatenItxura(botoia, eserlekua);
                    }
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}