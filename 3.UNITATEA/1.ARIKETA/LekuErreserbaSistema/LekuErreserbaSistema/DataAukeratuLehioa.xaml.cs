using System;
using System.Collections.Generic;
using System.Windows;

namespace LekuErreserbaSistema
{
    public partial class DataAukeratuLehioa : Window
    {
        public DateTime? HautatutakoData { get; private set; }

        public DataAukeratuLehioa()
        {
            InitializeComponent();

            // Kargatu datak eta orduak hasieran
            KargatuHasierakoBalioak();
        }

        private void KargatuHasierakoBalioak()
        {
            // Ezarri gaurko data
            dataPicker.SelectedDate = DateTime.Today;

            // Bete orduen ComboBox-a (00-tik 23-ra)
            for (int i = 0; i < 24; i++)
            {
                orduComboBox.Items.Add(i.ToString("00"));
            }

            // Bete minutuen ComboBox-a (15 minutuko tarteetan)
            minutuComboBox.Items.Add("00");
            minutuComboBox.Items.Add("15");
            minutuComboBox.Items.Add("30");
            minutuComboBox.Items.Add("45");

            // Hautatu uneko ordua eta hurbileneko minutua
            orduComboBox.SelectedItem = DateTime.Now.Hour.ToString("00");
            minutuComboBox.SelectedItem = "00"; // Lehenetsi gisa
        }

        private void Baieztatu_Click(object sender, RoutedEventArgs e)
        {
            // 1. Egiaztatu balioak hautatu direla
            if (dataPicker.SelectedDate == null || orduComboBox.SelectedItem == null || minutuComboBox.SelectedItem == null)
            {
                MessageBox.Show("Mesedez, bete eremu guztiak.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Lortu balioak eta sortu DateTime objektu osoa
            DateTime data = dataPicker.SelectedDate.Value;
            int ordua = int.Parse(orduComboBox.SelectedItem.ToString());
            int minutua = int.Parse(minutuComboBox.SelectedItem.ToString());

            DateTime dataEtaOrduOsoa = new DateTime(data.Year, data.Month, data.Day, ordua, minutua, 0);

            // 3. BALIDAZIOA: Ziurtatu data eta ordua etorkizunekoak direla
            if (dataEtaOrduOsoa < DateTime.Now)
            {
                MessageBox.Show("Ezin duzu iraganeko data edo ordu bat hautatu.", "Data okerra", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 4. Dena ondo badago, gorde emaitza eta itxi leihoa
            HautatutakoData = dataEtaOrduOsoa;
            this.DialogResult = true;
        }
    }
}