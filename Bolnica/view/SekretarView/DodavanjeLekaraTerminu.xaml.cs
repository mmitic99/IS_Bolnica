using Model;
using Repozitorijum;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for DodavanjeLekaraTerminu.xaml
    /// </summary>
    public partial class DodavanjeLekaraTerminu : Window
    {
        Termin termin;
        TextBox lekar;
        public DodavanjeLekaraTerminu(Termin termin, TextBox lekar)
        {
            InitializeComponent();
            DodavanjeLekaraTerminuPrikaz.ItemsSource = SkladisteZaLekara.GetInstance().GetAll();
            this.termin = termin;
            this.lekar = lekar;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DodavanjeLekaraTerminuPrikaz.SelectedIndex != -1)
            {
                Lekar selLek = (Lekar)DodavanjeLekaraTerminuPrikaz.SelectedItem;
                termin.JmbgLekara = selLek.Jmbg;
                lekar.Text = selLek.Ime + " " + selLek.Prezime;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
