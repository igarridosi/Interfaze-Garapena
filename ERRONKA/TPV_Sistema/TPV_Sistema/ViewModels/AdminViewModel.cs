using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPV_Sistema.Models;

namespace TPV_Sistema.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        // ERABILTZAILEEN ZERRENDA
        // ObservableCollection erabiltzen dugu, zerrenda aldatzean interfazea automatikoki freskatzeko.
        public ObservableCollection<Erabiltzailea> Erabiltzaileak { get; set; }
        public ObservableCollection<Produktua> Produktuak { get; set; }
        public ObservableCollection<Mahaia> Mahaiak { get; set; }
        public ObservableCollection<Eskaera> Eskaerak { get; set; }

        // HAUTATUTAKO ERABILTZAILEA
        // Interfazeko zerrendan hautatzen dugun erabiltzailea gordeko du.
        private Erabiltzailea _hautatutakoErabiltzailea;
        public Erabiltzailea HautatutakoErabiltzailea
        {
            get { return _hautatutakoErabiltzailea; }
            set
            {
                _hautatutakoErabiltzailea = value;
                // Erabiltzaile bat hautatzean, bere datuak textu-koadroetan erakutsi.
                if (value != null)
                {
                    ErabiltzaileIzena = value.ErabiltzaileIzena;
                    Pasahitza = value.Pasahitza;
                    Rola = value.Rola;
                }
                OnPropertyChanged(); // Interfazeari jakinarazi
            }
        }

        // TEXTU-KOADROEKIN LOTUTAKO PROPIETATEAK
        private string _erabiltzaileIzena;
        public string ErabiltzaileIzena { get => _erabiltzaileIzena; set { _erabiltzaileIzena = value; OnPropertyChanged(); } }

        private string _pasahitza;
        public string Pasahitza { get => _pasahitza; set { _pasahitza = value; OnPropertyChanged(); } }

        private string _rola;
        public string Rola { get => _rola; set { _rola = value; OnPropertyChanged(); } }

        // BOTOIEKIN LOTUTAKO KOMANDOAK
        public RelayCommand GehituAgindua { get; private set; }
        public RelayCommand AldatuAgindua { get; private set; }
        public RelayCommand EzabatuAgindua { get; private set; }
        public RelayCommand GarbituAgindua { get; private set; }

        // PRODUKTUAK

        private Produktua _hautatutakoProduktua;
        public Produktua HautatutakoProduktua
        {
            get { return _hautatutakoProduktua; }
            set
            {
                _hautatutakoProduktua = value;
                if (value != null)
                {
                    ProduktuIzena = value.Izena;
                    PrezioaString = value.Prezioa.ToString();
                    StockaString = value.Stocka.ToString();
                }
                OnPropertyChanged();
            }
        }

        private string _produktuIzena;
        public string ProduktuIzena { get => _produktuIzena; set { _produktuIzena = value; OnPropertyChanged(); } }

        private string _prezioaString;
        public string PrezioaString { get => _prezioaString; set { _prezioaString = value; OnPropertyChanged(); } }

        private string _stockaString;
        public string StockaString { get => _stockaString; set { _stockaString = value; OnPropertyChanged(); } }

        public RelayCommand GehituProduktuAgindua { get; private set; }
        public RelayCommand AldatuProduktuAgindua { get; private set; }
        public RelayCommand EzabatuProduktuAgindua { get; private set; }
        public RelayCommand GarbituProduktuAgindua { get; private set; }

        // MAHAIAK

        private Mahaia _hautatutakoMahaia;
        public Mahaia HautatutakoMahaia
        {
            get => _hautatutakoMahaia;
            set
            {
                _hautatutakoMahaia = value;
                if (value != null)
                {
                    MahaiIzena = value.MahaiIzena;
                    EdukieraString = value.Edukiera.ToString();
                }
                OnPropertyChanged();
            }
        }

        // ESKAERAK (SALMENTAK)

        private Eskaera _hautatutakoEskaera;
        public Eskaera HautatutakoEskaera
        {
            get => _hautatutakoEskaera;
            set
            {
                _hautatutakoEskaera = value;
                OnPropertyChanged();
                // Hautatutako eskaeraren lerroak kargatu
                if (value != null)
                {
                    _ = KargatuEskaeraLerroak();
                }
                else
                {
                    HautatutakoEskaerarenLerroak.Clear();
                }
            }
        }
        public ObservableCollection<EskaeraLerroa> HautatutakoEskaerarenLerroak { get; set; }


        private string _mahaiIzena;
        public string MahaiIzena { get => _mahaiIzena; set { _mahaiIzena = value; OnPropertyChanged(); } }

        private string _edukieraString;
        public string EdukieraString { get => _edukieraString; set { _edukieraString = value; OnPropertyChanged(); } }

        public RelayCommand GehituMahaiAgindua { get; private set; }
        public RelayCommand AldatuMahaiAgindua { get; private set; }
        public RelayCommand EzabatuMahaiAgindua { get; private set; }
        public RelayCommand GarbituMahaiAgindua { get; private set; }

        // Eraikitzailea eta Metodoak

        public AdminViewModel()
        {
            Erabiltzaileak = new ObservableCollection<Erabiltzailea>();
            // Komandoak hasieratu eta beren metodoak esleitu
            GehituAgindua = new RelayCommand(async (p) => await GehituErabiltzailea(), (p) => !string.IsNullOrEmpty(ErabiltzaileIzena));
            AldatuAgindua = new RelayCommand(async (p) => await AldatuErabiltzailea(), (p) => HautatutakoErabiltzailea != null);
            EzabatuAgindua = new RelayCommand(async (p) => await EzabatuErabiltzailea(), (p) => HautatutakoErabiltzailea != null);
            GarbituAgindua = new RelayCommand((p) => GarbituHautapena());

            // PRODUKTUEN HASIERAKETA
            Produktuak = new ObservableCollection<Produktua>();
            GehituProduktuAgindua = new RelayCommand(async (p) => await GehituProduktua(), (p) => !string.IsNullOrEmpty(ProduktuIzena));
            AldatuProduktuAgindua = new RelayCommand(async (p) => await AldatuProduktua(), (p) => HautatutakoProduktua != null);
            EzabatuProduktuAgindua = new RelayCommand(async (p) => await EzabatuProduktua(), (p) => HautatutakoProduktua != null);
            GarbituProduktuAgindua = new RelayCommand((p) => GarbituProduktuHautapena());

            // MAHAIEN HASIERAKETA
            Mahaiak = new ObservableCollection<Mahaia>();
            GehituMahaiAgindua = new RelayCommand(async (p) => await GehituMahaia(), (p) => !string.IsNullOrEmpty(MahaiIzena));
            AldatuMahaiAgindua = new RelayCommand(async (p) => await AldatuMahaia(), (p) => HautatutakoMahaia != null);
            EzabatuMahaiAgindua = new RelayCommand(async (p) => await EzabatuMahaia(), (p) => HautatutakoMahaia != null);
            GarbituMahaiAgindua = new RelayCommand((p) => GarbituMahaiHautapena());

            // ESKAEREN HASIERAKETA
            Eskaerak = new ObservableCollection<Eskaera>();
            HautatutakoEskaerarenLerroak = new ObservableCollection<EskaeraLerroa>();

            // ViewModel-a sortzean, datu-baseko erabiltzaileak kargatu
            Task.Run(async () =>
            {
                await KargatuErabiltzaileak();
                await KargatuProduktuak();
                await KargatuMahaiak();
                await KargatuEskaerak();
            });
        }

        private async Task KargatuErabiltzaileak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var users = await db.Erabiltzaileak.ToListAsync();
                // Eragiketak UI-ko elementuekin (ObservableCollection) UI-ren harian egin behar dira.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Erabiltzaileak.Clear();
                    foreach (var user in users)
                    {
                        Erabiltzaileak.Add(user);
                    }
                });
            }
        }

        private async Task GehituErabiltzailea()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var berria = new Erabiltzailea { ErabiltzaileIzena = this.ErabiltzaileIzena, Pasahitza = this.Pasahitza, Rola = this.Rola };
                db.Erabiltzaileak.Add(berria);
                await db.SaveChangesAsync();
                Erabiltzaileak.Add(berria); // Interfazeko zerrendan ere gehitu
            }
            GarbituHautapena();
        }

        private async Task AldatuErabiltzailea()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var aldatzeko = await db.Erabiltzaileak.FindAsync(HautatutakoErabiltzailea.Id);
                if (aldatzeko != null)
                {
                    aldatzeko.ErabiltzaileIzena = this.ErabiltzaileIzena;
                    aldatzeko.Pasahitza = this.Pasahitza;
                    aldatzeko.Rola = this.Rola;
                    await db.SaveChangesAsync();

                    // Interfazeko zerrenda eguneratu
                    await KargatuErabiltzaileak();
                }
            }
            GarbituHautapena();
        }

        private async Task EzabatuErabiltzailea()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var ezabatzeko = await db.Erabiltzaileak.FindAsync(HautatutakoErabiltzailea.Id);
                if (ezabatzeko != null)
                {
                    db.Erabiltzaileak.Remove(ezabatzeko);
                    await db.SaveChangesAsync();
                    Erabiltzaileak.Remove(HautatutakoErabiltzailea); // Interfazeko zerrendatik kendu
                }
            }
            GarbituHautapena();
        }

        private void GarbituHautapena()
        {
            HautatutakoErabiltzailea = null;
            ErabiltzaileIzena = "";
            Pasahitza = "";
            Rola = "";
        }

        // PRODUKTUEN METODOAK
        private async Task KargatuProduktuak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var prods = await db.Produktuak.ToListAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Produktuak.Clear();
                    foreach (var prod in prods)
                    {
                        Produktuak.Add(prod);
                    }
                });
            }
        }

        private async Task GehituProduktua()
        {
            if (!double.TryParse(this.PrezioaString.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double prezioNumerikoa))
            {
                MessageBox.Show("Sartutako prezioaren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(this.StockaString, out int stockNumerikoa))
            {
                MessageBox.Show("Sartutako stock-aren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. Gorde datu-basean
            await using (var db = new ElkarteaDbContext())
            {
                var berria = new Produktua { Izena = this.ProduktuIzena, Prezioa = prezioNumerikoa, Stocka = stockNumerikoa };
                db.Produktuak.Add(berria);
                await db.SaveChangesAsync();
                Produktuak.Add(berria);
            }
            GarbituProduktuHautapena();
        }

        private async Task AldatuProduktua()
        {
            if (HautatutakoProduktua == null) return;

            // 1. Bihurtu testua zenbaki.
            if (!double.TryParse(this.PrezioaString.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double prezioNumerikoa))
            {
                MessageBox.Show("Sartutako prezioaren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(this.StockaString, out int stockNumerikoa))
            {
                MessageBox.Show("Sartutako stock-aren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. Eguneratu datu-basean
            var updatedProduktua = new Produktua
            {
                Id = HautatutakoProduktua.Id,
                Izena = this.ProduktuIzena,
                Prezioa = prezioNumerikoa,
                Stocka = stockNumerikoa
            };

            await using (var db = new ElkarteaDbContext())
            {
                db.Produktuak.Update(updatedProduktua);
                await db.SaveChangesAsync();
                await KargatuProduktuak();
            }
            GarbituProduktuHautapena();
        }

        private async Task EzabatuProduktua()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var ezabatzeko = await db.Produktuak.FindAsync(HautatutakoProduktua.Id);
                if (ezabatzeko != null)
                {
                    db.Produktuak.Remove(ezabatzeko);
                    await db.SaveChangesAsync();
                    Produktuak.Remove(HautatutakoProduktua);
                }
            }
            GarbituProduktuHautapena();
        }

        private void GarbituProduktuHautapena()
        {
            HautatutakoProduktua = null;
            ProduktuIzena = "";
            PrezioaString = "0";
            StockaString = "0";
        }

        // MAHAIEN METODOAK
        private async Task KargatuMahaiak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var tables = await db.Mahaiak.ToListAsync();
                Application.Current.Dispatcher.Invoke(() => {
                    Mahaiak.Clear();
                    foreach (var table in tables) Mahaiak.Add(table);
                });
            }
        }

        private async Task GehituMahaia()
        {
            if (!int.TryParse(this.EdukieraString, out int edukieraNumerikoa))
            {
                MessageBox.Show("Sartutako edukiaren-aren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var berria = new Mahaia { MahaiIzena = this.MahaiIzena, Edukiera = edukieraNumerikoa };
            await using (var db = new ElkarteaDbContext())
            {
                db.Mahaiak.Add(berria);
                await db.SaveChangesAsync();
                Mahaiak.Add(berria);
            }
            GarbituMahaiHautapena();
        }

        private async Task AldatuMahaia()
        {
            if (!int.TryParse(this.EdukieraString, out int edukieraNumerikoa))
            {
                MessageBox.Show("Sartutako edukiaren-aren formatua ez da zuzena.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (HautatutakoMahaia == null) return;
            var updatedMahaia = new Mahaia { Id = HautatutakoMahaia.Id, MahaiIzena = this.MahaiIzena, Edukiera = edukieraNumerikoa };
            await using (var db = new ElkarteaDbContext())
            {
                db.Mahaiak.Update(updatedMahaia);
                await db.SaveChangesAsync();
                await KargatuMahaiak();
            }
            GarbituMahaiHautapena();
        }

        private async Task EzabatuMahaia()
        {
            if (HautatutakoMahaia == null) return;
            await using (var db = new ElkarteaDbContext())
            {
                db.Mahaiak.Remove(HautatutakoMahaia);
                await db.SaveChangesAsync();
                await KargatuMahaiak();
            }
            GarbituMahaiHautapena();
        }

        private void GarbituMahaiHautapena()
        {
            HautatutakoMahaia = null;
            MahaiIzena = "";
            EdukieraString = "0";
        }

        // ESKAEREN METODOAK
        private async Task KargatuEskaerak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var sales = await db.Eskaerak
                    .Include(e => e.Erabiltzailea) // Erabiltzailearen datuak ere ekartzeko
                    .OrderByDescending(e => e.Data) // Berrienak lehenengo
                    .ToListAsync();

                Application.Current.Dispatcher.Invoke(() => {
                    Eskaerak.Clear();
                    foreach (var sale in sales) Eskaerak.Add(sale);
                });
            }
        }

        private async Task KargatuEskaeraLerroak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                var lerroak = await db.EskaeraLerroak
                    .Include(l => l.Produktua) // Produktuaren izena erakusteko
                    .Where(l => l.EskaeraId == HautatutakoEskaera.Id)
                    .ToListAsync();

                Application.Current.Dispatcher.Invoke(() => {
                    HautatutakoEskaerarenLerroak.Clear();
                    foreach (var lerroa in lerroak) HautatutakoEskaerarenLerroak.Add(lerroa);
                });
            }
        }


    }
}

