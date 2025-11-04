using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Erabiltzailea
    {
        public int Id { get; set; }
        public string ErabiltzaileIzena { get; set; }
        public string Pasahitza { get; set; }
        public string Rola { get; set; } // "admin" edo "erabiltzailea"

        // Nabigazio-propietateak (erlazioak definitzeko)
        public virtual ICollection<Erreserba> Erreserbak { get; set; }
        public virtual ICollection<Eskaera> Eskaerak { get; set; }
    }
}
