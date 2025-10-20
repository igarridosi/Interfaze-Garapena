using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekuErreserbaSistema
{
    public class GarraioPlanoa
    {
        public List<Eserlekua> Eserlekuak { get; private set; }

        public GarraioPlanoa(int ilarak, int zutabeBakoitza)
        {
            Eserlekuak = new List<Eserlekua>();

            for(int i = 1; i <= ilarak; i++)
            {
                for(int j = 1; j<= zutabeBakoitza; j++)
                {
                    string id = $"I{i}-Z{j}";
                    Eserlekuak.Add(new Eserlekua(id));
                }
            }
        }

        public GarraioPlanoa(List<Eserlekua> lehendikDaudenEserlekuak)
        {
            Eserlekuak = lehendikDaudenEserlekuak;
        }

        // Leku zehatz bat bere IDaren bidez bilatzen du eta egoera aldatzen dio.
        public void ErreserbatuLekua(string lekuId)
        {
            var lekua = Eserlekuak.FirstOrDefault(l => l.Id == lekuId);

            if (lekua != null && lekua.Egoera == EgoeraEserlekua.Libre)
            {
                lekua.Egoera = EgoeraEserlekua.Okupatuta;
                Console.WriteLine($"{lekuId} lekua ondo erreserbatu da.");
            }
            else
            {
                Console.WriteLine($"Ezin izan da {lekuId} lekua erreserbatu. Baliteke jada okupatuta egotea.");
            }
        }

        // Erreserba bat bertan behera uzten du.
        public void UtziErreserbaBertanBehera(string lekuId)
        {
            var lekua = Eserlekuak.FirstOrDefault(l => l.Id == lekuId);
            if (lekua != null && lekua.Egoera == EgoeraEserlekua.Okupatuta)
            {
                lekua.Egoera = EgoeraEserlekua.Libre;
                Console.WriteLine($"{lekuId} lekuaren erreserba bertan behera utzi da.");
            }
            else
            {
                Console.WriteLine($"Ezin izan da erreserba bertan behera utzi. {lekuId} ez dago okupatuta.");
            }
        }

        // Metodo honek erabiltzaileak hautatzen dituen lekuak kudeatzen ditu
        public void AldatuHautapenEgoera(string lekuId)
        {
            var lekua = Eserlekuak.FirstOrDefault(l => l.Id == lekuId);
            if (lekua != null)
            {
                if (lekua.Egoera == EgoeraEserlekua.Libre)
                {
                    lekua.Egoera = EgoeraEserlekua.Hautatuta;
                }
                else if (lekua.Egoera == EgoeraEserlekua.Hautatuta)
                {
                    lekua.Egoera = EgoeraEserlekua.Libre;
                }
            }
        }

        public void BaieztatuErreserbak()
        {
            // Zerrenda osoa arakatu
            foreach (var eserlekua in this.Eserlekuak)
            {
                // Eserleku bat "Hautatuta" badago...
                if (eserlekua.Egoera == EgoeraEserlekua.Hautatuta)
                {
                    // ...bihurtu "Okupatuta"
                    eserlekua.Egoera = EgoeraEserlekua.Okupatuta;
                }
            }
        }
    }
}
