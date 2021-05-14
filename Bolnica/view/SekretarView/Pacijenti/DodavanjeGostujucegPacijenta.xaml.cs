using System;
using System.Windows;
using System.Windows.Controls;
using Bolnica.view.SekretarView.Termini;
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Pacijenti
{
    /// <summary>
    /// Interaction logic for DodavanjeGostujucegPacijenta.xaml
    /// </summary>
    public partial class DodavanjeGostujucegPacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private DataGrid terminiPrikaz;
        private PacijentKontroler pacijentKontroler;
        public DodavanjeGostujucegPacijenta(DataGrid pacijentiPrikaz, DataGrid terminiPrikaz)
        {
            InitializeComponent();
            this.pacijentiPrikaz = pacijentiPrikaz;
            this.terminiPrikaz = terminiPrikaz;
            pacijentKontroler = new PacijentKontroler();
            this.Owner = App.Current.MainWindow;
        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            Pacijent pacijent = new Pacijent
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Registrovan = false,
                Adresa = "",
                Pol = Model.Enum.Pol.Muski,
                DatumRodjenja = DateTime.Now,
                BrojTelefona = "",
                Email = "",
                Grad = new Grad { Naziv = "" },
                Korisnik = new Korisnik
                {
                    KorisnickoIme = jmbg.Text,
                    Lozinka = ime.Text
                },
                zdravstveniKarton = new ZdravstveniKarton(),
                BracnoStanje = "",
                Zanimanje = ""
            };

            if (!pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals("") && !pacijent.Prezime.Trim().Equals(""))
            {
                bool uspesno = pacijentKontroler.RegistrujPacijenta(pacijent);
                if (uspesno)
                {
                    pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Da li želite da zakažete hitan termin za ovog pacijenta?", "", System.Windows.Forms.MessageBoxButtons.YesNo);
                    
                    if(dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, pacijent, true);
                        s.Show();
                    }
                    
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
                    jmbg.Focus();
                }
            }
            else if (pacijent.Jmbg.Trim().Equals("") || pacijent.Ime.Trim().Equals("") || pacijent.Prezime.Trim().Equals(""))
            {
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
