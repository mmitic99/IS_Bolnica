using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Termini
{
    /// <summary>
    /// Interaction logic for DodavanjeLekaraTerminu.xaml
    /// </summary>
    public partial class DodavanjeLekaraTerminu : Window
    {
        private Termin termin;
        private TextBox lekar;
        private LekarKontroler lekarKontroler;
        public DodavanjeLekaraTerminu(Termin termin, TextBox lekar)
        {
            InitializeComponent();
            lekarKontroler = new LekarKontroler();

            DodavanjeLekaraTerminuPrikaz.ItemsSource = lekarKontroler.GetAll(); ;
            this.termin = termin;
            this.lekar = lekar;
        }
        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (DodavanjeLekaraTerminuPrikaz.SelectedIndex != -1)
            {
                LekarDTO izabraniLekar = (LekarDTO)DodavanjeLekaraTerminuPrikaz.SelectedItem;
                termin.JmbgLekara = izabraniLekar.Jmbg;
                lekar.Text = izabraniLekar.Ime + " " + izabraniLekar.Prezime;
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati lekara koga želite da dodate.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
