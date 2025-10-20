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
            string fitxategia = "eserlekuak.json";

            if (File.Exists(fitxategia))
            {
                string jsonText = File.ReadAllText(fitxategia);

                // Bihurtu JSON testua berriro gure objektuen zerrendara.
                var kargatutakoEserlekuak = JsonConvert.DeserializeObject<List<Eserlekua>>(jsonText);

                // Sortu GarraioPlanoa kargatutako datuekin.
                nireAutobusa = new GarraioPlanoa(kargatutakoEserlekuak);
            }
            else
            {
                nireAutobusa = new GarraioPlanoa(10, 4);
            }

            // Esleitu eserlekuen zerrenda propietate publikoari
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
        private void EguneratuBotoiBatenItxura(Button botoia, Eserlekua Eserlekua)
        {
            switch (Eserlekua.Egoera)
            {
                case EgoeraLekua.Libre:
                    botoia.Background = System.Windows.Media.Brushes.LightGreen;
                    break;
                case EgoeraLekua.Okupatuta:
                    botoia.Background = System.Windows.Media.Brushes.LightCoral;
                    botoia.IsEnabled = false;
                    break;
                case EgoeraLekua.Hautatuta:
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
    }
}