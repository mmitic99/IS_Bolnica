using Bolnica.DTOs;
using Kontroler;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for IzmenaObavestenja.xaml
    /// </summary>
    public partial class IzmenaObavestenja : Window
    {
        private DataGrid obavestenjaPrikaz;
        private ObavestenjeDTO obavestenje;
        private ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();
        public IzmenaObavestenja(DataGrid obavestenjaPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.obavestenjaPrikaz = obavestenjaPrikaz;
            obavestenje = (ObavestenjeDTO)obavestenjaPrikaz.SelectedItem;
            naslov.Text = obavestenje.Naslov;
            sadrzaj.Text = obavestenje.Sadrzaj;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            ObavestenjeDTO novoObavestenje = new ObavestenjeDTO
            {
                Naslov = naslov.Text,
                Sadrzaj = sadrzaj.Text,
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = obavestenje.JmbgKorisnika,
                Podsetnik = obavestenje.Podsetnik
            };

            bool uspesno = obavestenjaKontroler.IzmeniObavestenje(obavestenje, novoObavestenje);
            if (!uspesno)
            {
                MessageBox.Show("Desila se greška prilikom izmene obaveštenja.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            obavestenjaPrikaz.ItemsSource = obavestenjaKontroler.GetObavestenjaByJmbg("-1");
            obavestenjaPrikaz.ScrollIntoView(novoObavestenje);
            obavestenjaPrikaz.SelectedItem = novoObavestenje;
            this.Close();
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
