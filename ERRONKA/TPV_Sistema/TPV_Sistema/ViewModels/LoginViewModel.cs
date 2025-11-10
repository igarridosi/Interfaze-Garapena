using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TPV_Sistema.Views;
using Microsoft.EntityFrameworkCore;

namespace TPV_Sistema.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // Propietateak View-arekin lotzeko
        private string _erabiltzaileIzena;
        public string ErabiltzaileIzena
        {
            get { return _erabiltzaileIzena; }
            set { _erabiltzaileIzena = value; OnPropertyChanged(nameof(ErabiltzaileIzena)); }
        }

        private string _erroreMezua;
        public string ErroreMezua
        {
            get { return _erroreMezua; }
            set { _erroreMezua = value; OnPropertyChanged(nameof(ErroreMezua)); }
        }

        // Komandoa (botoiarekin lotzeko)
        public RelayCommand SartuAgindua { get; private set; }

        public LoginViewModel()
        {
            SartuAgindua = new RelayCommand(async (param) => await Sartu(param), OnartuDaiteke);
        }

        // Sartu botoiaren logika
        private async Task Sartu(object parameter)
        {
            var pasahitzaBox = parameter as PasswordBox;
            if (pasahitzaBox == null) return;
            var pasahitza = pasahitzaBox.Password;

            ErroreMezua = ""; // Garbitu erroreak

            await using (var db = new ElkarteaDbContext())
            {
                try
                {
                    var erabiltzailea = await db.Erabiltzaileak
                        .FirstOrDefaultAsync(e => e.ErabiltzaileIzena == ErabiltzaileIzena && e.Pasahitza == pasahitza);

                    if (erabiltzailea != null)
                    {
                        if (erabiltzailea.Rola == "admin")
                        {
                            var adminWindow = new AdminWindow();
                            adminWindow.Show();
                        }
                        else
                        {
                            var erabiltzaileWindow = new ErabiltzaileWindow(erabiltzailea);
                            erabiltzaileWindow.Show();
                        }

                        var loginWindow = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();
                        loginWindow?.Close();
                    }
                    else
                    {
                        ErroreMezua = "Erabiltzaile edo pasahitz okerra.";
                    }
                }
                catch (Exception ex)
                {
                    // Errore bat gertatzen bada, mezu batean erakutsi
                    MessageBox.Show($"Datu-basearekin errore bat gertatu da: {ex.Message}");
                }
            }
        }

        // Botoia noiz gaitu/desgaitu daitekeen definitzen du (aukerakoa)
        private bool OnartuDaiteke(object parameter)
        {
            return !string.IsNullOrEmpty(ErabiltzaileIzena);
        }

        // INotifyPropertyChanged interfazearen inplementazioa
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
