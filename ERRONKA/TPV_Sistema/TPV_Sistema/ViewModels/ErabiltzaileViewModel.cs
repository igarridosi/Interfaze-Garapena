using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuestPDF.Fluent;
using System.Diagnostics;
using System.IO;
using TPV_Sistema.Services;
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

        // TICKET PDF
        public RelayCommand InprimatuTicketAgindua { get; private set; }

        // KANTITATEA
        private string _kantitateaKatean = "0";
        public string KantitateaKatean
        {
            get => _kantitateaKatean;
            set { _kantitateaKatean = value; OnPropertyChanged(); }
        }

        public RelayCommand GehituDigitoaAgindua { get; private set; }
        public RelayCommand GarbituKantitateaAgindua { get; private set; }

        private const double BEZ_TASA = 0.21; // %21eko BEZ-a

        private double _bezZenbatekoa;
        public double BezZenbatekoa { get => _bezZenbatekoa; set { _bezZenbatekoa = value; OnPropertyChanged(); } }

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

            // TICKET
            GordeTicketAgindua = new RelayCommand(async (p) => await GordeEtaInprimatuAukera(), (p) => UnekoTicket.Any());

            GehituDigitoaAgindua = new RelayCommand(GehituDigitoa);
            GarbituKantitateaAgindua = new RelayCommand(GarbituKantitatea);

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

        private void GehituDigitoa(object parameter)
        {
            var digitoa = parameter as string;
            if (string.IsNullOrEmpty(digitoa)) return;

            if (KantitateaKatean == "0")
            {
                KantitateaKatean = digitoa;
            }
            else
            {
                KantitateaKatean += digitoa;
            }
        }

        private void GarbituKantitatea(object parameter)
        {
            KantitateaKatean = "0";
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
                // 1. Irakurri eta bihurtu kantitatea
                if (!int.TryParse(KantitateaKatean, out int kantitatea) || kantitatea <= 0)
                {
                    kantitatea = 1;
                }

                // Egiaztatu produktua jada ticket-ean dagoen
                var lerroa = UnekoTicket.FirstOrDefault(l => l.ProduktuaId == produktua.Id);

                if (lerroa != null)
                {
                    // Jada badago, kantitatea handitu
                    lerroa.Kantitatea += kantitatea;
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
                        Kantitatea = kantitatea,
                        PrezioaUnitateko = produktua.Prezioa
                    });
                }
            }
            GarbituKantitatea(null);
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

            BezZenbatekoa = Subtotala * BEZ_TASA;

            Totala = Subtotala + BezZenbatekoa; // Momentuz ez dugu zergarik gehituko
            OnPropertyChanged(nameof(Subtotala)); // Propietatearen aldaketaren berri eman
            OnPropertyChanged(nameof(BezZenbatekoa));
            OnPropertyChanged(nameof(Totala));
        }

        private void GarbituTicket(object parameter)
        {
            UnekoTicket.Clear();
        }

        private async Task<Eskaera> GordeTicket()
        {
            if (!UnekoTicket.Any()) return null;

            Eskaera eskaeraBerria; // Aldagaia kanpoan deklaratu

            await using (var db = new ElkarteaDbContext())
            {
                var erabiltzaileaDB = await db.Erabiltzaileak.FindAsync(_logeatutakoErabiltzailea.Id);
                if (erabiltzaileaDB == null)
                {
                    MessageBox.Show("ERRORE KRITIKOA: Erabiltzailea ez da datu-basean aurkitu.");
                    return null;
                }

                eskaeraBerria = new Eskaera
                {
                    Data = DateTime.Now,
                    Guztira = this.Totala,
                    Erabiltzailea = erabiltzaileaDB
                };

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

                db.Eskaerak.Add(eskaeraBerria);
                await db.SaveChangesAsync();
            }

            // Gorde ondoren, datu-basetik berriro irakurri erlazio guztiak ondo kargatzeko
            await using (var db = new ElkarteaDbContext())
            {
                // Garrantzitsua: Include erabili datu guztiak ekartzeko (Produktuak...)
#pragma warning disable CS8603 // Possible null reference return.
                return await db.Eskaerak
                    .Include(e => e.Lerroak)
                    .ThenInclude(l => l.Produktua)
                    .FirstOrDefaultAsync(e => e.Id == eskaeraBerria.Id);
#pragma warning restore CS8603 // Possible null reference return.
            }
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
                var gaur = DateTime.Today;

                var myReservations = await db.Erreserbak
                    .Include(r => r.Mahaia)
                    // ERABILTZAILEAREN ID-a ZUZENA DELA ZIURTATU
                    .Where(r => r.ErabiltzaileaId == _logeatutakoErabiltzailea.Id && r.Data.Date >= gaur)
                    .OrderBy(r => r.Data)
                    .ToListAsync();

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

        private void InprimatuTicket(Eskaera eskaera)
        {
            if (eskaera == null) return;

            string filePath = string.Empty;

            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // "Tickets" azpi-karpeta bat definitu
                string ticketsDirectory = Path.Combine(baseDirectory, "Tickets");

                // Egiaztatu karpeta hori existitzen den; ez bada, sortu
                Directory.CreateDirectory(ticketsDirectory);

                // Fitxategiaren bide osoa eraiki
                filePath = Path.Combine(ticketsDirectory, $"Ticket_{eskaera.Id}_{eskaera.Data:yyyyMMdd_HHmmss}.pdf");

                var document = new TicketDocument(eskaera, this.Subtotala, this.BezZenbatekoa);
                document.GeneratePdf(filePath);

                if (File.Exists(filePath))
                {
                    // Fitxategia ireki
                    var processStartInfo = new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true // GARRANTZITSUA .pdf-ak sistema eragilearen programa lehenetsiarekin irekitzeko
                    };
                    Process.Start(processStartInfo);
                }
                else
                {
                    // Fitxategia ez bada sortu, errore espezifiko bat bota
                    throw new FileNotFoundException("PDFa sortu behar zen, baina ezin izan da fitxategia aurkitu irekitzeko.", filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore bat gertatu da ticketa kudeatzean:\n\n{ex.Message}\n\nFitxategiaren bidea: {filePath}", "Inprimaketa Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Komandoen logika kudeatzeko metodo berria
        public async Task GordeEtaInprimatuAukera()
        {
            var gordetakoEskaera = await GordeTicket();

            if (gordetakoEskaera != null)
            {
                var result = MessageBox.Show("Salmenta ondo gorde da. Ticketa inprimatu nahi duzu?", "Eginda", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    InprimatuTicket(gordetakoEskaera);
                }

                GarbituTicket(null);
                await KargatuProduktuak();
            }
        }
    }
}
