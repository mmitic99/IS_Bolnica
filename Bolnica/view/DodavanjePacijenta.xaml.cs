using Model;
using Model.Skladista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for DodavanjePacijenta.xaml
    /// </summary>
    public partial class DodavanjePacijenta : Window
    {
        public DodavanjePacijenta()
        {
            InitializeComponent();
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
                Grad = new Grad { Naziv = grad.Text },
                Korisnik = new Korisnik
                {
                    KorisnickoIme = korIme.Text,
                    Lozinka = lozinka.Password
                }
            };

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
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    greska = true;
                }
            }
            if (!greska)
            {
                SkladistePacijenta.GetInstance().Save(pacijent);
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
