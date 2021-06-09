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
using System.Text.RegularExpressions;

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
            setToolTip(LekarProfilPage.isToolTipVisible);
            PosaljiBtn.IsEnabled = false;
            izmeniButton.IsEnabled = false;

        }

        private void TabelaLekova_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   if (TabelaLekova.SelectedIndex != -1)
            {
                izmeniButton.IsEnabled = true;
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
        private bool Validiraj(Regex sablon, String unos)
        {
            if (sablon.IsMatch(unos))
                return true;
            else
                return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            if (TabelaLekova.SelectedIndex != -1)
            {
                if (Validiraj(new Regex(@"^[A-Za-z]{1,25}$"), txt1.Text))
                {
                    if (Validiraj(new Regex(@"^[0-9]{1,4}$"), txt2.Text))
                    {
                        if (Validiraj(new Regex(@"^[A-Za-z]{1,25}$"), txt3.Text))
                        {
                            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO(VrstaCombo.SelectedIndex, lek.KolicinaLeka.ToString(), txt1.Text, KlasaCombo.SelectedIndex, txt2.Text, txt3.Text, txt4.Text);


                            LekDTO lekZaIzmenu = new LekDTO(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka), Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
                            LekKontroler.GetInstance().IzmeniLekLekar(TabelaLekova.SelectedIndex, lekZaIzmenu);
                        }
                        else
                            MessageBox.Show("Ime zamenskog leka mora bit string  !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Jacina mora biti broj !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Ime leka mora bit string !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Označite lek koji želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LekoviPage.getInstance().TabelaLekova.ItemsSource = new ObservableCollection<LekDTO>(LekKontroler.GetInstance().GetAll());

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String napomena = NapomenaTxt.Text;
            if (TabelaVerifikacija.SelectedIndex != -1) {
                if (NapomenaTxt.Text != "")
                {
                    VerifikacijaLekaDTO vL = new VerifikacijaLekaDTO(verifikacijaLeka.VremeSlanjaZahteva, verifikacijaLeka.Naslov, verifikacijaLeka.Sadrzaj, verifikacijaLeka.JmbgPrimaoca, verifikacijaLeka.JmbgPosiljaoca, napomena);
                    VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(vL);
                    NapomenaTxt.Text = "";
                    SastavTxt.Text = "";
                }
                else MessageBox.Show("Dodajte komentar !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        else MessageBox.Show("Označite lek koji želite da verifikujete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PosaljiBtn.IsEnabled = true;
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
        private void setToolTip(bool Prikazi)
        {


            if (Prikazi)
            {
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();
                this.Resources.Add(typeof(ToolTip), style);


            }
            else
            {
                this.Resources.Remove(typeof(ToolTip));
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void MenuItem_Click_Profil(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekarProfilPage();
        }
    }
}
