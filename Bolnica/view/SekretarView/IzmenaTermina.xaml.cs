using Bolnica.viewActions;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for IzmenaTermina.xaml
    /// </summary>
    public partial class IzmenaTermina : Window
    {
        private TerminPacijentLekar termin;
        private DataGrid terminiPrikaz;

        private TerminKontroler terminKontroler;
        private ProstorijeKontroler prostorijeKontroler;
        private ObavestenjaKontroler obavestenjaKontroler;

        public IzmenaTermina(DataGrid terminiPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            terminKontroler = new TerminKontroler();
            prostorijeKontroler = new ProstorijeKontroler();
            obavestenjaKontroler = new ObavestenjaKontroler();

            this.terminiPrikaz = terminiPrikaz;
            this.termin = (TerminPacijentLekar)terminiPrikaz.SelectedItem;
            if (termin.termin.VrstaTermina == Model.Enum.VrstaPregleda.Pregled)
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
            datum.DisplayDateStart = DateTime.Now;
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

            List<String> vremena = new List<String>();

            TimeSpan pocetak = new TimeSpan(6, 0, 0);
            TimeSpan kraj = new TimeSpan(23, 59, 59);

            if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
            {
                pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(termin.termin.JmbgLekara, termin.termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 1, "");


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


            if (!vremena.Contains(vreme))
            {
                vremena.Add(vreme);
            }
            vremena.Sort();
            vremeT.ItemsSource = vremena;
            vremeT.SelectedItem = vreme;

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

            noviTermin.IDTermina = termin.termin.IDTermina;

            terminKontroler.IzmeniTermin(noviTermin);

            terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void azurirajVreme(object sender, EventArgs e)
        {
            if (vremeT != null)
            {
                if (hitanT.IsChecked == true && vremeT.Text.Equals(""))
                {
                    List<String> vremena = new List<String>();

                    TimeSpan pocetak = new TimeSpan(6, 0, 0);
                    TimeSpan kraj = new TimeSpan(23, 59, 59);

                    if (datum.SelectedDate == DateTime.Now)
                    {
                        pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    }

                    List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(termin.termin.JmbgLekara, termin.termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 0, "");


                    foreach (Termin termin in moguciTermini)
                    {
                        String sati = termin.DatumIVremeTermina.Hour.ToString();
                        String minuti = termin.DatumIVremeTermina.Minute.ToString();

                        if (int.Parse(sati) >= 0 && int.Parse(sati) <= 9)
                        {
                            sati = "0" + sati;
                        }

                        if (int.Parse(minuti) >= 0 && int.Parse(minuti) <= 9)
                        {
                            minuti = "0" + minuti;
                        }

                        vremena.Add(sati + ":" + minuti);
                    }
                    vremeT.ItemsSource = vremena;
                }
                else
                {

                    if (!pacijent.Text.Equals("") || sender.Equals(pacijent))
                    {
                        if (!lekar.Text.Equals("") || sender.Equals(lekar))
                        {
                            List<String> vremena = new List<String>();

                            TimeSpan pocetak = new TimeSpan(6, 0, 0);
                            TimeSpan kraj = new TimeSpan(23, 59, 59);

                            if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
                            {
                                pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                            }

                            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(termin.termin.JmbgLekara, termin.termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 1, "");


                            foreach (Termin termin in moguciTermini)
                            {
                                if (datum.SelectedDate.Value.Month == termin.DatumIVremeTermina.Month && datum.SelectedDate.Value.Day == termin.DatumIVremeTermina.Day)
                                {

                                    String sati = termin.DatumIVremeTermina.Hour.ToString();
                                    String minuti = termin.DatumIVremeTermina.Minute.ToString();

                                    if (int.Parse(sati) >= 0 && int.Parse(sati) <= 9)
                                    {
                                        sati = "0" + sati;
                                    }

                                    if (int.Parse(minuti) >= 0 && int.Parse(minuti) <= 9)
                                    {
                                        minuti = "0" + minuti;
                                    }

                                    vremena.Add(sati + ":" + minuti);
                                }
                            }
                            vremeT.ItemsSource = vremena;
                        }
                        else
                        {

                        }


                    }
                }
            }
        }
    }
}
