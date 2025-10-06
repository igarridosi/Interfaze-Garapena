using AtazaKudeatzailea.Models;
using AtazaKudeatzailea.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace AtazaKudeatzailea.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly XmlDataService _dataService;
        private Ataza _hautatutakoAtaza;

        public ObservableCollection<Ataza> Atazak { get; set; }

        public Ataza HautatutakoAtaza
        {
            get => _hautatutakoAtaza;
            set
            {
                _hautatutakoAtaza = value;
                OnPropertyChanged();
            }
        }

        // Komandoak
        public ICommand GehituAtazaCmd { get; }
        public ICommand EditatuAtazaCmd { get; }
        public ICommand EzabatuAtazaCmd { get; }
        public ICommand IrtenCmd { get; }

        public MainViewModel()
        {
            _dataService = new XmlDataService("./Data/atazak.xml");
            Atazak = new ObservableCollection<Ataza>(_dataService.Kargatu());

            // Komandoen hasieraketa
            GehituAtazaCmd = new RelayCommand(param => GehituAtaza());
            EditatuAtazaCmd = new RelayCommand(param => EditatuAtaza(), param => HautatutakoAtaza != null);
            EzabatuAtazaCmd = new RelayCommand(param => EzabatuAtaza(), param => HautatutakoAtaza != null);
            IrtenCmd = new RelayCommand(param => App.Current.MainWindow.Close());
        }

        private void GehituAtaza()
        {
            var atazaBerria = new Ataza
            {
                Id = (Atazak.Any() ? Atazak.Max(t => t.Id) : 0) + 1, // ID berri bat kalkulatu
                AzkenEguna = DateTime.Today // Data lehenetsia
            };

            var viewModel = new AtazaEditatuViewModel(atazaBerria); // ViewModel bat sortuko dugu honentzat
            var dialog = new AtazaEditatuWindow
            {
                DataContext = viewModel
            };

            if (dialog.ShowDialog() == true)
            {
                Atazak.Add(atazaBerria);
                GordeDatuak();
            }
        }


        private void EditatuAtaza()
        {
            if (HautatutakoAtaza == null) return;

            // Kopia bat sortzen dugu "Utzi" sakatuz gero aldaketak ez gordetzeko
            var atazaKopia = new Ataza
            {
                Id = HautatutakoAtaza.Id,
                Titulua = HautatutakoAtaza.Titulua,
                Lehentasuna = HautatutakoAtaza.Lehentasuna,
                AzkenEguna = HautatutakoAtaza.AzkenEguna,
                Egina = HautatutakoAtaza.Egina
            };

            var viewModel = new AtazaEditatuViewModel(atazaKopia);
            var dialog = new AtazaEditatuWindow
            {
                DataContext = viewModel
            };

            if (dialog.ShowDialog() == true)
            {
                // Erabiltzaileak "Gorde" sakatu badu, jatorrizko ataza eguneratzen dugu
                HautatutakoAtaza.Titulua = atazaKopia.Titulua;
                HautatutakoAtaza.Lehentasuna = atazaKopia.Lehentasuna;
                HautatutakoAtaza.AzkenEguna = atazaKopia.AzkenEguna;
                GordeDatuak();
            }
        }

        private void EzabatuAtaza()
        {
            if (HautatutakoAtaza != null)
            {
                Atazak.Remove(HautatutakoAtaza);
                GordeDatuak();
            }
        }

        private void GordeDatuak()
        {
            _dataService.Gorde(Atazak);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
