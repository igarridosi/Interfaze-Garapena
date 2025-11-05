using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TPV_Sistema.Services
{
    public class WindowService
    {
        public void ShowWindow(Window window)
        {
            window.Show();
        }

        public void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
