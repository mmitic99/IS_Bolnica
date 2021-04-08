using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for DodavanjePacijenta.xaml
    /// </summary>
    public partial class DodavanjePacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        public DodavanjePacijenta(DataGrid pacijentiPrikaz)
        {
            InitializeComponent();
            this.pacijentiPrikaz = pacijentiPrikaz;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pacijent pacijent = new Pacijent
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                Grad = new Grad { Naziv = grad.Text }
            };


            Korisnik korisnik = new Korisnik();
            if (korIme.Text == "")
            {
                korisnik.KorisnickoIme = pacijent.Jmbg;
            }
            else
            {
                korisnik.KorisnickoIme = korIme.Text;
            }
            if (lozinka.Password == "")
            {
                korisnik.Lozinka = pacijent.Ime;
            }
            else
            {
                korisnik.Lozinka = lozinka.Password;
            }

            pacijent.Korisnik = korisnik;


            if (pol.SelectedIndex == 0)
            {
                pacijent.Pol = Model.Enum.Pol.Muski;
            }
            else
            {
                pacijent.Pol = Model.Enum.Pol.Zenski;
            }
            if (datum.SelectedDate != null)
            {
                pacijent.DatumRodjenja = (DateTime)datum.SelectedDate;
            }
            else
            {
                pacijent.DatumRodjenja = DateTime.Now;
            }
            bool greska = false;
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();
            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg != null)
                {
                    if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                    {
                        greska = true;
                    }
                }
            }
            if (!greska && !pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals(""))
            {
                SkladistePacijenta.GetInstance().Save(pacijent);
                pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
                this.Close();
            }
            else if (pacijent.Jmbg.Trim().Equals("") || pacijent.Ime.Trim().Equals(""))
            {
                MessageBox.Show("Polja JMBG i Ime su obavezna!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
