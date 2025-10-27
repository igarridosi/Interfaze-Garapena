using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace LekuErreserbaSistema
{
    // Egoera testu bezala gordetzeko JSON fitxategian (irakurgarriagoa da)
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum EgoeraEserlekua
    {
        Libre,
        Okupatuta,
        Hautatuta
    }

    public class Eserlekua : INotifyPropertyChanged
    {
        private EgoeraEserlekua _egoera;
        private DateTime? _erreserbaData;

        public string Id { get; set; }
        public string Zona { get; set; }

        public EgoeraEserlekua Egoera
        {
            get { return _egoera; }
            set
            {
                if (_egoera != value)
                {
                    _egoera = value;
                    OnPropertyChanged(); // Interfazeari jakinarazi egoera aldatu dela
                }
            }
        }

        // Data gordetzeko propietate berria
        public DateTime? ErreserbaData
        {
            get { return _erreserbaData; }
            set
            {
                if (_erreserbaData != value)
                {
                    _erreserbaData = value;
                    OnPropertyChanged();
                }
            }
        }

        // JSON-ek erabiltzeko eraikitzaile hutsa
        public Eserlekua() { }

        // Gure kodeak erabiltzeko eraikitzailea
        public Eserlekua(string id, string zona)
        {
            Id = id;
            Zona = zona;
            Egoera = EgoeraEserlekua.Libre;
            ErreserbaData = null;
        }

        // Interfazea eguneratzeko beharrezko kodea
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}