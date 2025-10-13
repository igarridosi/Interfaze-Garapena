using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AtazaKudeatzailea.Models
{
    public enum LehentasunMota
    {
        Baxua,
        Ertaina,
        Handia
    }

    public class Ataza : INotifyPropertyChanged
    {
        private string _titulua;
        private LehentasunMota _lehentasuna;
        private DateTime _azkenEguna;
        private bool _egina;

        public int Id { get; set; }

        public string Titulua
        {
            get => _titulua;
            set { _titulua = value; OnPropertyChanged(); }
        }
        
        public LehentasunMota Lehentasuna
        {
            get => _lehentasuna;
            set { _lehentasuna = value; OnPropertyChanged(); }
        }

        public DateTime AzkenEguna
        {
            get => _azkenEguna;
            set { _azkenEguna = value; OnPropertyChanged(); }
        }

        public bool Egina
        {
            get => _egina;
            set { _egina = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
