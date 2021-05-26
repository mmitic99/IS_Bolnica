using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for ObavestenjaPage.xaml
    /// </summary>
    public partial class LekoviPage : Page
    {
       
        public LekDTO lek;
        public List<LekDTO> Lekovi;
        public VerifikacijaLekaDTO verifikacijaLeka;

        private static LekoviPage instance = null;
        


        public static LekoviPage getInstance()
        {
            return instance;
        }
      
        public LekoviPage()
        {
            InitializeComponent();
            this.DataContext = this;
            Lekovi = LekKontroler.GetInstance().GetAll(); 
            TabelaVerifikacija.ItemsSource = VerifikacijaLekaKontroler.GetInstance().GetObavestenjaByJmbg(LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
            TabelaLekova.ItemsSource = LekKontroler.GetInstance().GetAll();
            List<VerifikacijaLekaDTO> VerifikacijaLekova = new List<VerifikacijaLekaDTO>();
            VerifikacijaLekova = VerifikacijaLekaKontroler.GetInstance().GetObavestenjaByJmbg(LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
            ImeDoktora.DataContext = LekarKontroler.getInstance().trenutnoUlogovaniLekar();
            instance = this;


        }

        private void TabelaLekova_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   if (TabelaLekova.SelectedIndex != -1)
            {
                lek = (LekDTO)TabelaLekova.SelectedItem;
                int indexSelektovanogLeka = TabelaLekova.SelectedIndex;
                txt1.Text = Lekovi[indexSelektovanogLeka].NazivLeka;
                txt2.Text = Lekovi[indexSelektovanogLeka].JacinaLeka.ToString();
                txt3.Text = Lekovi[indexSelektovanogLeka].ZamenskiLek;
                txt4.Text = Lekovi[indexSelektovanogLeka].SastavLeka;
                VrstaCombo.ItemsSource = Enum.GetValues(typeof(VrstaLeka));
                VrstaCombo.SelectedItem = Lekovi[indexSelektovanogLeka].VrstaLeka;
                KlasaCombo.ItemsSource = Enum.GetValues(typeof(KlasaLeka));
                KlasaCombo.SelectedItem = Lekovi[indexSelektovanogLeka].KlasaLeka;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            if (TabelaLekova.SelectedIndex != -1)
            {
                LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO(VrstaCombo.SelectedIndex, lek.KolicinaLeka.ToString(), txt1.Text, KlasaCombo.SelectedIndex, txt2.Text, txt3.Text, txt4.Text);
                

                LekDTO lekZaIzmenu = new LekDTO(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka), Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
                    LekKontroler.GetInstance().IzmeniLekLekar(TabelaLekova.SelectedIndex, lekZaIzmenu);
                
            }
            else
            {
                MessageBox.Show("Označite lek koji želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LekoviPage.getInstance().TabelaLekova.ItemsSource = new ObservableCollection<LekDTO>(LekKontroler.GetInstance().GetAll());

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { String napomena = NapomenaTxt.Text;
            VerifikacijaLekaDTO vL = new VerifikacijaLekaDTO(verifikacijaLeka.VremeSlanjaZahteva, verifikacijaLeka.Naslov, verifikacijaLeka.Sadrzaj, verifikacijaLeka.JmbgPrimaoca, verifikacijaLeka.JmbgPosiljaoca, napomena);
            VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(vL);
            NapomenaTxt.Text = "";
            SastavTxt.Text = "";
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            verifikacijaLeka = (VerifikacijaLekaDTO)TabelaVerifikacija.SelectedItem;
            SastavTxt.Text = ((VerifikacijaLekaDTO)TabelaVerifikacija.SelectedItem).Sadrzaj;

        }

        private void MenuItem_Click_Termini(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().trenutnoUlogovaniLekar());
        }

        private void MenuItem_Click_Pacijenti(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
        }


        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }
        private void MenuItem_Click_Obavestenja(object sender, RoutedEventArgs e)

        {
            LekarWindow.getInstance().Frame1.Content = new LekarObavestenjaPage();
        }

    }
}
