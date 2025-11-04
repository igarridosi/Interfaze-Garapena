using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Erreserba
    {
        public int Id { get; set; } // Primary Key
        public DateTime Data { get; set; } // Erreserbaren eguna
        public string Otordua { get; set; } // "bazkaria" edo "afaria"

        // Erlazioak beste taula batzuekin (Foreign Keys)
        public int ErabiltzaileId { get; set; }
        public int MahaiaId { get; set; }

        // Nabigazio-propietateak (EF Core-k erlazioak ulertzeko)
        public virtual Erabiltzailea Erabiltzailea { get; set; }
        public virtual Mahaia Mahaia { get; set; }
    }
}
