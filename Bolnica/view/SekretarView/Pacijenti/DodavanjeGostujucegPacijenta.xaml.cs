using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Bolnica.view.SekretarView.Termini;
using Kontroler;

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
        private DatePicker datumZaTermin;
        public DodavanjeGostujucegPacijenta(DataGrid pacijentiPrikaz, DataGrid terminiPrikaz, DatePicker datumZaTermin)
        {
            InitializeComponent();
            this.pacijentiPrikaz = pacijentiPrikaz;
            this.terminiPrikaz = terminiPrikaz;
            pacijentKontroler = new PacijentKontroler();
            this.Owner = App.Current.MainWindow;
            DataContext = new PacijentDTO();
            this.datumZaTermin = datumZaTermin;
        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            PacijentDTO pacijent = new PacijentDTO
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
                NazivGrada =  "" ,
                Korisnik = new KorisnikDTO()
                {
                    KorisnickoIme = jmbg.Text,
                    Lozinka = ime.Text
                },
                ZdravstveniKarton = new ZdravstveniKartonDTO(),
                BracnoStanje = "",
                Zanimanje = ""
            };

            if (!pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals("") && !pacijent.Prezime.Trim().Equals(""))
            {
                bool uspesno = pacijentKontroler.RegistrujPacijenta(pacijent);
                if (!uspesno)
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    jmbg.Focus();
                    return;
                }

                pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                SekretarWindow.SortirajDataGrid(pacijentiPrikaz, 0, ListSortDirection.Ascending);
                System.Windows.Forms.DialogResult dialogResult =
                    System.Windows.Forms.MessageBox.Show("Da li želite da zakažete hitan termin za ovog pacijenta?",
                        "", System.Windows.Forms.MessageBoxButtons.YesNo);

                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    var s = new ZakazivanjeTerminaSekretar(pacijent, true, null, datumZaTermin);
                    s.Show();
                }

                this.Close();
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
