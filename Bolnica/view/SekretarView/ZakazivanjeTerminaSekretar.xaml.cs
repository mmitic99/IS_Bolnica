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
        private LekarKontroler lekarKontroler;

        private List<Termin> moguciTermini = new List<Termin>();

        public ZakazivanjeTerminaSekretar(DataGrid terminiPrikaz, Pacijent izabraniPacijent, bool hitan = false)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            prostorijeKontroler = new ProstorijeKontroler();
            terminKontroler = new TerminKontroler();
            specijalizacijaKontroler = new SpecijalizacijaKontroler();
            lekarKontroler = new LekarKontroler();

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
            if (!pacijent.Text.Equals("") && vremeT.SelectedIndex != -1)
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

                string vremeTermina = ((string)vremeT.SelectedItem).Substring(0, 5);

                string[] vreme = vremeTermina.Split(':');
                int sati = int.Parse(vreme[0]);
                int minuti = int.Parse(vreme[1]);

                DateTime datumIVreme = new DateTime(selDate.Year, selDate.Month, selDate.Day, sati, minuti, 0);

                termin.DatumIVremeTermina = datumIVreme;
                termin.IdProstorije = ((Prostorija)sala.SelectedItem).IdProstorije;

                termin.opisTegobe = tegobe.Text;
                termin.IDTermina = termin.generateRandId();
                termin.TrajanjeTermina = 30;

                if (moguciTermini[vremeT.SelectedIndex].JmbgPacijenta != null)
                {
                    Termin noviTermin = terminKontroler.GetById(moguciTermini[vremeT.SelectedIndex].IDTermina);
                    if (termin.JmbgLekara.Equals(noviTermin.JmbgLekara))
                    {
                        while (terminKontroler.GetTerminZaDatumILekara(noviTermin.DatumIVremeTermina, noviTermin.JmbgLekara) != null)
                        {
                            noviTermin.DatumIVremeTermina = noviTermin.DatumIVremeTermina.AddDays(1);
                        }
                        terminKontroler.IzmeniTermin(noviTermin);
                    }
                }

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

            if (hitanT.IsChecked == true)
            {
                if (vremeT != null && !pacijent.Text.Equals("") && vrstaSpec.SelectedIndex != -1)
                {
                    moguciTermini = TerminKontroler.getInstance().NadjiHitanTermin(termin.JmbgPacijenta, ((Specijalizacija)vrstaSpec.SelectedItem).VrstaSpecijalizacije);
                    vremeT.ItemsSource = GenerisiVremena(moguciTermini);
                }
            }
            else
            {
                if (vremeT != null)
                {
                    if (!pacijent.Text.Equals("") || sender.Equals(pacijent))
                    {
                        if (!lekar.Text.Equals("") || sender.Equals(lekar))
                        {

                            TimeSpan pocetak = new TimeSpan(6, 0, 0);
                            TimeSpan kraj = new TimeSpan(23, 59, 59);

                            if (datum.SelectedDate.Value.Month == DateTime.Now.Month && datum.SelectedDate.Value.Day == DateTime.Now.Day)
                            {
                                pocetak = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                            }

                            moguciTermini = terminKontroler.NadjiTermineZaParametre(termin.JmbgLekara, termin.JmbgPacijenta, new List<DateTime> { (DateTime)datum.SelectedDate }, pocetak, kraj, 1, "", null, true);
                            vremeT.ItemsSource = GenerisiVremena(moguciTermini);
                        }
                    }
                }
            }
        }

        private List<string> GenerisiVremena(List<Termin> moguciTermini)
        {
            List<String> vremena = new List<String>();
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

                    string prikazVremena = sati + ":" + minuti;

                    if (hitanT.IsChecked == true)
                    {
                        Lekar lekarUTerminu = lekarKontroler.GetByJmbg(termin.JmbgLekara);
                        prikazVremena += " lekar: " + lekarUTerminu.Ime + " " + lekarUTerminu.Prezime;
                        prikazVremena += " sala: " + termin.brojSobe;
                    }

                    vremena.Add(prikazVremena);
                }
            }

            return vremena;
        }

        private void vremeT_GotFocus(object sender, RoutedEventArgs e)
        {

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
                termin.lekar = null;
            }
            lekar.Text = "";
            vrstaSpec.SelectedIndex = -1;
            datum.IsEnabled = false;
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
                termin.lekar = null;
            }
            lekar.Text = "";
            vrstaSpec.SelectedIndex = -1;
            datum.IsEnabled = true;
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
