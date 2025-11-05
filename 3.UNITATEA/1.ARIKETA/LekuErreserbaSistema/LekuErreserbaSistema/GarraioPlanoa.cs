using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LekuErreserbaSistema
{
    public class GarraioPlanoa
    {
        public List<Eserlekua> Eserlekuak { get; private set; }
        private int _zutabeBakoitza;

        public GarraioPlanoa(int ilarak, int zutabeBakoitza)
        {
            _zutabeBakoitza = Math.Max(1, zutabeBakoitza);
            Eserlekuak = new List<Eserlekua>();

            for (int i = 1; i <= ilarak; i++)
            {
                for (int j = 1; j <= _zutabeBakoitza; j++)
                {
                    string id = $"I{i}-Z{j}";
                    string zona = (j == 1 || j == _zutabeBakoitza) ? "Lehioa" : "Pasabidea";
                    Eserlekuak.Add(new Eserlekua(id, zona));
                }
            }
        }

        public GarraioPlanoa(List<Eserlekua> lehendikDaudenEserlekuak)
        {
            Eserlekuak = lehendikDaudenEserlekuak ?? new List<Eserlekua>();
            // Try to infer number of columns from IDs like "I{row}-Z{col}"
            _zutabeBakoitza = 1;
            try
            {
                var cols = Eserlekuak.Select(e =>
                {
                    if (string.IsNullOrEmpty(e?.Id)) return 1;
                    // Expect format I{row}-Z{col}
                    var dash = e.Id.IndexOf('-');
                    if (dash < 0) return 1;
                    var zPos = e.Id.IndexOf('Z', dash);
                    if (zPos < 0) return 1;
                    var colStr = e.Id.Substring(zPos + 1);
                    if (int.TryParse(colStr, out int c)) return c;
                    return 1;
                });
                if (cols.Any()) _zutabeBakoitza = cols.Max();
            }
            catch
            {
                _zutabeBakoitza = 1;
            }
        }

        // Left side seats (row by row)
        public IEnumerable<Eserlekua> EzkerrekoZutabeak => GetSideSeats(left: true);

        // Right side seats (row by row)
        public IEnumerable<Eserlekua> EskuinekoZutabeak => GetSideSeats(left: false);

        private IEnumerable<Eserlekua> GetSideSeats(bool left)
        {
            if (_zutabeBakoitza <= 1)
            {
                return left ? Eserlekuak : Enumerable.Empty<Eserlekua>();
            }

            int total = Eserlekuak.Count;
            int rows = (int)Math.Ceiling((double)total / _zutabeBakoitza);
            int leftCount = _zutabeBakoitza / 2; // e.g. for 4 cols -> 2 left, 2 right; for 3 -> 1 left, 2 right

            var result = new List<Eserlekua>();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < _zutabeBakoitza; c++)
                {
                    int idx = r * _zutabeBakoitza + c;
                    if (idx >= total) break;
                    bool isLeft = c < leftCount;
                    if (isLeft == left)
                    {
                        result.Add(Eserlekuak[idx]);
                    }
                }
            }
            return result;
        }

        // Leku zehatz bat bere IDaren bidez bilatzen du eta egoera aldatzen dio.
        public void ErreserbatuLekua(string lekuId)
        {
            var lekua = Eserlekuak.FirstOrDefault(l => l.Id == lekuId);

            if (lekua != null && lekua.Egoera == EgoeraEserlekua.Libre)
            {
                lekua.Egoera = EgoeraEserlekua.Okupatuta;
            }
        }

        // Erreserba bat bertan behera uzten du.
        public void UtziErreserbaBertanBehera(string lekuId)
        {
            var lekua = Eserlekuak.FirstOrDefault(l => l.Id == lekuId);
            if (lekua != null && lekua.Egoera == EgoeraEserlekua.Okupatuta)
            {
                lekua.Egoera = EgoeraEserlekua.Libre;
            }
        }

        public void BaieztatuErreserbak()
        {
            foreach (var eserlekua in this.Eserlekuak)
            {
                if (eserlekua.Egoera == EgoeraEserlekua.Hautatuta)
                {
                    eserlekua.Egoera = EgoeraEserlekua.Okupatuta;
                }
            }
        }
    }
}