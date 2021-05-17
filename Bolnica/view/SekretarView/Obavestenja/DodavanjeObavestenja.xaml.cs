using Kontroler;
using Model;
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
using Bolnica.DTOs;

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
            if(!naslov.Text.Equals("") || !sadrzaj.Text.Equals(""))
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

                if (uspesno)
                {
                    obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
                    this.Close();
                }

            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
