using Kontroler;
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
    /// Interaction logic for DodavanjeGostujucegPacijenta.xaml
    /// </summary>
    public partial class DodavanjeGostujucegPacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private PacijentKontroler pacijentKontroler;
        public DodavanjeGostujucegPacijenta(DataGrid pacijentiPrikaz)
        {
            InitializeComponent();
            this.pacijentiPrikaz = pacijentiPrikaz;
            pacijentKontroler = new PacijentKontroler();
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
                },
                zdravstveniKarton = new ZdravstveniKarton()

            };

            if (!pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals("") && !pacijent.Prezime.Trim().Equals(""))
            {
                bool uspesno = pacijentKontroler.RegistrujPacijenta(pacijent);
                if (uspesno)
                {
                    pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
