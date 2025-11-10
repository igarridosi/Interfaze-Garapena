using System.Configuration;
using System.Data;
using System.Windows;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

namespace TPV_Sistema
{
    public partial class App : Application
    {
        public App()
        {
            // QuestPDF lizentzia aplikazioa hasi bezain laster ezarri
            QuestPDF.Settings.License = LicenseType.Community;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            // Aplikazioaren "kultura" nagusia ezarri
            // "es-ES" Espainiako gaztelania da, euroa (€) erabiltzen duena.
            // "eu-ES" ere erabil daiteke euskararako. Biek euroa erabiltzen dute.
            var culture = new CultureInfo("es-ES");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // WPF-ren datu-loturetarako (binding) hizkuntza ere ezarri behar da
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }

}
