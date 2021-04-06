using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for DodavanjeGostujucegPacijenta.xaml
    /// </summary>
    public partial class DodavanjeGostujucegPacijenta : Window
    {
        public DodavanjeGostujucegPacijenta(DataGrid pacijentiPrikaz)
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
                }
            };

            bool greska = false;
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();
            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    greska = true;
                }
            }
            if (!greska && !pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals("") && !pacijent.Prezime.Trim().Equals(""))
            {
                SkladistePacijenta.GetInstance().Save(pacijent);
                this.Close();
            }
            else if (pacijent.Jmbg.Trim().Equals("") || pacijent.Ime.Trim().Equals("") || pacijent.Prezime.Trim().Equals(""))
            {
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);

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
