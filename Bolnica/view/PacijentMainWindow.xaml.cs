using Bolnica.viewActions;
using Model;
using Repozitorijum;
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
using System.Windows.Shapes;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PacijentMainWindow.xaml
    /// </summary>
    public partial class PacijentMainWindow : Window
    {
        public static PacijentMainWindow instance = null;

        public static PacijentMainWindow getInstance()
        {
            return instance;
        }
        public Pacijent pacijent { get; set; }

        public PacijentMainWindow(Pacijent pacijent)
        {
            InitializeComponent();
            instance = this;
            this.pacijent = pacijent;
            Ime.DataContext = pacijent;
            DataContext = new MainViewModel();
            //DataContext = pacijent;
           /* if (pacijent.Pol == Model.Enum.Pol.Muski)
            {
                fotPacijenta.Source = new BitmapImage(new Uri("...//...//Images//patient.png");
            } else
            {
                fotPacijenta.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/placeholder.png"));
            }*/
        }

        private void Odjava(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }
    }
}
