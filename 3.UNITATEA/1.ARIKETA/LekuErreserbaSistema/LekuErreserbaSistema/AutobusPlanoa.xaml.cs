using System.Windows;
using System.Windows.Controls;

namespace LekuErreserbaSistema
{
    public partial class AutobusPlanoa : UserControl
    {
        public static readonly DependencyProperty PlanoaProperty =
            DependencyProperty.Register("Planoa", typeof(GarraioPlanoa), typeof(AutobusPlanoa), new PropertyMetadata(null, OnPlanoaChanged));

        public GarraioPlanoa Planoa
        {
            get { return (GarraioPlanoa)GetValue(PlanoaProperty); }
            set { SetValue(PlanoaProperty, value); }
        }

        private static void OnPlanoaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var kontrola = d as AutobusPlanoa;
            kontrola.DataContext = kontrola.Planoa;
        }

        public AutobusPlanoa()
        {
            InitializeComponent();
        }

        private void EserlekuanKlikEginDa(object sender, RoutedEventArgs e)
        {
            Button sakatutakoBotoia = sender as Button;
            Eserlekua hautatutakoEserlekua = sakatutakoBotoia?.DataContext as Eserlekua;
            if (hautatutakoEserlekua == null) return;

            if (hautatutakoEserlekua.Egoera == EgoeraEserlekua.Libre)
            {
                var dataLehioa = new DataAukeratuLehioa();
                if (dataLehioa.ShowDialog() == true)
                {
                    hautatutakoEserlekua.ErreserbaData = dataLehioa.HautatutakoData;
                    hautatutakoEserlekua.Egoera = EgoeraEserlekua.Hautatuta;
                }
            }
            else if (hautatutakoEserlekua.Egoera == EgoeraEserlekua.Hautatuta)
            {
                hautatutakoEserlekua.ErreserbaData = null;
                hautatutakoEserlekua.Egoera = EgoeraEserlekua.Libre;
            }
            else if (hautatutakoEserlekua.Egoera == EgoeraEserlekua.Okupatuta)
            {
                MessageBoxResult emaitza = MessageBox.Show(
                    $"Ziur zaude '{hautatutakoEserlekua.Id}' eserlekuaren erreserba bertan behera utzi nahi duzula?",
                    "Erreserba Utzi", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (emaitza == MessageBoxResult.Yes)
                {
                    hautatutakoEserlekua.ErreserbaData = null;
                    hautatutakoEserlekua.Egoera = EgoeraEserlekua.Libre;
                }
            }
        }
    }
}