using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.view.SekretarView.Pacijenti
{
    /// <summary>
    /// Interaction logic for DodavanjePacijenta.xaml
    /// </summary>
    public partial class DodavanjePacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private PacijentKontroler pacijentKontroler;
        private List<String> bracnoStanjeMuskarac = new List<string> {"neoženjen", "oženjen", "udovac", "razveden", "ostalo"};
        private List<String> bracnoStanjeZena = new List<string> {"neudata", "udata", "udovica", "razvedena", "ostalo"};

        public DodavanjePacijenta(DataGrid pacijentiPrikaz)
        {
            InitializeComponent();
            DataContext = new PacijentDTO();
            this.pacijentiPrikaz = pacijentiPrikaz;
            pacijentKontroler = new PacijentKontroler();
            this.Owner = App.Current.MainWindow;

            BracnoStanje.ItemsSource = bracnoStanjeMuskarac;
            BracnoStanje.SelectedIndex = 0;

        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            PacijentDTO pacijent = new PacijentDTO
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                NazivGrada = grad.Text,
                ZdravstveniKarton = new ZdravstveniKartonDTO(),
                Korisnik = new KorisnikDTO() {KorisnickoIme = jmbg.Text, Lozinka = ime.Text},
                BracnoStanje = (string) BracnoStanje.SelectedItem,
                Zanimanje = (string) Zanimanje.Text,
                Pol = Pol.SelectedIndex == 0 ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski
            };

            if (datum.SelectedDate != null)
            {
                pacijent.DatumRodjenja = (DateTime)datum.SelectedDate;
            }
            else
            {
                pacijent.DatumRodjenja = DateTime.Now;
            }

            if (!pacijent.Jmbg.Trim().Equals("") && !pacijent.Ime.Trim().Equals("") && !pacijent.Prezime.Trim().Equals(""))
            {
                bool uspesno = pacijentKontroler.RegistrujPacijenta(pacijent);
                if (uspesno)
                {
                    pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                    SekretarWindow.SortirajDataGrid(pacijentiPrikaz, 0, ListSortDirection.Ascending);
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
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pol_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BracnoStanje != null)
            {
                if (Pol.SelectedIndex == 0)
                {
                    BracnoStanje.ItemsSource = bracnoStanjeMuskarac;
                    BracnoStanje.SelectedIndex = 0;
                }
                else
                {
                    BracnoStanje.ItemsSource = bracnoStanjeZena;
                    BracnoStanje.SelectedIndex = 0;
                }
            }
        }
    }
}
