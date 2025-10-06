using AtazaKudeatzailea.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtazaKudeatzailea.ViewModels
{
    public class AtazaEditatuViewModel : INotifyPropertyChanged
    {
        public Ataza Ataza { get; set; }

        // ComboBox-a betetzeko Lehentasun mota guztien zerrenda
        public Array LehentasunMotak => Enum.GetValues(typeof(LehentasunMota));

        public AtazaEditatuViewModel(Ataza ataza)
        {
            Ataza = ataza;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
