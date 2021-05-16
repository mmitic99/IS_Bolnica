using System;
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
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for DodavanjeLekara.xaml
    /// </summary>
    public partial class DodavanjeLekara : Window
    {
        private DataGrid lekariPrikaz;
        private SpecijalizacijaKontroler specijalizacijaKontroler;
        private LekarKontroler lekarKontroler;
        public DodavanjeLekara(DataGrid lekariPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            DataContext = new Lekar();

            specijalizacijaKontroler = new SpecijalizacijaKontroler();
            lekarKontroler = new LekarKontroler();

            this.lekariPrikaz = lekariPrikaz;
            Specijalizacija.ItemsSource = specijalizacijaKontroler.GetAll();
            Specijalizacija.SelectedIndex = 0;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Lekar lekar = new Lekar()
            {
                Ime = ime.Text,
                Prezime = prezime.Text,
                Jmbg = jmbg.Text,
                Adresa = adresa.Text,
                BrojTelefona = tel.Text,
                Email = email.Text,
                Grad = new Grad {Naziv = grad.Text},
                Korisnik = new Korisnik {KorisnickoIme = jmbg.Text, Lozinka = ime.Text},
                Pol = Pol.SelectedIndex == 0 ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski,
                Specijalizacija = (Specijalizacija)Specijalizacija.SelectedItem,
                BrojSlobodnihDana = 25
                

            };
            if (datum.SelectedDate != null)
            {
                lekar.DatumRodjenja = (DateTime)datum.SelectedDate;
            }
            else
            {
                lekar.DatumRodjenja = DateTime.Now;
            }

            if (!lekar.Jmbg.Trim().Equals("") && !lekar.Ime.Trim().Equals("") && !lekar.Prezime.Trim().Equals(""))
            {
                bool uspesno = lekarKontroler.RegistrujLekara(lekar);
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
            else if (lekar.Jmbg.Trim().Equals("") || lekar.Ime.Trim().Equals("") || lekar.Prezime.Trim().Equals(""))
            {
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

    private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
