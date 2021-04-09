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

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for ZakazivanjeTerminaSekretar.xaml
    /// </summary>
    public partial class ZakazivanjeTerminaSekretar : Window
    {
        private Termin termin;
        DataGrid terminiPrikaz;
        public ZakazivanjeTerminaSekretar(DataGrid terminiPrikaz)
        {
            InitializeComponent();
            this.terminiPrikaz = terminiPrikaz;
            datum.SelectedDate = DateTime.Now;
            termin = new Termin();
            SkladisteZaProstorije sklad = new SkladisteZaProstorije();
            sala.ItemsSource = sklad.GetAll();
            sala.SelectedIndex = 0;
            List<String> vremeTermina = new List<string>();
            vremeTermina.Add("08:00");
            vremeTermina.Add("08:30");
            vremeTermina.Add("09:00");
            vremeTermina.Add("09:30");
            vremeTermina.Add("10:00");
            vremeTermina.Add("10:30");
            vremeTermina.Add("11:00");
            vremeTermina.Add("11:30");
            vremeTermina.Add("12:00");
            vremeTermina.Add("12:30");
            vremeT.ItemsSource = vremeTermina;
            vremeT.SelectedIndex = 0;
            datum.DisplayDateStart = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (vrstaT.SelectedIndex == 0) 
            {
                termin.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
            }
            else
            {
                termin.VrstaTermina = Model.Enum.VrstaPregleda.Operacija;
            }
            termin.IdProstorije = int.Parse(((Prostorija)sala.SelectedItem).BrojSobe);
            DateTime selDate = (DateTime)datum.SelectedDate;

            string[] vreme = ((string)vremeT.SelectedItem).Split(':');
            int sati = int.Parse(vreme[0]);
            int minuti = int.Parse(vreme[1]);

            DateTime datumIVreme = new DateTime(selDate.Year, selDate.Month, selDate.Day, sati, minuti, 0);

            termin.DatumIVremeTermina = datumIVreme;
            termin.IdProstorije = int.Parse(((Prostorija)sala.SelectedItem).BrojSobe);

            termin.opisTegobe = tegobe.Text;
            termin.IDTermina = termin.generateRandId();
            SkladisteZaTermine.getInstance().Save(termin); 
            terminiPrikaz.ItemsSource = SkladisteZaTermine.getInstance().GetBuduciTerminPacLekar();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijentaTerminu(termin, pacijent);
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeLekaraTerminu(termin, lekar);
            s.Show();
        }
    }
}
