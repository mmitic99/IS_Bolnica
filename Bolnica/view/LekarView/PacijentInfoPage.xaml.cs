
using Kontroler;
using Model;
using Bolnica.model;
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
using Servis;
using Bolnica.DTOs;
using Bolnica.Kontroler;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for PacijentInfoPage.xaml
    /// </summary>
    public partial class PacijentInfoPage : Page
    {
        private static PacijentInfoPage instance = null;
        public ZdravstveniKartonDTO Karton;
        public String Jmbg;
        public PacijentDTO pacijent;

        public static PacijentInfoPage getInstance()
        {
            return instance;
        }
        public PacijentInfoPage(String jmbg)
        {
           
            
            InitializeComponent();
            instance = this;
            int izabraniPacijent = 0;
            Jmbg = jmbg;
            ImeDoktora.DataContext = LekarKontroler.getInstance().trenutnoUlogovaniLekar();
            List<PacijentDTO> pacijentiDTO = PacijentKontroler.GetInstance().GetAll();
            ComboBox1.ItemsSource = pacijentiDTO;
            
            List<ProstorijaDTO> prostorije = new List<ProstorijaDTO>();

            foreach(ProstorijaDTO p in ProstorijeKontroler.GetInstance().GetAll())
            {
                if (p.VrstaProstorije == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
                    prostorije.Add(p);
            }
            ProstorijaBox.ItemsSource = prostorije;

            if (jmbg != null)
            {

                 pacijent = PacijentKontroler.GetInstance().GetByJmbg(jmbg);
                
                
                foreach (PacijentDTO pacijentDTO in PacijentKontroler.GetInstance().GetAll())
                {
                    if (pacijentDTO.Jmbg.Equals(jmbg))
                    {
                        break;
                    }
                    izabraniPacijent++;
                }
                ComboBox1.SelectedIndex = izabraniPacijent;
                txt1.Text = pacijent.Ime;
                txt2.Text = pacijent.Prezime;
                if (pacijent.Pol == Model.Enum.Pol.Muski)
                    txt3.Text = "M";
                else
                    txt3.Text = "Ž";

                txt4.Text = pacijent.DatumRodjenja.ToShortDateString();
                txt5.Text = pacijent.Adresa;
                txt6.Text = pacijent.BracnoStanje;
                txt7.Text = pacijent.Zanimanje;
                txt8.Text = pacijent.BrojTelefona;
                ObservableCollection<String> alergeni = new ObservableCollection<String>(pacijent.ZdravstveniKarton.Alergeni);
                String alergeniString = String.Join(",", alergeni);
                txt9.Text = alergeniString;
            }
    }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new IzdavanjeReceptaPage();
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pacijent = (PacijentDTO)ComboBox1.SelectedItem;
            Jmbg = pacijent.Jmbg;
            txt1.Text = pacijent.Ime;
            txt2.Text = pacijent.Prezime;
            if (pacijent.Pol == Model.Enum.Pol.Muski)
                txt3.Text = "M";
            else
                txt3.Text = "Ž";
            txt4.Text = pacijent.DatumRodjenja.ToString();
            txt5.Text = pacijent.Adresa;
            txt6.Text = pacijent.BracnoStanje;
            txt7.Text = pacijent.Zanimanje;
            txt8.Text = pacijent.BrojTelefona;
            ObservableCollection<String> alergeni = new ObservableCollection<String>(pacijent.ZdravstveniKarton.Alergeni);
            String alergeniString = String.Join(",", alergeni);
            txt9.Text = alergeniString;
        }

        private void MenuItem_Click_Termini(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().trenutnoUlogovaniLekar());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String dijalog = AnamnezaTxt.Text;
            Anamneza anamneza = new Anamneza()
            {
                AnamnezaDijalog = dijalog, 
                DatumAnamneze=DateTime.Now, 
                ImeLekara= LekarKontroler.getInstance().trenutnoUlogovaniLekar().FullName
            };
            PacijentDTO pacijent = (PacijentDTO)ComboBox1.SelectedItem;
            Pacijent pacijentIzmena = PacijentServis.GetInstance().GetByJmbg(pacijent.Jmbg);
            Pacijent pacijentNovi = pacijentIzmena;
            pacijentNovi.ZdravstveniKarton.Anamneze.Add(anamneza);
            PacijentServis.GetInstance().IzmeniPacijenta(pacijentIzmena, pacijentNovi);
            AnamnezaTxt.Clear();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(Jmbg);
        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new ZakazivanjeTerminaPage(Jmbg);
        }

        private void MenuItem_Click_Lekovi(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekoviPage();
        }

        private void MenuItem_Click_Obavestenja(object sender, RoutedEventArgs e)

        {
            LekarWindow.getInstance().Frame1.Content = new LekarObavestenjaPage();
        }
        private void Button_Click_SlanjeNaLecenje(object sender, RoutedEventArgs e)
        {
           ProstorijaDTO prostorija = ((ProstorijaDTO)ProstorijaBox.SelectedItem);
            DateTime vremePocetkaLecenja = (DateTime)PocetakLecenjaBox.SelectedDate;
            DateTime vremeKrajaLecenja = (DateTime)KrajLecenjaBox.SelectedDate;
            int zauzetiKreveti = 0;
            int brojKreveta = 0;
            bool pacijentVecLezi = false;
           foreach (BolnickoLecenjeDTO bl in BolnickaLecenjaKontroler.GetInstance().GetAll())
            {
                if (bl.idProstorije == prostorija.IdProstorije)
                {
                    zauzetiKreveti++;
                }

            }
           
                foreach(StacionarnaOpremaDTO so in prostorija.Staticka)
            {
                if (so.TipStacionarneOpreme.Equals("Krevet"))
                {
                     brojKreveta = so.Kolicina;
                }
            }
                foreach(BolnickoLecenjeDTO bolnickoLecenjeDTO in BolnickaLecenjaKontroler.GetInstance().GetAll())
            {
                if (Jmbg.Equals(bolnickoLecenjeDTO.jmbgPacijenta))
                    pacijentVecLezi = true;
            }
            if (pacijentVecLezi)
            {
                MessageBox.Show("Pacijent je več na stacionarnom lečenju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (zauzetiKreveti < brojKreveta)
            {
                BolnickoLecenjeDTO bolnickoLecenjeDTO = new BolnickoLecenjeDTO()
                {
                    idProstorije = prostorija.IdProstorije,
                    jmbgPacijenta = Jmbg,
                    jmbgLekara = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg,
                    DatumPrijema = vremePocetkaLecenja,
                    DatumOtpustanja = vremeKrajaLecenja
                };
                BolnickaLecenjaKontroler.GetInstance().Save(bolnickoLecenjeDTO);
            }
            else {
                MessageBox.Show("Svi kreveti su zauzeti, izaberite drugu prostoriju!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            }




        }
        private void Button_Click_Prikaz_Lecenja(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PrikazBolnickihLecenja();
                }

    }
}
