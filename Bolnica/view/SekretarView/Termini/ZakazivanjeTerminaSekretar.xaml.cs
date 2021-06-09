using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.view.SekretarView.Termini
{
    /// <summary>
    /// Interaction logic for ZakazivanjeTerminaSekretar.xaml
    /// </summary>
    public partial class ZakazivanjeTerminaSekretar : Window
    {
        private TerminDTO termin;
        private ProstorijeKontroler prostorijeKontroler;
        private TerminKontroler terminKontroler;
        private SpecijalizacijaKontroler specijalizacijaKontroler;
        private LekarKontroler lekarKontroler;
        private DatePicker datumZaTermin;

        private List<TerminDTO> moguciTermini = new List<TerminDTO>();

        public ZakazivanjeTerminaSekretar(PacijentDTO izabraniPacijent, bool hitan = false, LekarDTO izabraniLekar = null, DatePicker datumZaTermin = null)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            prostorijeKontroler = new ProstorijeKontroler();
            terminKontroler = new TerminKontroler();
            specijalizacijaKontroler = new SpecijalizacijaKontroler();
            lekarKontroler = new LekarKontroler();

            datum.SelectedDate = DateTime.Now.AddDays(1);
            termin = new TerminDTO();
            if (izabraniPacijent != null)
            {
                termin.JmbgPacijenta = izabraniPacijent.Jmbg;
                pacijent.Text = izabraniPacijent.Ime + " " + izabraniPacijent.Prezime;
            }

            if (izabraniLekar != null)
            {
                termin.JmbgLekara = izabraniLekar.Jmbg;
                lekar.Text = izabraniLekar.Ime + " " + izabraniLekar.Prezime;
            }

            sala.ItemsSource = prostorijeKontroler.GetAll();
            sala.SelectedIndex = 0;

            datum.DisplayDateStart = DateTime.Now;

            vrstaSpec.ItemsSource = specijalizacijaKontroler.GetAll();
            this.datumZaTermin = datumZaTermin;
            hitanT.IsChecked = hitan;

        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (pacijent.Text.Equals("") || vremeT.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati pacijenta i vreme termina.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            termin.VrstaTermina = vrstaT.SelectedIndex == 0 ? Model.Enum.VrstaPregleda.Pregled : Model.Enum.VrstaPregleda.Operacija;
            termin.IdProstorije = int.Parse(((ProstorijaDTO) sala.SelectedItem).BrojSobe);
            DateTime selDate = (DateTime) datum.SelectedDate;

            string vremeTermina = ((string) vremeT.SelectedItem).Substring(0, 5);

            string[] vreme = vremeTermina.Split(':');
            int sati = int.Parse(vreme[0]);
            int minuti = int.Parse(vreme[1]);

            DateTime datumIVreme = new DateTime(selDate.Year, selDate.Month, selDate.Day, sati, minuti, 0);

            termin.DatumIVremeTermina = datumIVreme;
            termin.IdProstorije = ((ProstorijaDTO) sala.SelectedItem).IdProstorije;

            termin.opisTegobe = tegobe.Text;
            termin.TrajanjeTermina = 30;

            if (moguciTermini[vremeT.SelectedIndex].JmbgPacijenta != null)
            {
                TerminDTO noviTermin = terminKontroler.GetById(moguciTermini[vremeT.SelectedIndex].IDTermina);
                if (termin.JmbgLekara.Equals(noviTermin.JmbgLekara))
                {
                    PronadjiNovoVremeTermina(noviTermin);
                    terminKontroler.IzmeniTermin(noviTermin);
                }
            }
            terminKontroler.ZakaziTermin(termin);
            datumZaTermin.SelectedDate = datumIVreme;
            this.Close();
        }

        private void PronadjiNovoVremeTermina(TerminDTO noviTermin)
        {
            while (terminKontroler.GetTerminZaDatumILekara(noviTermin.DatumIVremeTermina, noviTermin.JmbgLekara) != null)
            {
                noviTermin.DatumIVremeTermina = noviTermin.DatumIVremeTermina.AddDays(1);
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dodajPacijentaT_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijentaTerminu(termin, pacijent);
            s.ShowDialog();
        }

        private void dodajLekaraT_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeLekaraTerminu(termin, lekar);
            s.ShowDialog();
        }

        private void AzurirajVreme(object sender, EventArgs e)
        {

            if (hitanT.IsChecked == true)
            {
                if (vremeT != null && !pacijent.Text.Equals("") && vrstaSpec.SelectedIndex != -1)
                {
                    vremeT.Visibility = Visibility.Visible;
                    NeRadi.Visibility = Visibility.Hidden;
                    moguciTermini = TerminKontroler.getInstance().NadjiHitanTermin(termin.JmbgPacijenta, (vrstaSpec.SelectedItem).ToString());
                    vremeT.ItemsSource = GenerisiVremena(moguciTermini); 
                    if (vremeT.Items.Count == 0)
                    {
                        NeRadi.Content = "Ni jedan lekar ne radi!";
                        vremeT.Visibility = Visibility.Hidden;
                        NeRadi.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        vremeT.Visibility = Visibility.Visible;
                        NeRadi.Visibility = Visibility.Hidden;
                    }
                }
                return;
            }

            if (vremeT != null && (!pacijent.Text.Equals("") || sender.Equals(pacijent)) && (!lekar.Text.Equals("") || sender.Equals(lekar)))
            {
                vremeT.Visibility = Visibility.Visible;
                NeRadi.Visibility = Visibility.Hidden;
                TimeSpan pocetak = new TimeSpan(6, 0, 0);
                TimeSpan kraj = new TimeSpan(23, 59, 59);

                if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
                {
                    pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                }
                ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
                {
                    JmbgPacijenta = termin.JmbgPacijenta,
                    JmbgLekara = termin.JmbgLekara,
                    IzabraniDani = new List<DateTime> { (DateTime)datum.SelectedDate },
                    Pocetak = pocetak,
                    Kraj = kraj,
                    Prioritet = 1,
                    tegobe = "",
                    PrethodnoZakazaniTermin = null,
                    trajanjeUMinutama = 30,
                    sekretar = true
                };
                moguciTermini = terminKontroler.NadjiTermineZaParametre(parametri);
                vremeT.ItemsSource = GenerisiVremena(moguciTermini);
                if (vremeT.Items.Count == 0)
                {
                    NeRadi.Content = "Lekar ne radi!";
                    vremeT.Visibility = Visibility.Hidden;
                    NeRadi.Visibility = Visibility.Visible;
                }
                else
                {
                    vremeT.Visibility = Visibility.Visible;
                    NeRadi.Visibility = Visibility.Hidden;
                }
            }
        }

        private List<string> GenerisiVremena(List<TerminDTO> termini)
        {
            List<String> vremena = new List<String>();
            foreach (TerminDTO termin in termini)
            {
                if (datum.SelectedDate.Value.Month == termin.DatumIVremeTermina.Month && datum.SelectedDate.Value.Day == termin.DatumIVremeTermina.Day)
                {
                    var prikazVremena = ParsirajVreme(termin.DatumIVremeTermina.Hour.ToString(), termin.DatumIVremeTermina.Minute.ToString());
                    if (hitanT.IsChecked == true)
                    {
                        prikazVremena = AzurirajPrikazVremenaZaHitanTermin(termin, prikazVremena);
                    }
                    vremena.Add(prikazVremena);
                }
            }
            return vremena;
        }

        private string AzurirajPrikazVremenaZaHitanTermin(TerminDTO hitanTermin, string prikazVremena)
        {
            LekarDTO lekarUTerminu = lekarKontroler.GetByJmbg(hitanTermin.JmbgLekara);
            prikazVremena += " lekar: " + lekarUTerminu.Ime + " " + lekarUTerminu.Prezime;
            prikazVremena += " sala: " + hitanTermin.brojSobe;
            return prikazVremena;
        }

        private static string ParsirajVreme(string sati, string minuti)
        {
            sati = GenerisiSate(sati);
            minuti = GenerisiMinute(minuti);
            return sati + ":" + minuti;
        }

        private static string GenerisiMinute(string minuti)
        {
            if (int.Parse(minuti) >= 0 && int.Parse(minuti) <= 9)
            {
                minuti = "0" + minuti;
            }
            return minuti;
        }

        private static string GenerisiSate(string sati)
        {
            if (int.Parse(sati) >= 0 && int.Parse(sati) <= 9)
            {
                sati = "0" + sati;
            }
            return sati;
        }

        private void hitanT_Checked(object sender, RoutedEventArgs e)
        {
            lekarLabela.Visibility = Visibility.Hidden;
            lekar.Visibility = Visibility.Hidden;
            lekarButton.Visibility = Visibility.Hidden;
            sala.Visibility = Visibility.Hidden;
            salaText.Visibility = Visibility.Visible;
            vrstaSpec.Visibility = Visibility.Visible;
            vrstaSpecLabela.Visibility = Visibility.Visible;
            if (termin != null)
            {
                termin.JmbgLekara = null;
            }
            lekar.Text = "";
            vrstaSpec.SelectedIndex = -1;
            datum.IsEnabled = false;
            datum.SelectedDate = DateTime.Now;
            vremeT.ItemsSource = null;
        }

        private void hitanT_Unchecked(object sender, RoutedEventArgs e)
        {
            lekarLabela.Visibility = Visibility.Visible;
            lekar.Visibility = Visibility.Visible;
            lekarButton.Visibility = Visibility.Visible;
            sala.Visibility = Visibility.Visible;
            salaText.Visibility = Visibility.Hidden;
            vrstaSpec.Visibility = Visibility.Hidden;
            vrstaSpecLabela.Visibility = Visibility.Hidden;
            if (termin != null)
            {
                termin.JmbgLekara = null;
            }
            lekar.Text = "";
            vrstaSpec.SelectedIndex = -1;
            datum.IsEnabled = true;
            datum.DisplayDateStart = DateTime.Now.AddDays(1);
            datum.SelectedDate = DateTime.Now.AddDays(1);
            vremeT.ItemsSource = null;
        }

        private void vremeT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vrstaSpec != null && vremeT.SelectedIndex != -1 && moguciTermini.Count > 0)
            {
                termin.JmbgLekara = moguciTermini[vremeT.SelectedIndex].JmbgLekara;
                termin.brojSobe = moguciTermini[vremeT.SelectedIndex].brojSobe;
            }
        }
    }
}
