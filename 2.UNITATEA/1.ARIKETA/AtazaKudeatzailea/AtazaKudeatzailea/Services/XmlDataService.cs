using AtazaKudeatzailea.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AtazaKudeatzailea.Services
{
    public class XmlDataService
    {
        private readonly string _filePath;

        public XmlDataService(string filePath)
        {
            _filePath = filePath;
            // Ziurtatu "Data" direktorioa existitzen dela
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        public List<Ataza> Kargatu()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Ataza>();
            }

            try
            {
                XDocument doc = XDocument.Load(_filePath);

                return doc.Descendants("Tarea").Select(t => new Ataza
                {
                    Id = (int)t.Attribute("id"),
                    Titulua = (string)t.Element("Titulua"),
                    Lehentasuna = (LehentasunMota)Enum.Parse(typeof(LehentasunMota), (string)t.Element("Lehentasuna")),
                    AzkenEguna = (DateTime)t.Element("AzkenEguna"),
                    Egina = (string)t.Element("Egoera") == "Eginda"
                }).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errorea XML kargatzean: " + ex.Message);
                return new List<Ataza>();
            }
        }

        public void Gorde(IEnumerable<Ataza> atazak)
        {
            XDocument doc = new XDocument(
                new XElement("Atazas",
                    atazak.Select(t => new XElement("Ataza",
                        new XAttribute("id", t.Id),
                        new XElement("Titulua", t.Titulua),
                        new XElement("Lehentasuna", t.Lehentasuna.ToString()),
                        new XElement("AzkenEguna", t.AzkenEguna.ToString("yyyy-MM-dd")),
                        new XElement("Egoera", t.Egina ? "Eginda" : "Egin gabe")
                    ))
                )
            );
            doc.Save(_filePath);
        }
    }
}
