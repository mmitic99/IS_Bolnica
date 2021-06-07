using Bolnica.DTOs;
using Kontroler;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for DodavanjeObavestenja.xaml
    /// </summary>
    public partial class DodavanjeObavestenja : Window
    {
        private ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();
        private DataGrid obavestenjaPrikaz;
        public DodavanjeObavestenja(DataGrid obavestenjaPrikaz)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.obavestenjaPrikaz = obavestenjaPrikaz;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (!naslov.Text.Equals("") || !sadrzaj.Text.Equals(""))
            {
                ObavestenjeDTO obavestenje = new ObavestenjeDTO
                {
                    JmbgKorisnika = "-1",
                    Naslov = naslov.Text,
                    Sadrzaj = sadrzaj.Text,
                    Podsetnik = false,
                    VremeObavestenja = DateTime.Now,
                    Vidjeno = false
                };
                bool uspesno = obavestenjaKontroler.Save(obavestenje);

                if (!uspesno)
                {
                    MessageBox.Show("Desila se greška prilikom kreiranja obaveštenja.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetOavestenjaByJmbg("-1");
                this.Close();

            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
