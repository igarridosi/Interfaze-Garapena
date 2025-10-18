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

namespace LekuErreserbaSistema
{
    public partial class MainWindow : Window
    {
        private GarraioPlanoa nireAutobusa;
        public List<Eserlekua> Eserlekuak { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            nireAutobusa = new GarraioPlanoa(10, 4);
            Eserlekuak = nireAutobusa.Eserlekuak;

            // Honek esaten dio XAML-ari non bilatu behar dituen {Binding} bidez eskatutako propietateak
            this.DataContext = this;
        }

        private void EserlekuanKlikEginDa(object sender, RoutedEventArgs e)
        {
            // WPF-n, sakatutako botoiari lotutako Eserlekua zuzenean lor dezakegu bere DataContext-etik.
            Button sakatutakoBotoia = sender as Button;
            if (sakatutakoBotoia == null) return;

            Eserlekua hautatutakoEserlekua = sakatutakoBotoia.DataContext as Eserlekua;
            if (hautatutakoEserlekua == null) return;

            // Deitu gure logikari
            nireAutobusa.AldatuHautapenEgoera(hautatutakoEserlekua.Id);

            // Aldaketa bisualki islatu
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
    }
}