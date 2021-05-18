using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Bolnica.viewActions;
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Termini
{
    /// <summary>
    /// Interaction logic for IzmenaTermina.xaml
    /// </summary>
    public partial class IzmenaTermina : Window
    {
        private TerminPacijentLekarDTO termin;
        private DataGrid terminiPrikaz;

        private TerminKontroler terminKontroler;
        private ProstorijeKontroler prostorijeKontroler;

        public IzmenaTermina(DataGrid terminiPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            terminKontroler = new TerminKontroler();
            prostorijeKontroler = new ProstorijeKontroler();

            this.terminiPrikaz = terminiPrikaz;
            this.termin = (TerminPacijentLekarDTO)terminiPrikaz.SelectedItem;
            vrstaT.SelectedIndex = termin.termin.VrstaTermina == Model.Enum.VrstaPregleda.Pregled ? 0 : 1;
            pacijent.Text = termin.pacijent.Ime + " " + termin.pacijent.Prezime;
            lekar.Text = termin.lekar.Ime + " " + termin.lekar.Prezime;

            datum.SelectedDate = termin.termin.DatumIVremeTermina;
            datum.DisplayDateStart = DateTime.Now;
            String sati = termin.termin.DatumIVremeTermina.Hour.ToString();
            String minuti = termin.termin.DatumIVremeTermina.Minute.ToString();
            string vreme = ParsirajVreme(sati, minuti);


            TimeSpan pocetak = new TimeSpan(6, 0, 0);
            TimeSpan kraj = new TimeSpan(23, 59, 59);

            if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
            {
                pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
            {
                JmbgPacijenta = termin.termin.JmbgPacijenta,
                JmbgLekara = termin.termin.JmbgLekara,
                IzabraniDani = new List<DateTime> { (DateTime)datum.SelectedDate },
                Pocetak = pocetak,
                Kraj = kraj,
                Prioritet = 1,
                tegobe = "",
                PrethodnoZakazaniTermin = null,
                trajanjeUMinutama = 30,
                sekretar = true
            };

            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametri);
            List<string> vremena = GenerisiVremena(moguciTermini);

            if (!vremena.Contains(vreme = ParsirajVreme(sati, minuti)))
            {
                vremena.Add(vreme = ParsirajVreme(sati, minuti));
            }
            vremena.Sort();
            vremeT.ItemsSource = vremena;
            vremeT.SelectedItem = vreme = ParsirajVreme(sati, minuti);

            sala.SelectedIndex = 0;
            sala.ItemsSource = prostorijeKontroler.GetAll();
            for (int i = 0; i < sala.Items.Count; i++)
            {
                if (((Prostorija)sala.Items[i]).BrojSobe.Equals(termin.termin.IdProstorije.ToString()))
                {
                    sala.SelectedIndex = i;
                }
            }
            tegobe.Text = termin.termin.opisTegobe;

        }

        private string ParsirajVreme(string sati, string minuti)
        {
            String vreme = GenerisiSate(sati);
            vreme += GenerisiMinute(minuti);
            return vreme;
        }

        private string GenerisiMinute(string minuti)
        {
            string vreme;
            if (termin.termin.DatumIVremeTermina.Minute >= 0 && termin.termin.DatumIVremeTermina.Minute <= 9)
            {
                vreme = "0" + minuti;
            }
            else
            {
                vreme = minuti;
            }

            return vreme;
        }

        private string GenerisiSate(string sati)
        {
            string vreme;
            if (termin.termin.DatumIVremeTermina.Hour >= 0 && termin.termin.DatumIVremeTermina.Hour <= 9)
            {
                vreme = "0" + sati + ":";
            }
            else
            {
                vreme = sati + ":";
            }

            return vreme;
        }

        private List<string> GenerisiVremena(List<Termin> moguciTermini)
        {
            List<String> vremena = new List<String>();
            foreach (Termin termin in moguciTermini)
            {
                if (datum.SelectedDate.Value.Month == termin.DatumIVremeTermina.Month && datum.SelectedDate.Value.Day == termin.DatumIVremeTermina.Day)
                {

                    String noviSati = termin.DatumIVremeTermina.Hour.ToString();
                    String noviMinuti = termin.DatumIVremeTermina.Minute.ToString();

                    if (int.Parse(noviSati) >= 0 && int.Parse(noviSati) <= 9)
                    {
                        noviSati = "0" + noviSati;
                    }

                    if (int.Parse(noviMinuti) >= 0 && int.Parse(noviMinuti) <= 9)
                    {
                        noviMinuti = "0" + noviMinuti;
                    }

                    vremena.Add(noviSati + ":" + noviMinuti);
                }
            }

            return vremena;
        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (vremeT.SelectedIndex == -1)
            {
                MessageBox.Show("Morate vreme termina.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Termin noviTermin = new Termin
            {
                IdProstorije = int.Parse(((Prostorija) sala.SelectedItem).BrojSobe),
                opisTegobe = tegobe.Text,
                JmbgPacijenta = termin.pacijent.Jmbg,
                JmbgLekara = termin.lekar.Jmbg,
                VrstaTermina = termin.termin.VrstaTermina
            };

            DateTime selDate = (DateTime) datum.SelectedDate;

            string[] vreme = ((string) vremeT.SelectedItem).Split(':');
            int sati = int.Parse(vreme[0]);
            int minuti = int.Parse(vreme[1]);

            DateTime datumIVreme = new DateTime(selDate.Year, selDate.Month, selDate.Day, sati, minuti, 0);

            noviTermin.DatumIVremeTermina = datumIVreme;

            noviTermin.IDTermina = termin.termin.IDTermina;

            terminKontroler.IzmeniTermin(noviTermin, noviTermin);

            terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
            SekretarWindow.SortirajDataGrid(terminiPrikaz, 0, ListSortDirection.Ascending);

            this.Close();
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AzurirajVreme(object sender, EventArgs e)
        {
            if (vremeT == null) return;
            if (hitanT.IsChecked == true && vremeT.Text.Equals(""))
            {

                TimeSpan pocetak = new TimeSpan(6, 0, 0);
                TimeSpan kraj = new TimeSpan(23, 59, 59);

                if (datum.SelectedDate == DateTime.Now)
                {
                    pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                }

                ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
                {
                    JmbgPacijenta = termin.termin.JmbgPacijenta,
                    JmbgLekara = termin.termin.JmbgLekara,
                    IzabraniDani = new List<DateTime> { (DateTime)datum.SelectedDate },
                    Pocetak = pocetak,
                    Kraj = kraj,
                    Prioritet = 0,
                    tegobe = "",
                    PrethodnoZakazaniTermin = null,
                    trajanjeUMinutama = 30,
                    sekretar = true
                };

                List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametri);


                List<String> vremena = GenerisiVremena(moguciTermini);
                vremeT.ItemsSource = vremena;
            }
            else
            {

                if ((!pacijent.Text.Equals("") || sender.Equals(pacijent)) && (!lekar.Text.Equals("") || sender.Equals(lekar)))
                {
                    TimeSpan pocetak = new TimeSpan(6, 0, 0);
                    TimeSpan kraj = new TimeSpan(23, 59, 59);

                    if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
                    {
                        pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    }
                    ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
                    {
                        JmbgPacijenta = termin.termin.JmbgPacijenta,
                        JmbgLekara = termin.termin.JmbgLekara,
                        IzabraniDani = new List<DateTime> { (DateTime)datum.SelectedDate },
                        Pocetak = pocetak,
                        Kraj = kraj,
                        Prioritet = 1,
                        tegobe = "",
                        PrethodnoZakazaniTermin = null,
                        trajanjeUMinutama = 30,
                        sekretar = true
                    };
                    List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametri);
                    List<string> vremena = GenerisiVremena(moguciTermini);
                    vremeT.ItemsSource = vremena;
                }
            }
        }
    }
}
