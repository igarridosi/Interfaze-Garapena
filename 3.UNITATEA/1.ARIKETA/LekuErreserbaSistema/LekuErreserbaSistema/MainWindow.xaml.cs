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
        private GarraioPlanoa nireGarraioa;
        private string unekoFitxategia;
        public List<Eserlekua> Eserlekuak { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AutobusaAukeratu_Click(object sender, RoutedEventArgs e)
        {
            // Autobusaren diseinua: 12 ilara, 4 zutabe
            KargatuPlanoa(12, 4, "autobusa.json");
        }

        private void TrenaAukeratu_Click(object sender, RoutedEventArgs e)
        {
            // Trenaren diseinua: 20 ilara, 4 zutabe (2+2)
            KargatuPlanoa(20, 4, "trena.json");
        }

        private void HegazkinaAukeratu_Click(object sender, RoutedEventArgs e)
        {
            // Hegazkinaren diseinua: 25 ilara, 6 zutabe (3+3)
            KargatuPlanoa(25, 6, "hegazkina.json");
        }

        private void AtzeraBotoia_Click(object sender, RoutedEventArgs e)
        {
            // Gorde aldaketak atzera joan aurretik
            GordeUnekoPlanoa();

            // Erakutsi menua eta ezkutatu planoa
            MenuPanela.Visibility = Visibility.Visible;
            PlanoPanela.Visibility = Visibility.Collapsed;

            // Garbitu datuak hurrengorako prest egoteko
            nireGarraioa = null;
            Eserlekuak = null;
            this.DataContext = null;
        }

        private void KargatuPlanoa(int ilarak, int zutabeak, string fitxategiIzena)
        {
            this.unekoFitxategia = fitxategiIzena;
            string bideFidagarria = System.IO.Path.Combine(AppContext.BaseDirectory, fitxategiIzena);

            if (File.Exists(bideFidagarria))
            {
                // Kargatu lehendik dagoen fitxategitik
                string jsonText = File.ReadAllText(bideFidagarria);
                var kargatutakoEserlekuak = JsonConvert.DeserializeObject<List<Eserlekua>>(jsonText);
                nireGarraioa = new GarraioPlanoa(kargatutakoEserlekuak);
            }
            else
            {
                // Sortu plano berri bat hutsetik
                nireGarraioa = new GarraioPlanoa(ilarak, zutabeak);
            }

            // Eguneratu datu-lotura eta interfazea
            Eserlekuak = nireGarraioa.Eserlekuak;
            this.DataContext = this;

            // Erakutsi planoa eta ezkutatu menua
            MenuPanela.Visibility = Visibility.Collapsed;
            PlanoPanela.Visibility = Visibility.Visible;
        }

        private void GordeUnekoPlanoa()
        {
            // Ezer kargatu ez bada, ez dago zer gorde
            if (nireGarraioa == null || string.IsNullOrEmpty(unekoFitxategia))
            {
                return;
            }

            try
            {
                string bideFidagarria = System.IO.Path.Combine(AppContext.BaseDirectory, unekoFitxategia);
                string jsonText = JsonConvert.SerializeObject(nireGarraioa.Eserlekuak, Formatting.Indented);
                File.WriteAllText(bideFidagarria, jsonText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errorea fitxategia gordetzean: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GordeUnekoPlanoa();
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

            nireGarraioa.AldatuHautapenEgoera(hautatutakoEserlekua.Id);

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

        private void BaieztatuKlikEginda(object sender, RoutedEventArgs e)
        {
            // 1. Deitu logikari datuak aldatzeko (Hautatuta -> Okupatuta)
            nireGarraioa.BaieztatuErreserbak();

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