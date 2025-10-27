using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LekuErreserbaSistema
{
    public partial class AutobusPlanoa : UserControl
    {
        public AutobusPlanoa()
        {
            InitializeComponent();
        }

        // DependencyProperty bat sortzen dugu kanpotik datuak jasotzeko.
        // Hau da WPF-n osagaien artean datuak lotzeko modu estandarra.
        public static readonly DependencyProperty PlanoaProperty =
            DependencyProperty.Register("Planoa", typeof(GarraioPlanoa), typeof(AutobusPlanoa), new PropertyMetadata(null, OnPlanoaChanged));

        // Propietate publikoa gure DependencyProperty-ra sartzeko
        public GarraioPlanoa Planoa
        {
            get { return (GarraioPlanoa)GetValue(PlanoaProperty); }
            set { SetValue(PlanoaProperty, value); }
        }

        // Planoa aldatzen denean (normalean hasieran), osagaiaren DataContext-a ezartzen dugu.
        private static void OnPlanoaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var kontrola = d as AutobusPlanoa;
            if (kontrola != null)
            {
                kontrola.DataContext = kontrola.Planoa;
            }
        }
    }
}
