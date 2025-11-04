using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Eskaera
    {
        public int Id { get; set; } // Primary Key
        public DateTime Data { get; set; } // Eskaera noiz egin den
        public double Guztira { get; set; } // Ticket-aren totala

        // Erlazioa (nork egin du eskaera)
        public int ErabiltzaileId { get; set; }
        public virtual Erabiltzailea Erabiltzailea { get; set; }

        // Erlazioa (eskaera honek zein lerro dituen)
        public virtual ICollection<Eskaera> Lerroak { get; set; }

        public Eskaera()
        {
            // Garrantzitsua: bilduma hasieratzea, erroreak saihesteko
            Lerroak = new List<Eskaera>();
        }
    }
}
