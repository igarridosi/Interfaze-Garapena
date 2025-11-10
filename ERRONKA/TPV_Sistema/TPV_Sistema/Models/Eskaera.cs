using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Eskaera
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public double Guztira { get; set; }
        public int ErabiltzaileId { get; set; }
        public virtual Erabiltzailea Erabiltzailea { get; set; }

        // ZIURTATU LERRO HAU HORRELA DAGOELA
        public virtual ICollection<EskaeraLerroa> Lerroak { get; set; }

        public Eskaera()
        {
            Lerroak = new List<EskaeraLerroa>();
        }
    }
}
