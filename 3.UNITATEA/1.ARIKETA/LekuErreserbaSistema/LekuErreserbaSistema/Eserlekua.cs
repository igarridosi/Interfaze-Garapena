using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekuErreserbaSistema
{
    public enum EgoeraLekua
    {
        Libre,
        Okupatuta,
        Hautatuta
    }
    public class Eserlekua
    {
        public string Id { get; set; }
        public EgoeraLekua Egoera { get; set; }

        public Eserlekua(string id)
        {
            Id = id;
            Egoera = EgoeraLekua.Libre;
        }

    }
}
