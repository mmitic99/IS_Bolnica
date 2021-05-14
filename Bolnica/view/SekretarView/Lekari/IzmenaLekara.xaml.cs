using System;
using System.Collections.Generic;
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
    /// Interaction logic for IzmenaLekara.xaml
    /// </summary>
    public partial class IzmenaLekara : Window
    {
        private DataGrid lekariPrikaz;
        private Lekar lekar;
        private LekarKontroler lekarKontroler;
        private SpecijalizacijaKontroler specijalizacijaKontroler;
        public IzmenaLekara(DataGrid lekariPrikaz)
        {
            InitializeComponent(); 
            this.Owner = App.Current.MainWindow;
            lekarKontroler = new LekarKontroler();
            specijalizacijaKontroler = new SpecijalizacijaKontroler();

            this.lekariPrikaz = lekariPrikaz;
            this.lekar = (Lekar)lekariPrikaz.SelectedItem;
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
            throw new NotImplementedException();
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
