using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Mahaia
    {
        public int Id { get; set; } // Primary Key
        public string MahaiIzena { get; set; } // Adibidez: "Mahaia 1", "Barra", "Goiko gela"
        public int Edukiera { get; set; } // Zenbat pertsona sartzen diren

        // Nabigazio-propietatea
        public virtual ICollection<Erreserba> Erreserbak { get; set; }
    }
}
