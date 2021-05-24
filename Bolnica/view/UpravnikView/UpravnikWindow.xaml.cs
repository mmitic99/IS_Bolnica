using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Kontroler;
using Model;
using Repozitorijum;
using Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.view.UpravnikView
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        Upravnik upravnik;

        private static UpravnikWindow instance = null;

        public static UpravnikWindow GetInstance()
        {
            return instance;
        }

        public UpravnikWindow(Upravnik upravnik)
        {
            InitializeComponent();

            this.upravnik = upravnik;
            instance = this;
            this.DataContext = this;

            // TODO : izmeniti tip sa Prostorija na ProstorijaDTO
            //ListaProstorija = ProstorijeKontroler.GetInstance().GetAll();
            TabelaProstorija.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
            TabelaProstorijaIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
            TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
            TabelaProstorijeUKojuSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
            TabelaStatickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Staticka_;
            TabelaDinamickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna_;
            TabelaZakazanihPrebacivanjaOpreme.ItemsSource =  SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll();
            TabelaLekova.ItemsSource = SkladisteZaLekoveXml.GetInstance().GetAll();
            TabelaLekovaIzmeni.ItemsSource = SkladisteZaLekoveXml.GetInstance().GetAll();
            TabelaRenoviranja.ItemsSource = SkladisteZaRenoviranjaXml.GetInstance().GetAll();
            TabelaVerifikacija.ItemsSource = SkladisteZaVerifikacijuLekaXml.GetInstance().GetObavestenjaByJmbg("1903999772025");
            BrojProstorijeRenoviranje.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
            LekariLekovi.ItemsSource = SkladisteZaLekaraXml.GetInstance().GetAll();
            LekariLekoviIzmeni.ItemsSource = SkladisteZaLekaraXml.GetInstance().GetAll();
            ProstorijeKontroler.GetInstance().AzurirajRenoviranjaProstorija();
            ProstorijeKontroler.GetInstance().AzurirajStanjeOpremeAkoJeBiloPrebacivanja();
        }

        // metoda za dodavanje nove prostorije
        private void DodajProstoriju(object sender, RoutedEventArgs e)
        {
            ProstorijaValidacijaDTO prostorijaZaValidaciju = new ProstorijaValidacijaDTO
                                                                 (
                                                                 BrojProstorijeTextBox.Text, 
                                                                 SpratTextBox.Text, 
                                                                 VrstaProstorijeComboBox.SelectedIndex, 
                                                                 KvadraturaTextBox.Text
                                                                 );
            if (ProstorijeKontroler.GetInstance().ProveriValidnostProstorije(prostorijaZaValidaciju))
            {
                ProstorijaDTO prostorija = new ProstorijaDTO
                                               (
                                               BrojProstorijeTextBox.Text, 
                                               Int32.Parse(SpratTextBox.Text), 
                                               ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBox.SelectedIndex), 
                                               Double.Parse(KvadraturaTextBox.Text)
                                               );
                ProstorijeKontroler.GetInstance().DodajProstoriju(prostorija);
            }
        }

        // metoda za izmenu selektovane prostorije
        private void IzmeniProstoriju(object sender, RoutedEventArgs e)
        {

            if (TabelaProstorijaIzmeni.SelectedIndex != -1)
            {
                ProstorijaValidacijaDTO prostorijaZaValidaciju = new ProstorijaValidacijaDTO
                                                                     (
                                                                     BrojProstorijeTextBoxIzmeni.Text, 
                                                                     SpratTextBoxIzmeni.Text, 
                                                                     VrstaProstorijeComboBoxIzmeni.SelectedIndex, 
                                                                     KvadraturaTextBoxIzmeni.Text
                                                                     );
                if (ProstorijeKontroler.GetInstance().ProveriValidnostIzmeneProstorije(prostorijaZaValidaciju))
                {
                    ProstorijaDTO prostorija = new ProstorijaDTO
                                                   (
                                                   BrojProstorijeTextBoxIzmeni.Text, 
                                                   Int32.Parse(SpratTextBoxIzmeni.Text), 
                                                   ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBoxIzmeni.SelectedIndex), 
                                                   Double.Parse(KvadraturaTextBoxIzmeni.Text)
                                                   );
                    ProstorijeKontroler.GetInstance().IzmeniProstoriju(TabelaProstorijaIzmeni.SelectedIndex, prostorija);
                }
            }
            else
            {
                MessageBox.Show("Označite prostoriju koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // metoda za brisanje selektovane prostorije
        private void ObrisiProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaProstorija.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiProstoriju(TabelaProstorija.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Označite prostoriju koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabelaProstorijaIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaProstorijaIzmeni.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().NamapirajProstoriju(TabelaProstorijaIzmeni.SelectedIndex);
            }
        }

        private void ObrisiStatickuOpremuIzMagacina(object sender, RoutedEventArgs e)
        {
            if (TabelaStatickeMagacin.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzMagacina(TabelaStatickeMagacin.SelectedIndex);
            }
            else
                MessageBox.Show("Označite statičku opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajStatickuOpremuUMagacin(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivStatickeOpreme.Text, KolicinaStatickeOpreme.Text))
                ProstorijeKontroler.GetInstance().DodajStacionarnuOpremuUMagacin(NazivStatickeOpreme.Text, Int32.Parse(KolicinaStatickeOpreme.Text));
        }

        private void ObrisiPotrosnuOpremuIzMagacina(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacin.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiPotrosnuOpremuIzMagacina(TabelaDinamickeMagacin.SelectedIndex);
            }
            else
                MessageBox.Show("Označite potrošnu opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajPotrosnuOpremuUMagacin(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivDinamickeOpreme.Text, KolicinaDinamickeOpreme.Text))
                ProstorijeKontroler.GetInstance().DodajPotrosnuOpremuUMagacin(NazivDinamickeOpreme.Text, Int32.Parse(KolicinaDinamickeOpreme.Text));
        }

        private void IzmeniStatickuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaStatickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaStatickeOpremeIzmeni.Text))
                    ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuUMagacinu(TabelaStatickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaStatickeOpremeIzmeni.Text));
            }
        }

        private void IzmeniPotrosnuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaDinamickeOpremeIzmeni.Text))
                    ProstorijeKontroler.GetInstance().IzmeniDinamickuOpremuUMagacinu(TabelaDinamickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaDinamickeOpremeIzmeni.Text));
            }
        }
        private void TabelaStatickeMagacinIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaStatickeMagacinIzmeni.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().NamapirajStatickuOpremuMagacina(TabelaStatickeMagacinIzmeni.SelectedIndex);
        }

        private void TabelaDinamickeMagacinIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().NamapirajDinamickuOpremuMagacina(TabelaDinamickeMagacinIzmeni.SelectedIndex);
        }

        private void PrikaziProzorZaSelektovanuProstoriju(object sender, RoutedEventArgs e)
        {
            if ((TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex))
            {
                TabelaOpremeIzKojeSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex);
                TabelaOpremeUKojuSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
                ProstorijaIzKojeSePrebacujeLabel.Content = ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe_
                    + " - " + VrstaProstorijeLepIspis(ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].VrstaProstorije);
                ProstorijaUKojuSePrebacujeLabel.Content = ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe_
                    + " - " + VrstaProstorijeLepIspis(ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].VrstaProstorije_);
                RaspodelaOpremeTab.IsEnabled = true;
                RaspodelaOpremeTab.IsSelected = true;
            }
            else if (TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex == -1)
                MessageBox.Show("Označite prostoriju iz desne tabele u koju želite da prebacite opremu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex == -1)
                MessageBox.Show("Označite prostoriju iz leve tabele iz koje želite da prebacite opremu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Označili ste dve iste prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PrikaziProzorZaZakazivanjePrebacivanjaOpreme(object sender, RoutedEventArgs e)
        {
            if (TabelaOpremeIzKojeSePrebacuje.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpremePriPrebacivanju(KolicinaOpremeSKojomSeRadi_Copy.Text))
                    if (ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].Staticka_[TabelaOpremeIzKojeSePrebacuje.SelectedIndex].Kolicina_ - Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text) >= 0)
                    {
                        ZakazanaPrebacivanjaOpremeTab.IsEnabled = true;
                        ZakazanaPrebacivanjaOpremeTab.IsSelected = true;
                        ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text = ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe_;
                        ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text = ProstorijeKontroler.GetInstance().GetAllProstorije()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe_;
                        NazivOpremeZakazivanje.Text = NazivOpremeSKojomSeRadi_Copy.Text;
                        KolicinaOpremeZakazivanje.Text = KolicinaOpremeSKojomSeRadi_Copy.Text;
                    }
                    else
                        MessageBox.Show("Ne možete prebaciti toliko opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz leve tabele za koju želite da zakažete prebacivanje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void RaspodelaOpremeTab_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RaspodelaOpremeTab.IsSelected == false)
            {
                NazivOpremeSKojomSeRadi.Clear();
                KolicinaOpremeSKojomSeRadi.Clear();
                RaspodelaOpremeTab.IsEnabled = false;
            }
        }

        private void PrebaciStatickuOpremuUProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaOpremeIzKojeSePrebacuje.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpremePriPrebacivanju(KolicinaOpremeSKojomSeRadi_Copy.Text))
                    ProstorijeKontroler.GetInstance().PrebaciStacionarnuOpremuUProstoriju(TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex, TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex, NazivOpremeSKojomSeRadi_Copy.Text, Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text));
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz leve kolone koju želite da prebacite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IzmeniStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaOpremeSKojomSeRadi.Text))
                    ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex, TabelaOpremeUKojuSePrebacuje.SelectedIndex, Int32.Parse(KolicinaOpremeSKojomSeRadi.Text));
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ObrisiStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex, TabelaOpremeUKojuSePrebacuje.SelectedIndex);
            else
            {
                MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da obrišete iz prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabelaOpremeIzKojeSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeIzKojeSePrebacuje.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().NamapirajOpremuZaPrebacivanjeIzProstorije(TabelaOpremeIzKojeSePrebacuje.SelectedIndex, TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex);
        }

        private void TabelaOpremeUKojuSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().NamapirajOpremuZaPrebacivanjeUProstoriju(TabelaOpremeUKojuSePrebacuje.SelectedIndex, TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
        }
        private String VrstaProstorijeLepIspis(Model.Enum.VrstaProstorije vrsta)
        {
            if (vrsta == Model.Enum.VrstaProstorije.Magacin)
                return "MAGACIN";
            else if (vrsta == Model.Enum.VrstaProstorije.Operaciona_sala)
                return "OPERACIONA SALA";
            else if (vrsta == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
                return "SOBA ZA BOLESNIKE";
            else
                return "SOBA ZA PREGLEDE";
        }

        private void VratiSeNazad_Click(object sender, RoutedEventArgs e)
        {
            RaspodelaOpremeTab.IsEnabled = true;
            RaspodelaOpremeTab.IsSelected = true;
        }

        private void ZakazanaPrebacivanjaOpremeTab_LostFocus(object sender, RoutedEventArgs e)
        {
            ZakazanaPrebacivanjaOpremeTab.IsEnabled = false;
        }

        private DateTime PodesiDatumIVremePreraspodele()
        {
            DateTime datumVreme = (DateTime)DatumPreraspodele.SelectedDate;
            int sati = (int)Sat.SelectedIndex + 6;
            int minuti = 30;
            if (Minut.SelectedIndex == 0)
            {
                minuti = 0;
            }
            return new DateTime(datumVreme.Year, datumVreme.Month, datumVreme.Day, sati, minuti, 0);
        }
        private void ZakaziPrebacivanjeOpreme_Click(object sender, RoutedEventArgs e)
        {
            ZakazanaPreraspodelaStatickeOpremeDTO preraspodela = new ZakazanaPreraspodelaStatickeOpremeDTO();
            if (DatumPreraspodele.SelectedDate != null)
            {
                DateTime odabranDatumIVreme = PodesiDatumIVremePreraspodele();
                int IdPrveProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text);
                int IdDrugeProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text);
                double trajanjePreraspodele = 60;
                if ((ProstorijeKontroler.GetInstance().DaLiJeSLobodnaProstorija(IdPrveProstorije, odabranDatumIVreme, trajanjePreraspodele)) &&
                    ((ProstorijeKontroler.GetInstance().DaLiJeSLobodnaProstorija(IdDrugeProstorije, odabranDatumIVreme, trajanjePreraspodele))))
                {
                    preraspodela = new ZakazanaPreraspodelaStatickeOpremeDTO(ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text, ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text, odabranDatumIVreme, 60, NazivOpremeZakazivanje.Text, Int32.Parse(KolicinaOpremeZakazivanje.Text));
                    ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().ZakaziPreraspodeluStatickeOpreme(preraspodela);
                }
                else if (ProstorijeKontroler.GetInstance().DaLiJeSLobodnaProstorija(IdPrveProstorije, odabranDatumIVreme, trajanjePreraspodele))
                {
                    MessageBox.Show("Prostorija u koju želite da prebacite opremu je zauzeta u to vreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Prostorija iz koje želite da prebacite opremu je zauzeta u to vreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Označite datum preraspodele opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OtkaziPrebacivanjeOpreme_Click(object sender, RoutedEventArgs e)
        {
            if (TabelaZakazanihPrebacivanjaOpreme.SelectedIndex != -1)
                ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().OtkaziPreraspodeluStatickeOpreme(TabelaZakazanihPrebacivanjaOpreme.SelectedIndex);
            else
                MessageBox.Show("Označite preraspodelu iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ZakaziRenoviranje_Button_Click(object sender, RoutedEventArgs e)
        {
            if (BrojProstorijeRenoviranje.SelectedIndex == -1 || DatumPocetkaRenoviranja.SelectedDate == null || DatumZavrsetkaRenoviranja.SelectedDate == null)
                MessageBox.Show("Niste uneli sve potrebnje podatke (broj prostorije ili datume) !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else 
            {
                String Broj = ProstorijeKontroler.GetInstance().GetAllProstorije()[BrojProstorijeRenoviranje.SelectedIndex].BrojSobe_;
                DateTime DatumPocetka = (DateTime)DatumPocetkaRenoviranja.SelectedDate;
                DateTime DatumKraja = (DateTime)DatumZavrsetkaRenoviranja.SelectedDate;
                RenoviranjeDTO renoviranje = new RenoviranjeDTO(Broj, DatumPocetka, DatumKraja);
                ProstorijeKontroler.GetInstance().RenovirajProstoriju(renoviranje);
            }
        }

        private void OtkaziRenoviranje_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TabelaRenoviranja.SelectedIndex != -1)
                ProstorijeKontroler.GetInstance().ZavrsiRenoviranje(TabelaRenoviranja.SelectedIndex);
            else
                MessageBox.Show("Označite renoviranje iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajLek(object sender, RoutedEventArgs e)
        {
            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO(VrstaLeka.SelectedIndex, KolicinaLeka.Text, NazivLeka.Text, KlasaLeka.SelectedIndex, JacinaLeka.Text, ZamenskiLek.Text, SastavLeka.Text);
            LekDTO lekZaDodavanje;
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                lekZaDodavanje = new LekDTO(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka),Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
                LekKontroler.GetInstance().DodajLek(lekZaDodavanje);
            }
        }

        private void ObrisiLek(object sender, RoutedEventArgs e)
        {
            if (TabelaLekova.SelectedIndex != -1)
            {
                LekKontroler.GetInstance().IzbrisiLek(TabelaLekova.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Označite lek koji želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IzmeniLek(object sender, RoutedEventArgs e)
        {
            if (TabelaLekovaIzmeni.SelectedIndex != -1)
            {
                LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO(VrstaLekaIzmeni.SelectedIndex, KolicinaLekaIzmeni.Text, NazivLekaIzmeni.Text, KlasaLekaIzmeni.SelectedIndex, JacinaLekaIzmeni.Text, ZamenskiLekIzmeni.Text, SastavLekaIzmeni.Text);
                LekDTO lekZaIzmenu;
                if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "izmeni"))
                {
                    lekZaIzmenu = new LekDTO(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka), Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
                    LekKontroler.GetInstance().IzmeniLek(TabelaLekovaIzmeni.SelectedIndex, lekZaIzmenu);
                }
            }
            else
            {
                MessageBox.Show("Označite lek koji želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabelaLekovaIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaLekovaIzmeni.SelectedIndex != -1)
            {
                LekKontroler.GetInstance().NamapirajInfoOLeku(TabelaLekovaIzmeni.SelectedIndex);
                LekKontroler.GetInstance().NamapirajVrstuLeka(TabelaLekovaIzmeni.SelectedIndex);
                LekKontroler.GetInstance().NamapirajKlasuLeka(TabelaLekovaIzmeni.SelectedIndex);
            }
        }
        
        private void PosaljiLekNaProveru(object sender, RoutedEventArgs e)
        {
            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO(VrstaLeka.SelectedIndex, KolicinaLeka.Text, NazivLeka.Text, KlasaLeka.SelectedIndex, JacinaLeka.Text, ZamenskiLek.Text, SastavLeka.Text);
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                String infoOLeku = "Vrsta:" + VrstaLekaLepIspis(lekZaValidaciju.VrstaLeka) + " Jačina:" + lekZaValidaciju.JacinaLeka + " mg" + 
                                   " Zamenski lek:" + lekZaValidaciju.ZamenskiLek + " Sastav:" + lekZaValidaciju.SastavLeka;
                VerifikacijaLekaDTO verifikacija = new VerifikacijaLekaDTO(DateTime.Now, lekZaValidaciju.NazivLeka, infoOLeku, "1903999772025", GetJmbgLekaraZaValidaciju(LekariLekovi.SelectedIndex), Napomena.Text);
                VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(verifikacija);
            }
        }

        private String VrstaLekaLepIspis(int index)
        {
            if (index == 0)
                return "kapsula";
            else if (index == 1)
                return "tableta";
            else if (index == 2)
                return "sirup";
            else if (index == 3)
                return "sprej";
            else if (index == 4)
                return "gel";
            else
                return "šumeća tableta";
        }

        private String GetJmbgLekaraZaValidaciju(int index)
        {
            List<LekarDTO> lekari = LekarKontroler.getInstance().GetAll();
            return lekari[index].Jmbg;
        }

        private void TabelaVerifikacija_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            VerifikacijaLekaKontroler.GetInstance().NamapirajSadrzajVerifikacije(TabelaVerifikacija.SelectedIndex);
        }

        private void PretraziOpremu(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostPretrage(NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex))
                ProstorijeKontroler.GetInstance().PretraziProstorijePoOpremi(NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex);
        }

        private void ResetujPretraguOpreme(object sender, RoutedEventArgs e)
        {
            TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = true;
            NazivPretraga.Text = "";
            KolicinaPretraga.Text = "";
            TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAllProstorije();
        }
    }
}
