using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TPV_Sistema.Models;
using TPV_Sistema.ViewModels;

namespace TPV_Sistema.Views
{
    public partial class ErabiltzaileWindow : Window
    {
        public ErabiltzaileWindow(Erabiltzailea logeatutakoErabiltzailea)
        {
            InitializeComponent();
            // Erabiltzaile objektua jaso eta ViewModel-ari pasatu
            this.DataContext = new ErabiltzaileViewModel(logeatutakoErabiltzailea);
        }
    }
}
