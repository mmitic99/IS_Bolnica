using Model;
using Model.Skladista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for IzmenaPacijenta.xaml
    /// </summary>
    public partial class IzmenaPacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private Pacijent pacijent;
        public IzmenaPacijenta(DataGrid pacijentiPrikaz)
        {
            InitializeComponent();
            this.pacijentiPrikaz = pacijentiPrikaz;
            this.pacijent = (Pacijent)pacijentiPrikaz.SelectedItem;
            jmbg.Text = pacijent.Jmbg;
            ime.Text = pacijent.Ime;
            prezime.Text = pacijent.Prezime;
            adresa.Text = pacijent.Adresa;
            tel.Text = pacijent.BrojTelefona;
            email.Text = pacijent.Email;
            grad.Text = pacijent.Grad.Naziv;
            korIme.Text = pacijent.Korisnik.KorisnickoIme;
            lozinka.Password = pacijent.Korisnik.Lozinka;
            datum.SelectedDate = pacijent.DatumRodjenja.Date;
            if (pacijent.Pol == Model.Enum.Pol.Muski)
            {
                pol.SelectedIndex = 0;
            }
            else
            {
                pol.SelectedIndex = 1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pacijent noviPacijent = new Pacijent
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                Grad = new Grad { Naziv = grad.Text },
                Korisnik = new Korisnik
                {
                    KorisnickoIme = korIme.Text,
                    Lozinka = lozinka.Password
                }
            };

            if (pol.SelectedIndex == 0)
            {
                noviPacijent.Pol = Model.Enum.Pol.Muski;
            }
            else
            {
                noviPacijent.Pol = Model.Enum.Pol.Zenski;
            }
            if (datum.SelectedDate != null)
            {
                noviPacijent.DatumRodjenja = (DateTime)datum.SelectedDate;
            }
            else
            {
                noviPacijent.DatumRodjenja = DateTime.Now;
            }

            bool greska = false;
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();
            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    pacijenti.RemoveAt(i);
                }
            }
            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(noviPacijent.Jmbg))
                {
                    greska = true;
                }
            }

            if (!greska)
            {
                pacijenti.Add(noviPacijent);
                SkladistePacijenta.GetInstance().SaveAll(pacijenti);
                pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
                this.Close();
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
    }
}
