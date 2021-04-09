using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for IzmenaPacijenta.xaml
    /// </summary>
    public partial class IzmenaPacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private Pacijent pacijent;
        private ObservableCollection<String> alergeni;
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
            if(pacijent.zdravstveniKarton == null)
            {
                pacijent.zdravstveniKarton = new ZdravstveniKarton();
            }
            if (pacijent.Pol == Model.Enum.Pol.Muski)
            {
                pol.SelectedIndex = 0;
            }
            else
            {
                pol.SelectedIndex = 1;
            }
            alergeni = new ObservableCollection<String>(pacijent.zdravstveniKarton.Alergeni);
            alergeniList.ItemsSource = alergeni;
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
                },
                zdravstveniKarton = pacijent.zdravstveniKarton
            };
            noviPacijent.zdravstveniKarton.Alergeni = new List<string>(alergeni);

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

            if (!greska && !noviPacijent.Jmbg.Equals("") && !noviPacijent.Ime.Trim().Equals(""))
            {
                pacijenti.Add(noviPacijent);
                SkladistePacijenta.GetInstance().SaveAll(pacijenti);
                pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
                this.Close();
            }
            else if (noviPacijent.Jmbg.Trim().Equals("") || noviPacijent.Ime.Trim().Equals(""))
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

        private void dodajA_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeAlregena(alergeni);
            s.Show();
        }

        private void obrisiA_Click(object sender, RoutedEventArgs e)
        {
            if(alergeniList.SelectedIndex != -1)
            {
                alergeni.RemoveAt(alergeniList.SelectedIndex);
            }
        }
    }
}
