using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace LekuErreserbaSistema
{
    // IValueConverter interfazeak esaten dio klase honek datuak bihurtzen dakiela
    public class EgoeraToColorConverter : IValueConverter
    {
        // Metodo honek datua (Egoera) kolore bihurtzen du
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EgoeraEserlekua egoera)
            {
                switch (egoera)
                {
                    case EgoeraEserlekua.Libre:
                        return Brushes.White;
                    case EgoeraEserlekua.Okupatuta:
                        return Brushes.Red;
                    case EgoeraEserlekua.Hautatuta:
                        return Brushes.LightBlue;
                    default:
                        return Brushes.Gray;
                }
            }
            return Brushes.Gray;
        }

        // Ez dugu hau erabiliko, baina interfazeak behartzen gaitu jartzera
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}