using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPV_Sistema.Views;

public static class NavigationService
{
    public static void BerrabiaraziAplikazioa(Window unekoLeihoa)
    {
        // 1. URRATSA: Login leiho BERRIA sortu eta erakutsi.
        // Orain bi leiho daude irekita.
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();

        // 2. URRATSA: Leiho ZAHARRA itxi.
        // Orain leiho bakarra geratzen da irekita (LoginWindow berria).
        // Aplikazioak ez du ixten, ez delako azken leihoa izan.
        if (unekoLeihoa != null)
        {
            unekoLeihoa.Close();
        }
    }
}
