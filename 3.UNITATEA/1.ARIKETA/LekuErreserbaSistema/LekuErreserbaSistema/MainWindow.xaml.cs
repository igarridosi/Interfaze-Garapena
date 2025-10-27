using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LekuErreserbaSistema
{
    public partial class MainWindow : Window
    {
        private GarraioPlanoa nireGarraioa;
        private string unekoFitxategia;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AutobusaAukeratu_Click(object sender, RoutedEventArgs e) { KargatuPlanoa(12, 4, "autobusa.json"); }
        private void TrenaAukeratu_Click(object sender, RoutedEventArgs e) { KargatuPlanoa(20, 4, "trena.json"); }
        private void HegazkinaAukeratu_Click(object sender, RoutedEventArgs e) { KargatuPlanoa(25, 6, "hegazkina.json"); }

        private void AtzeraBotoia_Click(object sender, RoutedEventArgs e)
        {
            GordeUnekoPlanoa();
            MenuPanela.Visibility = Visibility.Visible;
            PlanoPanela.Visibility = Visibility.Collapsed;
            GarraioIkuspegia.Content = null;
            nireGarraioa = null;
        }

        private void KargatuPlanoa(int ilarak, int zutabeak, string fitxategiIzena)
        {
            unekoFitxategia = fitxategiIzena;
            string bideFidagarria = Path.Combine(AppContext.BaseDirectory, fitxategiIzena);

            if (File.Exists(bideFidagarria))
            {
                var eserlekuak = JsonConvert.DeserializeObject<List<Eserlekua>>(File.ReadAllText(bideFidagarria));
                nireGarraioa = new GarraioPlanoa(eserlekuak);
            }
            else
            {
                nireGarraioa = new GarraioPlanoa(ilarak, zutabeak);
            }

            UserControl planoIkuspegia = new AutobusPlanoa { Planoa = nireGarraioa };
            GarraioIkuspegia.Content = planoIkuspegia;

            MenuPanela.Visibility = Visibility.Collapsed;
            PlanoPanela.Visibility = Visibility.Visible;
        }

        private void GordeUnekoPlanoa()
        {
            if (nireGarraioa == null) return;
            try
            {
                string bideFidagarria = Path.Combine(AppContext.BaseDirectory, unekoFitxategia);
                File.WriteAllText(bideFidagarria, JsonConvert.SerializeObject(nireGarraioa.Eserlekuak, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errorea gordetzean: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GordeUnekoPlanoa();
        }

        private void BaieztatuKlikEginda(object sender, RoutedEventArgs e)
        {
            nireGarraioa?.BaieztatuErreserbak();
        }
    }
}