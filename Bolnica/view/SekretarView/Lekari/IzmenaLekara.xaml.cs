﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bolnica.DTOs;
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for IzmenaLekara.xaml
    /// </summary>
    public partial class IzmenaLekara : Window
    {
        private DataGrid lekariPrikaz;
        private LekarDTO lekar;
        private LekarKontroler lekarKontroler;
        private SpecijalizacijaKontroler specijalizacijaKontroler;
        public IzmenaLekara(DataGrid lekariPrikaz)
        {
            InitializeComponent(); 
            this.Owner = App.Current.MainWindow;
            lekarKontroler = new LekarKontroler();
            specijalizacijaKontroler = new SpecijalizacijaKontroler();

            this.lekariPrikaz = lekariPrikaz;
            this.lekar = lekarKontroler.GetByJmbg(((LekarDTO) lekariPrikaz.SelectedItem).Jmbg);
            jmbg.Text = lekar.Jmbg;
            ime.Text = lekar.Ime;
            prezime.Text = lekar.Prezime;
            adresa.Text = lekar.Adresa;
            tel.Text = lekar.BrojTelefona;
            email.Text = lekar.Email;
            grad.Text = lekar.Grad.Naziv;
            korIme.Text = lekar.Korisnik.KorisnickoIme;
            lozinka.Password = lekar.Korisnik.Lozinka;
            datum.SelectedDate = lekar.DatumRodjenja.Date;

            pol.SelectedIndex = lekar.Pol == Model.Enum.Pol.Muski ? 0 : 1;

            List<Specijalizacija> specijalizacije = specijalizacijaKontroler.GetAll();

            Specijalizacija.ItemsSource = specijalizacije;

            for (int i = 0; i < specijalizacije.Count; i++)
            {
                if (specijalizacije[i].VrstaSpecijalizacije.Equals(lekar.Specijalizacija.VrstaSpecijalizacije))
                {
                    Specijalizacija.SelectedIndex = i;
                    break;
                }
            }


        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            LekarDTO noviLekar = new LekarDTO()
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                Grad = new Grad { Naziv = grad.Text },
                Korisnik = new Korisnik { KorisnickoIme = jmbg.Text, Lozinka = ime.Text },
                Pol = pol.SelectedIndex == 0 ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski,
                Specijalizacija = (Specijalizacija)Specijalizacija.SelectedItem,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana

            };
            if (datum.SelectedDate != null)
            {
                noviLekar.DatumRodjenja = (DateTime)datum.SelectedDate;
            }
            else
            {
                noviLekar.DatumRodjenja = DateTime.Now;
            }


            if (!noviLekar.Jmbg.Trim().Equals("") && !noviLekar.Ime.Trim().Equals("") && !noviLekar.Prezime.Trim().Equals(""))
            {
                bool uspesno = lekarKontroler.IzmeniLekara(lekar.Jmbg, noviLekar);
                if (uspesno)
                {
                    lekariPrikaz.ItemsSource = lekarKontroler.GetAll();
                    SekretarWindow.SortirajDataGrid(lekariPrikaz, 1, ListSortDirection.Ascending);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
                    jmbg.Focus();
                }
            }
            else if (noviLekar.Jmbg.Trim().Equals("") || noviLekar.Ime.Trim().Equals("") || noviLekar.Prezime.Trim().Equals(""))
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
