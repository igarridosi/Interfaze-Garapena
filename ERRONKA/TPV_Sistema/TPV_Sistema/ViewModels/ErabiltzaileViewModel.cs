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
    public class ErabiltzaileViewModel : BaseViewModel
    {
        // PRODUKTUEN ZERRENDA OSOA (BOTOIAK SORTZEKO)
        public ObservableCollection<Produktua> ProduktuGuztiak { get; set; }

        // UNEKO TICKET-A OSATZEN DUTEN LERROAK
        public ObservableCollection<EskaeraLerroa> UnekoTicket { get; set; }

        private readonly Erabiltzailea _logeatutakoErabiltzailea;

        // TICKETAREN TOTALAK (INTERFAZEAN ERAKUSTEKO)
        private double _subtotala;
        public double Subtotala { get => _subtotala; set { _subtotala = value; OnPropertyChanged(); } }

        private double _totala;
        public double Totala { get => _totala; set { _totala = value; OnPropertyChanged(); } }

        // BOTOIEKIN LOTUTAKO KOMANDOAK
        public RelayCommand GehituProduktuTicketeraAgindua { get; private set; }
        public RelayCommand GordeTicketAgindua { get; private set; }
        public RelayCommand EzabatuLerroaAgindua { get; private set; }
        public RelayCommand GarbituTicketAgindua { get; private set; }

        // ERRESERBAK
        public ObservableCollection<Mahaia> Mahaiak { get; set; }
        public ObservableCollection<Erreserba> NireErreserbak { get; set; }

        private DateTime _hautatutakoData = DateTime.Today;
        public DateTime HautatutakoData { get => _hautatutakoData; set { _hautatutakoData = value; OnPropertyChanged(); } }

        private Mahaia _hautatutakoMahaia;
        public Mahaia HautatutakoMahaia { get => _hautatutakoMahaia; set { _hautatutakoMahaia = value; OnPropertyChanged(); } }

        public ObservableCollection<string> Otorduak { get; set; }
        private string _hautatutakoOtordua;
        public string HautatutakoOtordua { get => _hautatutakoOtordua; set { _hautatutakoOtordua = value; OnPropertyChanged(); } }

        private Erreserba _hautatutakoErreserba;
        public Erreserba HautatutakoErreserba { get => _hautatutakoErreserba; set { _hautatutakoErreserba = value; OnPropertyChanged(); } }

        public RelayCommand EginErreserbaAgindua { get; private set; }
        public RelayCommand EzabatuErreserbaAgindua { get; private set; }


        public ErabiltzaileViewModel(Erabiltzailea erabiltzailea)
        {
            _logeatutakoErabiltzailea = erabiltzailea; // Gorde jasotako erabiltzailea

            ProduktuGuztiak = new ObservableCollection<Produktua>();
            UnekoTicket = new ObservableCollection<EskaeraLerroa>();
            Mahaiak = new ObservableCollection<Mahaia>();

            // Komandoak hasieratu
            GehituProduktuTicketeraAgindua = new RelayCommand(GehituProduktuTicketera);
            GordeTicketAgindua = new RelayCommand(async (p) => await GordeTicket(), (p) => UnekoTicket.Any());
            EzabatuLerroaAgindua = new RelayCommand(EzabatuLerroa, (p) => p is EskaeraLerroa);
            GarbituTicketAgindua = new RelayCommand(GarbituTicket, (p) => UnekoTicket.Any());

            // ERRESERBEN HASIERAKETA (gehitu hau)
            NireErreserbak = new ObservableCollection<Erreserba>();
            Otorduak = new ObservableCollection<string> { "Bazkaria", "Afaria" };
            EginErreserbaAgindua = new RelayCommand(async (p) => await EginErreserba(), (p) => HautatutakoMahaia != null && !string.IsNullOrEmpty(HautatutakoOtordua));
            EzabatuErreserbaAgindua = new RelayCommand(async (p) => await EzabatuErreserba(), (p) => HautatutakoErreserba != null);

            // UnekoTicket bilduma aldatzen den bakoitzean, totalak birkalkulatu
            UnekoTicket.CollectionChanged += (s, e) => KalkulatuTotalak();

            // Metodo asinkrono bat deituko dugu try-catch batekin
            _ = InitializeAsync();

            /* Datuak kargatu
            Task.Run(async () => {
                await KargatuProduktuak();
                await KargatuMahaiak();
                await KargatuNireErreserbak();
            });
            */
        }

        private async Task InitializeAsync()
        {
            try
            {
                await KargatuProduktuak();
                await KargatuMahaiak();
                await KargatuNireErreserbak();
            }
            catch (Exception ex)
            {
                // Errore bat gertatzen bada kargatzean, erakutsi
                MessageBox.Show($"Errore bat gertatu da datuak kargatzean: {ex.Message}", "Karga Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task KargatuProduktuak()
        {
            await using (var db = new ElkarteaDbContext())
            {
                // Stock-a duten produktuak bakarrik erakutsi
                var prods = await db.Produktuak.Where(p => p.Stocka > 0).ToListAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProduktuGuztiak.Clear();
                    foreach (var p in prods)
                    {
                        ProduktuGuztiak.Add(p);
                    }
                });
            }
        }

        private void GehituProduktuTicketera(object parameter)
        {
            if (parameter is Produktua produktua)
            {
                // Egiaztatu produktua jada ticket-ean dagoen
                var lerroa = UnekoTicket.FirstOrDefault(l => l.ProduktuaId == produktua.Id);

                if (lerroa != null)
                {
                    // Jada badago, kantitatea handitu
                    lerroa.Kantitatea++;
                    // Propietateen aldaketak jakinarazteko, EskaeraLerroa klasea aldatu beharko dugu
                    OnPropertyChanged(nameof(UnekoTicket)); // Trikimailu bat interfazea freskatzeko
                }
                else
                {
                    // Ez badago, lerro berri bat sortu
                    UnekoTicket.Add(new EskaeraLerroa
                    {
                        ProduktuaId = produktua.Id,
                        Produktua = produktua, // Nabigazio-propietatea bete
                        Kantitatea = 1,
                        PrezioaUnitateko = produktua.Prezioa
                    });
                }
            }
            KalkulatuTotalak();
        }

        private void EzabatuLerroa(object parameter)
        {
            if (parameter is EskaeraLerroa lerroa)
            {
                if (lerroa.Kantitatea > 1)
                {
                    lerroa.Kantitatea--;
                    OnPropertyChanged(nameof(UnekoTicket)); // Freskatu
                }
                else
                {
                    UnekoTicket.Remove(lerroa);
                }
            }
            KalkulatuTotalak();
        }

        private void KalkulatuTotalak()
        {
            Subtotala = UnekoTicket.Sum(l => l.Kantitatea * l.PrezioaUnitateko);
            Totala = Subtotala; // Momentuz ez dugu zergarik gehituko
            OnPropertyChanged(nameof(Subtotala)); // Propietatearen aldaketaren berri eman
            OnPropertyChanged(nameof(Totala));
        }

        private void GarbituTicket(object parameter)
        {
            UnekoTicket.Clear();
        }

        private async Task GordeTicket()
        {
            // 1. Egiaztatu baldintzak betetzen direla gorde aurretik
            if (!UnekoTicket.Any())
            {
                MessageBox.Show("Ezin da ticket huts bat gorde.");
                return;
            }

            if (_logeatutakoErabiltzailea == null)
            {
                MessageBox.Show("ERRORE KRITIKOA: Saioa hasitako erabiltzailea ez da aurkitu.");
                return;
            }

            // 2. Datu-basearekin lan egin
            await using (var db = new ElkarteaDbContext())
            {
                // Bilatu saioa hasitako erabiltzailea uneko DbContext-aren barruan
                var erabiltzaileaDB = await db.Erabiltzaileak.FindAsync(_logeatutakoErabiltzailea.Id);
                if (erabiltzaileaDB == null)
                {
                    MessageBox.Show("ERRORE KRITIKOA: Saioa hasitako erabiltzailea ez da datu-basean existitzen.");
                    return;
                }

                // Sortu eskaera berria, oraingoan erabiltzaile objektu zuzenarekin
                var eskaeraBerria = new Eskaera
                {
                    Data = DateTime.Now,
                    Guztira = this.Totala,
                    Erabiltzailea = erabiltzaileaDB
                };

                // Prozesatu lerroak
                foreach (var ticketLerroa in UnekoTicket)
                {
                    var produktuaDB = await db.Produktuak.FindAsync(ticketLerroa.ProduktuaId);
                    if (produktuaDB != null)
                    {
                        produktuaDB.Stocka -= ticketLerroa.Kantitatea;

                        eskaeraBerria.Lerroak.Add(new EskaeraLerroa
                        {
                            Produktua = produktuaDB,
                            Kantitatea = ticketLerroa.Kantitatea,
                            PrezioaUnitateko = ticketLerroa.PrezioaUnitateko
                        });
                    }
                }

                // Gorde dena
                db.Eskaerak.Add(eskaeraBerria);
                await db.SaveChangesAsync();
            }

            // 3. Amaierako ekintzak
            MessageBox.Show("Salmenta ondo gorde da!", "Eginda", MessageBoxButton.OK, MessageBoxImage.Information);
            GarbituTicket(null);
            await KargatuProduktuak();
        }
        // ERRESERBEN METODOAK (gehitu metodo berri hauek)
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

        private async Task KargatuNireErreserbak()
        {
            // Egiaztapen gehigarria
            if (_logeatutakoErabiltzailea == null) return;

            await using (var db = new ElkarteaDbContext())
            {
                // 1. ZIURTATU DATAREN ZATIA BAKARRIK KONPARATZEN DUGULA
                var gaur = DateTime.Today;

                var myReservations = await db.Erreserbak
                    .Include(r => r.Mahaia)
                    // 2. ERABILTZAILEAREN ID-a ZUZENA DELA ZIURTATU
                    .Where(r => r.ErabiltzaileaId == _logeatutakoErabiltzailea.Id && r.Data.Date >= gaur)
                    .OrderBy(r => r.Data)
                    .ToListAsync();

                // DEBUGGING: Jarri eten-puntu bat (breakpoint) beheko lerroan
                // eta ikusi "myReservations" aldagaiak zenbat elementu dituen.
                Application.Current.Dispatcher.Invoke(() => {
                    NireErreserbak.Clear();
                    foreach (var res in myReservations)
                    {
                        NireErreserbak.Add(res);
                    }
                });
            }
        }

        private async Task EginErreserba()
        {
            // 1. Egiaztatu datuak hautatuta daudela
            if (HautatutakoMahaia == null || string.IsNullOrEmpty(HautatutakoOtordua))
            {
                MessageBox.Show("Mesedez, hautatu mahai bat eta otordu bat erreserba egiteko.", "Datuak falta dira", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Datu-basearekin lan egin instantzia bakar batean
            await using (var db = new ElkarteaDbContext())
            {
                // Lehenik, egiaztatu ea mahaia jada okupatuta dagoen
                bool okupatuta = await db.Erreserbak.AnyAsync(r =>
                    r.MahaiaId == HautatutakoMahaia.Id &&
                    r.Data.Date == HautatutakoData.Date &&
                    r.Otordua == HautatutakoOtordua);

                if (okupatuta)
                {
                    MessageBox.Show("Mahaia jada erreserbatuta dago data eta otordu horretarako.", "Okupatuta", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Lortu uneko DbContext-ak jarraitzen dituen objektuak ("tracked objects")
                var mahaiaDB = await db.Mahaiak.FindAsync(HautatutakoMahaia.Id);
                var erabiltzaileaDB = await db.Erabiltzaileak.FindAsync(_logeatutakoErabiltzailea.Id);

                // Segurtasun-egiaztapena
                if (mahaiaDB == null || erabiltzaileaDB == null)
                {
                    MessageBox.Show("Errore bat gertatu da: Mahaia edo erabiltzailea ez da datu-basean aurkitu.", "Errore Kritikoa", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Sortu erreserba berria, nabigazio-propietateak erabiliz
                var berria = new Erreserba
                {
                    Erabiltzailea = erabiltzaileaDB,
                    Mahaia = mahaiaDB,
                    Data = HautatutakoData.Date,
                    Otordua = HautatutakoOtordua
                };

                db.Erreserbak.Add(berria);
                await db.SaveChangesAsync();
            }

            // 3. Eguneratu interfazea
            await KargatuNireErreserbak();
            MessageBox.Show("Erreserba ondo egin da!", "Eginda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task EzabatuErreserba()
        {
            // Egiaztatu erreserba bat hautatuta dagoela
            if (HautatutakoErreserba == null) return;

            var result = MessageBox.Show("Ziur zaude erreserba hau ezabatu nahi duzula?", "Ezabatu", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            await using (var db = new ElkarteaDbContext())
            {
                // Bilatu ezabatu beharreko erreserba DbContext honen barruan
                var ezabatzeko = await db.Erreserbak.FindAsync(HautatutakoErreserba.Id);

                if (ezabatzeko != null)
                {
                    db.Erreserbak.Remove(ezabatzeko);
                    await db.SaveChangesAsync();
                }
            }

            // Eguneratu zerrenda ezabatu ondoren
            await KargatuNireErreserbak();
        }
    }
}
