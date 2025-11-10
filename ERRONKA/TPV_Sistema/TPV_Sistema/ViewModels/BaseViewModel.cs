using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Sistema.ViewModels
{
    // Hemendik aurrera, gure ViewModel berriek `BaseViewModel`-tik heredatuko dute,
    // eta ez dugu `OnPropertyChanged` metodoa behin eta berriro idatzi beharko.

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
