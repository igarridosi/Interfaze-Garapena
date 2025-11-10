using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class EskaeraLerroa : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _kantitatea;
        public int Kantitatea
        {
            get => _kantitatea;
            set { _kantitatea = value; OnPropertyChanged(); OnPropertyChanged(nameof(LerroarenTotala)); }
        }

        public double PrezioaUnitateko { get; set; }

        // Kalkulatutako propietate bat, interfazean erakusteko
        public double LerroarenTotala => Kantitatea * PrezioaUnitateko;

        public int EskaeraId { get; set; }
        public virtual Eskaera Eskaera { get; set; }

        public int ProduktuaId { get; set; }
        public virtual Produktua Produktua { get; set; }

        // INotifyPropertyChanged-en inplementazioa
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
