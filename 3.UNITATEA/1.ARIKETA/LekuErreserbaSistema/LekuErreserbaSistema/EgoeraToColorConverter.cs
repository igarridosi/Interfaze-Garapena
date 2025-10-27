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
                    case EgoeraEserlekua.Hautatuta:
                        return Brushes.LightBlue;
                    case EgoeraEserlekua.Okupatuta:
                        return Brushes.LightCoral;
                    case EgoeraEserlekua.Libre:
                    default:
                        // Itzuli "ezarri gabe" kolorea, botoiak bere kolore lehenetsia har dezan
                        return DependencyProperty.UnsetValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}