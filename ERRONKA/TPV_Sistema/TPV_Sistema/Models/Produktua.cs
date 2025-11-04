using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class Produktua
    {
        public int Id { get; set; }
        public string Izena { get; set; }
        public double Prezioa { get; set; }
        public int Stocka { get; set; }

        // Nabigazio-propietatea
        public virtual ICollection<Eskaera> EskaeraLerroak { get; set; }
    }
}
