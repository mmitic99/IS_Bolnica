using Bolnica.DTOs;
using Kontroler;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for IzmenaProfila.xaml
    /// </summary>
    public partial class IzmenaProfila : Window
    {
        private SekretarDTO sekretar;
        private Label imeS;
        private Label prezimeS;
        private SekretarKontroler sekretarKontroler;
        private string stariJmbg;

        public IzmenaProfila(SekretarDTO sekretar, Label imeS, Label prezimeS)
        {
            InitializeComponent();
            this.sekretar = sekretar;
            this.imeS = imeS;
            this.prezimeS = prezimeS;
            this.Owner = App.Current.MainWindow;
            sekretarKontroler = new SekretarKontroler();

            stariJmbg = sekretar.Jmbg;

            jmbg.Text = sekretar.Jmbg;
            ime.Text = sekretar.Ime;
            prezime.Text = sekretar.Prezime;
            adresa.Text = sekretar.Adresa;
            tel.Text = sekretar.BrojTelefona;
            email.Text = sekretar.Email;
            grad.Text = sekretar.NazivGrada;
            korIme.Text = sekretar.Korisnik.KorisnickoIme;
            datum.SelectedDate = sekretar.DatumRodjenja.Date;

            pol.SelectedIndex = sekretar.Pol == Model.Enum.Pol.Muski ? 0 : 1;

            DataContext = sekretar;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            SekretarDTO noviSekretar = new SekretarDTO()
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                NazivGrada = grad.Text,
                Korisnik = new KorisnikDTO() { KorisnickoIme = korIme.Text, Lozinka = sekretar.Korisnik.Lozinka },
                Pol = pol.SelectedIndex == 0 ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski,
                BrojSlobodnihDana = sekretar.BrojSlobodnihDana

            };
            noviSekretar.DatumRodjenja = datum.SelectedDate != null ? (DateTime)datum.SelectedDate : DateTime.Now;


            if (!noviSekretar.Jmbg.Trim().Equals("") && !noviSekretar.Ime.Trim().Equals("") && !noviSekretar.Prezime.Trim().Equals(""))
            {
                bool uspesno = sekretarKontroler.IzmeniSekretara(stariJmbg, noviSekretar);
                if (!uspesno)
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    jmbg.Focus();
                    return;
                }

                sekretar = noviSekretar;
                imeS.Content = noviSekretar.Ime;
                prezimeS.Content = noviSekretar.Prezime;
                this.Close();
            }
            else if (noviSekretar.Jmbg.Trim().Equals("") || noviSekretar.Ime.Trim().Equals("") || noviSekretar.Prezime.Trim().Equals(""))
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
