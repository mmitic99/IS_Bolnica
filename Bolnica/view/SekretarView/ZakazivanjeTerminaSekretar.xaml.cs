using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for ZakazivanjeTerminaSekretar.xaml
    /// </summary>
    public partial class ZakazivanjeTerminaSekretar : Window
    {
        private Termin termin;
        private DataGrid terminiPrikaz;
        private ProstorijeKontroler prostorijeKontroler;
        private TerminKontroler terminKontroler;
        private SpecijalizacijaKontroler specijalizacijaKontroler;

        public ZakazivanjeTerminaSekretar(DataGrid terminiPrikaz, Pacijent izabraniPacijent, bool hitan)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            prostorijeKontroler = new ProstorijeKontroler();
            terminKontroler = new TerminKontroler();
            specijalizacijaKontroler = new SpecijalizacijaKontroler();

            hitanT.IsChecked = hitan;
            this.terminiPrikaz = terminiPrikaz;
            datum.SelectedDate = DateTime.Now;
            termin = new Termin();
            if (izabraniPacijent != null)
            {
                termin.JmbgPacijenta = izabraniPacijent.Jmbg;
                pacijent.Text = izabraniPacijent.Ime + " " + izabraniPacijent.Prezime;
            }

            sala.ItemsSource = prostorijeKontroler.GetAll();
            sala.SelectedIndex = 0;
           
            datum.DisplayDateStart = DateTime.Now;

            vrstaSpec.ItemsSource = specijalizacijaKontroler.GetAll();

        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (!pacijent.Text.Equals("") && vremeT.SelectedIndex == -1)
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
                termin.IdProstorije = ((Prostorija)sala.SelectedItem).IdProstorije;

                termin.opisTegobe = tegobe.Text;
                termin.IDTermina = termin.generateRandId();
                termin.TrajanjeTermina = 30;

                terminKontroler.ZakaziTermin(termin);

                terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();

                this.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati pacijenta i vreme termina.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void azurirajVreme(object sender, EventArgs e)
        {
            if (vremeT != null)
            {
                if (hitanT.IsChecked == true && vremeT.Text.Equals("") && termin != null)
                {
                    /*List<String> vremena = new List<String>();

                    TimeSpan pocetak = new TimeSpan(6, 0, 0);
                    TimeSpan kraj = new TimeSpan(23, 59, 59);

                    if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
                    {
                        pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    }

                    List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(termin.JmbgLekara, termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 0, "");


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
                    vremeT.ItemsSource = vremena;*/
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

                            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(termin.JmbgLekara, termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 1, "");


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

        private void vremeT_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void hitanT_Checked(object sender, RoutedEventArgs e)
        {
            lekarLabela.Visibility = Visibility.Hidden;
            lekar.Visibility = Visibility.Hidden;
            lekarButton.Visibility = Visibility.Hidden;
            vrstaSpec.Visibility = Visibility.Visible;
            vrstaSpecLabela.Visibility = Visibility.Visible;
        }

        private void hitanT_Unchecked(object sender, RoutedEventArgs e)
        {
            lekarLabela.Visibility = Visibility.Visible;
            lekar.Visibility = Visibility.Visible;
            lekarButton.Visibility = Visibility.Visible;
            vrstaSpec.Visibility = Visibility.Hidden;
            vrstaSpecLabela.Visibility = Visibility.Hidden;
        }
    }
}
