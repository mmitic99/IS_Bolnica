using Bolnica.model;
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
    /// Interaction logic for IzmenaTermina.xaml
    /// </summary>
    public partial class IzmenaTermina : Window
    {
        private TerminPacijentLekar termin;
        DataGrid terminiPrikaz;
        public IzmenaTermina(DataGrid terminiPrikaz)
        {
            InitializeComponent();
            this.terminiPrikaz = terminiPrikaz;
            this.termin = (TerminPacijentLekar)terminiPrikaz.SelectedItem;
            if(termin.termin.VrstaTermina == Model.Enum.VrstaPregleda.Pregled)
            {
                vrstaT.SelectedIndex = 0;
            }
            else
            {
                vrstaT.SelectedIndex = 1;
            }
            pacijent.Text = termin.pacijent.Ime + " " + termin.pacijent.Prezime;
            lekar.Text = termin.lekar.Ime + " " + termin.lekar.Prezime;

            datum.SelectedDate = termin.termin.DatumIVremeTermina;
            String sati = termin.termin.DatumIVremeTermina.Hour.ToString();
            String minuti = termin.termin.DatumIVremeTermina.Minute.ToString();
            String vreme;
            if (termin.termin.DatumIVremeTermina.Hour >= 0 && termin.termin.DatumIVremeTermina.Hour <= 9)
            {
                vreme = "0" + sati + ":";
            }
            else
            {
                vreme = sati + ":";
            }

            if (termin.termin.DatumIVremeTermina.Minute >= 0 && termin.termin.DatumIVremeTermina.Minute <= 9)
            {
                vreme += "0" + minuti;
            }
            else
            {
                vreme += minuti;
            }
            vremeT.Items.Add("08:00");
            vremeT.Items.Add("08:30");
            vremeT.Items.Add("09:00");
            vremeT.Items.Add("09:30");
            vremeT.Items.Add("10:00");
            vremeT.Items.Add("10:30");
            vremeT.Items.Add("11:00");
            vremeT.Items.Add("11:30");
            vremeT.Items.Add("12:00");
            vremeT.Items.Add("12:30");
            if (!vremeT.Items.Contains(vreme))
            {
                vremeT.Items.Add(vreme);
            }
            vremeT.SelectedItem = vreme;
            sala.ItemsSource = new SkladisteZaProstorije().GetAll();
            for (int i = 0; i < sala.Items.Count; i++)
            {
                if(((Prostorija)sala.Items[i]).BrojSobe.Equals(termin.termin.IdProstorije.ToString()))
                {
                    sala.SelectedIndex = i;
                }
            }
            tegobe.Text = termin.termin.opisTegobe;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Termin noviTermin = new Termin
            {
                IdProstorije = int.Parse(((Prostorija)sala.SelectedItem).BrojSobe),
                opisTegobe = tegobe.Text,
                JmbgPacijenta = termin.pacijent.Jmbg,
                JmbgLekara = termin.lekar.Jmbg,
                VrstaTermina = termin.termin.VrstaTermina
            };
            
            DateTime selDate = (DateTime)datum.SelectedDate;

            string[] vreme = ((string)vremeT.SelectedItem).Split(':');
            int sati = int.Parse(vreme[0]);
            int minuti = int.Parse(vreme[1]);

            DateTime datumIVreme = new DateTime(selDate.Year, selDate.Month, selDate.Day, sati, minuti, 0);

            noviTermin.DatumIVremeTermina = datumIVreme;

            SkladisteZaTermine.getInstance().RemoveByID(termin.termin.IDTermina);
            noviTermin.IDTermina = noviTermin.generateRandId();

            Obavestenje obavestenjePacijent = new Obavestenje
            {
                JmbgKorisnika = termin.pacijent.Jmbg,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.termin.DatumIVremeTermina + "" +
                " je pomeren na " + noviTermin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            SkladisteZaObavestenja.GetInstance().Save(obavestenjePacijent);

            Obavestenje obavestenjeLekar = new Obavestenje
            {
                JmbgKorisnika = termin.lekar.Jmbg,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.termin.DatumIVremeTermina + "" +
                " je pomeren na " + noviTermin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            SkladisteZaObavestenja.GetInstance().Save(obavestenjeLekar);

            SkladisteZaTermine.getInstance().Save(noviTermin);

            terminiPrikaz.ItemsSource = SkladisteZaTermine.getInstance().GetBuduciTerminPacLekar();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
