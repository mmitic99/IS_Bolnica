using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.view.SekretarView.Pacijenti
{
    /// <summary>
    /// Interaction logic for IzmenaPacijenta.xaml
    /// </summary>
    public partial class IzmenaPacijenta : Window
    {
        private DataGrid pacijentiPrikaz;
        private PacijentDTO pacijent;
        private ObservableCollection<String> alergeni;
        private PacijentKontroler pacijentKontroler; 
        private List<String> bracnoStanjeMuskarac = new List<string> { "neoženjen", "oženjen", "udovac", "razveden", "ostalo" };
        private List<String> bracnoStanjeZena = new List<string> { "neudata", "udata", "udovica", "razvedena", "ostalo" };

        public IzmenaPacijenta(DataGrid pacijentiPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            pacijentKontroler = new PacijentKontroler();

            this.pacijentiPrikaz = pacijentiPrikaz;
            this.pacijent = (PacijentDTO)pacijentiPrikaz.SelectedItem;
            jmbg.Text = pacijent.Jmbg;
            ime.Text = pacijent.Ime;
            prezime.Text = pacijent.Prezime;
            adresa.Text = pacijent.Adresa;
            tel.Text = pacijent.BrojTelefona;
            email.Text = pacijent.Email;
            grad.Text = pacijent.NazivGrada;
            korIme.Text = pacijent.Korisnik.KorisnickoIme;
            lozinka.Password = pacijent.Korisnik.Lozinka;
            datum.SelectedDate = pacijent.DatumRodjenja.Date;
            if(pacijent.ZdravstveniKarton == null)
            {
                pacijent.ZdravstveniKarton = new ZdravstveniKartonDTO();
            }
            if (pacijent.Pol == Model.Enum.Pol.Muski)
            {
                BracnoStanje.ItemsSource = bracnoStanjeMuskarac;
                pol.SelectedIndex = 0;
                if (bracnoStanjeMuskarac.Contains(pacijent.BracnoStanje))
                {
                    BracnoStanje.SelectedItem = pacijent.BracnoStanje;
                }
            }
            else
            {
                BracnoStanje.ItemsSource = bracnoStanjeZena;
                pol.SelectedIndex = 1;
                if (bracnoStanjeZena.Contains(pacijent.BracnoStanje))
                {
                    BracnoStanje.SelectedItem = pacijent.BracnoStanje;
                }
            }

            Zanimanje.Text = pacijent.Zanimanje;
            alergeni = new ObservableCollection<String>(pacijent.ZdravstveniKarton.Alergeni);
            alergeniList.ItemsSource = alergeni;

            DataContext = new PacijentDTO()
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                NazivGrada = grad.Text,
                Korisnik = new KorisnikDTO()
                {
                    KorisnickoIme = korIme.Text,
                    Lozinka = lozinka.Password
                },
                ZdravstveniKarton = pacijent.ZdravstveniKarton,
                BracnoStanje = (string)BracnoStanje.SelectedItem,
                Zanimanje = Zanimanje.Text
            };

        }

        private void sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            PacijentDTO noviPacijent = new PacijentDTO
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                NazivGrada = grad.Text ,
                Korisnik = new KorisnikDTO()
                {
                    KorisnickoIme = korIme.Text,
                    Lozinka = lozinka.Password
                },
                ZdravstveniKarton = pacijent.ZdravstveniKarton,
                BracnoStanje = (string)BracnoStanje.SelectedItem,
                Zanimanje = Zanimanje.Text,
                datumKreiranjaNaloga = pacijent.datumKreiranjaNaloga
            };
            noviPacijent.ZdravstveniKarton.Alergeni = new List<string>(alergeni);

            noviPacijent.Pol = pol.SelectedIndex == 0 ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski;
            noviPacijent.DatumRodjenja = datum.SelectedDate != null ? (DateTime) datum.SelectedDate : DateTime.Now;


            if (!noviPacijent.Jmbg.Trim().Equals("") && !noviPacijent.Ime.Trim().Equals("") && !noviPacijent.Prezime.Trim().Equals(""))
            {
                bool uspesno = pacijentKontroler.IzmeniPacijenta(pacijent, noviPacijent);
                if (!uspesno)
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    jmbg.Focus();
                    return;
                }

                pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                SekretarWindow.SortirajDataGrid(pacijentiPrikaz, 0, ListSortDirection.Ascending);
                this.Close();
            }
            else if (pacijent.Jmbg.Trim().Equals("") || pacijent.Ime.Trim().Equals("") || pacijent.Prezime.Trim().Equals(""))
            {
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!!!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DodajAlergen_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeAlregena(alergeni);
            s.ShowDialog();
        }

        private void ObrisiAlergen_Click(object sender, RoutedEventArgs e)
        {
            if (alergeniList.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati alergen koji želite da obrišete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            alergeni.RemoveAt(alergeniList.SelectedIndex);
        }
    }
}
