using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.Models
{
    public class EskaeraLerroa
    {
        public int Id { get; set; } // Primary Key
        public int Kantitatea { get; set; }
        public double PrezioaUnitateko { get; set; } // Produktuaren prezioa gorde saltzeko unean

        // Erlazioak
        public int EskaeraId { get; set; }
        public virtual Eskaera Eskaera { get; set; }

        public int ProduktuaId { get; set; }
        public virtual Produktua Produktua { get; set; }
    }
}
