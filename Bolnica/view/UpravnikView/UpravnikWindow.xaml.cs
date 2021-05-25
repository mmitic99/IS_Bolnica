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
            PocetnaStrana.IsSelected = true;
            ProstorijeKontroler.GetInstance().AzurirajRenoviranjaProstorija();
            ProstorijeKontroler.GetInstance().AzurirajStanjeOpremeAkoJeBiloPrebacivanja();
            PrikaziTabele();
        }

        private void PrikaziTabele()
        {
            OsveziPrikazProstorija();
            OsveziPrikazOpreme();
            OsveziPrikazLekova();
            OsveziPrikazZakazanihPreraspodela();
            OsveziPrikazRenoviranja();
            OsveziPrikazVerifikacijaLeka();
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
                OsveziPrikazProstorija();
                OcistiTextPoljaProstorije();
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
                    OsveziPrikazProstorija();
                    OcistiTextPoljaProstorije();
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
                OsveziPrikazProstorija();
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
                NamapirajProstoriju(TabelaProstorijaIzmeni.SelectedIndex);
            }
        }

        private void ObrisiStatickuOpremuIzMagacina(object sender, RoutedEventArgs e)
        {
            if (TabelaStatickeMagacin.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzMagacina(TabelaStatickeMagacin.SelectedIndex);
                OsveziPrikazOpreme();
            }
            else
                MessageBox.Show("Označite statičku opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajStatickuOpremuUMagacin(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivStatickeOpreme.Text, KolicinaStatickeOpreme.Text))
            {
                ProstorijeKontroler.GetInstance().DodajStacionarnuOpremuUMagacin(NazivStatickeOpreme.Text, Int32.Parse(KolicinaStatickeOpreme.Text));
                OsveziPrikazOpreme();
                OcistiTextPoljaStatickeOpremeUMagacinu();
            }
        }

        private void ObrisiPotrosnuOpremuIzMagacina(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacin.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiPotrosnuOpremuIzMagacina(TabelaDinamickeMagacin.SelectedIndex);
                OsveziPrikazOpreme();
            }
            else
                MessageBox.Show("Označite potrošnu opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajPotrosnuOpremuUMagacin(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivDinamickeOpreme.Text, KolicinaDinamickeOpreme.Text))
            {
                ProstorijeKontroler.GetInstance().DodajPotrosnuOpremuUMagacin(NazivDinamickeOpreme.Text, Int32.Parse(KolicinaDinamickeOpreme.Text));
                OsveziPrikazOpreme();
                OcistiTextPoljaDinamickeOpremeUMagacinu();
            }
        }

        private void IzmeniStatickuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaStatickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaStatickeOpremeIzmeni.Text))
                {
                    ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuUMagacinu(TabelaStatickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaStatickeOpremeIzmeni.Text));
                    OsveziPrikazOpreme();
                    OcistiTextPoljaStatickeOpremeUMagacinu();
                }
            }
        }

        private void IzmeniPotrosnuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaDinamickeOpremeIzmeni.Text))
                {
                    ProstorijeKontroler.GetInstance().IzmeniDinamickuOpremuUMagacinu(TabelaDinamickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaDinamickeOpremeIzmeni.Text));
                    OsveziPrikazOpreme();
                    OcistiTextPoljaDinamickeOpremeUMagacinu();
                }
            }
        }
        private void TabelaStatickeMagacinIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaStatickeMagacinIzmeni.SelectedIndex != -1)
                NamapirajStatickuOpremuMagacina(TabelaStatickeMagacinIzmeni.SelectedIndex);
        }

        private void TabelaDinamickeMagacinIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
                NamapirajDinamickuOpremuMagacina(TabelaDinamickeMagacinIzmeni.SelectedIndex);
        }

        private void PrikaziProzorZaSelektovanuProstoriju(object sender, RoutedEventArgs e)
        {
            if ((TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex))
            {
                TabelaOpremeIzKojeSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex);
                TabelaOpremeUKojuSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
                ProstorijaIzKojeSePrebacujeLabel.Content = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe
                    + " - " + VrstaProstorijeLepIspis(ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].VrstaProstorije);
                ProstorijaUKojuSePrebacujeLabel.Content = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe
                    + " - " + VrstaProstorijeLepIspis(ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].VrstaProstorije);
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
                    if (ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].Staticka[TabelaOpremeIzKojeSePrebacuje.SelectedIndex].Kolicina - Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text) >= 0)
                    {
                        ZakazanaPrebacivanjaOpremeTab.IsEnabled = true;
                        ZakazanaPrebacivanjaOpremeTab.IsSelected = true;
                        ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe;
                        ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe;
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
                {
                    PrebacivanjeOpremeInfoDTO prebacivanjeInfo = new PrebacivanjeOpremeInfoDTO
                                                                     (
                                                                     TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex,
                                                                     TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex,
                                                                     NazivOpremeSKojomSeRadi_Copy.Text,
                                                                     Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text)
                                                                     );
                    ProstorijeKontroler.GetInstance().PrebaciStacionarnuOpremuUProstoriju(prebacivanjeInfo);
                    OsveziPrikazOpreme();
                    OsveziPrikazTabelaOpreme(prebacivanjeInfo.IndexIzKojeProstorije, prebacivanjeInfo.IndexUKojuProstoriju);
                }
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz leve kolone koju želite da prebacite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IzmeniStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            IzmenaOpremeInfoDTO izmenaOpremeInfo = new IzmenaOpremeInfoDTO
                                                       (
                                                       TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex, 
                                                       TabelaOpremeUKojuSePrebacuje.SelectedIndex, 
                                                       Int32.Parse(KolicinaOpremeSKojomSeRadi.Text)
                                                       );
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaOpremeSKojomSeRadi.Text))
                {
                    ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuProstorije(izmenaOpremeInfo);
                    OsveziPrikazOpreme();
                    OsveziPrikazTabelaOpreme(-1, izmenaOpremeInfo.IndexProstorije);
                }
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ObrisiStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex, TabelaOpremeUKojuSePrebacuje.SelectedIndex);
                OsveziPrikazOpreme();
                OsveziPrikazTabelaOpreme(-1, TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da obrišete iz prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabelaOpremeIzKojeSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeIzKojeSePrebacuje.SelectedIndex != -1)
                NamapirajOpremuZaPrebacivanjeIzProstorije(TabelaOpremeIzKojeSePrebacuje.SelectedIndex, TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex);
        }

        private void TabelaOpremeUKojuSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
                NamapirajOpremuZaPrebacivanjeUProstoriju(TabelaOpremeUKojuSePrebacuje.SelectedIndex, TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
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
                    preraspodela = new ZakazanaPreraspodelaStatickeOpremeDTO
                                       (
                                       ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text, 
                                       ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text, 
                                       odabranDatumIVreme, 
                                       trajanjePreraspodele, 
                                       NazivOpremeZakazivanje.Text, 
                                       Int32.Parse(KolicinaOpremeZakazivanje.Text)
                                       );
                    ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().ZakaziPreraspodeluStatickeOpreme(preraspodela);
                    OsveziPrikazPreraspodeleOpreme();
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
            {
                ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().OtkaziPreraspodeluStatickeOpreme(TabelaZakazanihPrebacivanjaOpreme.SelectedIndex);
                OsveziPrikazPreraspodeleOpreme();
            }
            else
                MessageBox.Show("Označite preraspodelu iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ZakaziRenoviranje_Button_Click(object sender, RoutedEventArgs e)
        {
            if (BrojProstorijeRenoviranje.SelectedIndex == -1 || DatumPocetkaRenoviranja.SelectedDate == null || DatumZavrsetkaRenoviranja.SelectedDate == null)
                MessageBox.Show("Niste uneli sve potrebnje podatke (broj prostorije ili datume) !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else 
            {
                String Broj = ProstorijeKontroler.GetInstance().GetAll()[BrojProstorijeRenoviranje.SelectedIndex].BrojSobe;
                DateTime DatumPocetka = (DateTime)DatumPocetkaRenoviranja.SelectedDate;
                DateTime DatumKraja = (DateTime)DatumZavrsetkaRenoviranja.SelectedDate;
                RenoviranjeDTO renoviranje = new RenoviranjeDTO(Broj, DatumPocetka, DatumKraja);
                ProstorijeKontroler.GetInstance().RenovirajProstoriju(renoviranje);
                OsveziPrikazRenoviranja();
                OcistiTextPoljaRenoviranja();
            }
        }

        private void OtkaziRenoviranje_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TabelaRenoviranja.SelectedIndex != -1)
            {
                ProstorijeKontroler.GetInstance().ZavrsiRenoviranje(TabelaRenoviranja.SelectedIndex);
                OsveziPrikazRenoviranja();
                OsveziPrikazProstorija();
            }
            else
                MessageBox.Show("Označite renoviranje iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajLek(object sender, RoutedEventArgs e)
        {
            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO
                                                   (
                                                   VrstaLeka.SelectedIndex, 
                                                   KolicinaLeka.Text, 
                                                   NazivLeka.Text, 
                                                   KlasaLeka.SelectedIndex, 
                                                   JacinaLeka.Text, 
                                                   ZamenskiLek.Text, 
                                                   SastavLeka.Text
                                                   );
            LekDTO lekZaDodavanje;
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                lekZaDodavanje = new LekDTO
                                     (
                                     LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), 
                                     Int64.Parse(lekZaValidaciju.KolicinaLeka), 
                                     lekZaValidaciju.NazivLeka, 
                                     LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka),
                                     Int32.Parse(lekZaValidaciju.JacinaLeka), 
                                     lekZaValidaciju.ZamenskiLek, 
                                     lekZaValidaciju.SastavLeka
                                     );
                LekKontroler.GetInstance().DodajLek(lekZaDodavanje);
                OsveziPrikazLekova();
                OcistiTextPoljaLekova();
            }
        }

        private void ObrisiLek(object sender, RoutedEventArgs e)
        {
            if (TabelaLekova.SelectedIndex != -1)
            {
                LekKontroler.GetInstance().IzbrisiLek(TabelaLekova.SelectedIndex);
                OsveziPrikazLekova();
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
                LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO
                                                       (
                                                       VrstaLekaIzmeni.SelectedIndex, 
                                                       KolicinaLekaIzmeni.Text, 
                                                       NazivLekaIzmeni.Text, 
                                                       KlasaLekaIzmeni.SelectedIndex, 
                                                       JacinaLekaIzmeni.Text, 
                                                       ZamenskiLekIzmeni.Text, 
                                                       SastavLekaIzmeni.Text
                                                       );
                LekDTO lekZaIzmenu;
                if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "izmeni"))
                {
                    lekZaIzmenu = new LekDTO
                                      (
                                      LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), 
                                      Int64.Parse(lekZaValidaciju.KolicinaLeka), 
                                      lekZaValidaciju.NazivLeka, 
                                      LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka), 
                                      Int32.Parse(lekZaValidaciju.JacinaLeka), 
                                      lekZaValidaciju.ZamenskiLek, 
                                      lekZaValidaciju.SastavLeka
                                      );
                    LekKontroler.GetInstance().IzmeniLek(TabelaLekovaIzmeni.SelectedIndex, lekZaIzmenu);
                    OsveziPrikazLekova();
                    OcistiTextPoljaLekova();
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
                NamapirajInfoOLeku(TabelaLekovaIzmeni.SelectedIndex);
                NamapirajVrstuLeka(TabelaLekovaIzmeni.SelectedIndex);
                NamapirajKlasuLeka(TabelaLekovaIzmeni.SelectedIndex);
            }
        }
        
        private void PosaljiLekNaProveru(object sender, RoutedEventArgs e)
        {
            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO
                                                   (
                                                   VrstaLeka.SelectedIndex, 
                                                   KolicinaLeka.Text, 
                                                   NazivLeka.Text, 
                                                   KlasaLeka.SelectedIndex, 
                                                   JacinaLeka.Text, 
                                                   ZamenskiLek.Text, 
                                                   SastavLeka.Text
                                                   );
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                String infoOLeku = "Vrsta:" + VrstaLekaLepIspis(lekZaValidaciju.VrstaLeka) + " Jačina:" + lekZaValidaciju.JacinaLeka + " mg" + 
                                   " Zamenski lek:" + lekZaValidaciju.ZamenskiLek + " Sastav:" + lekZaValidaciju.SastavLeka;
                VerifikacijaLekaDTO verifikacija = new VerifikacijaLekaDTO
                                                       (
                                                       DateTime.Now, 
                                                       lekZaValidaciju.NazivLeka, 
                                                       infoOLeku, 
                                                       "1903999772025", 
                                                       GetJmbgLekaraZaValidaciju(LekariLekovi.SelectedIndex), 
                                                       Napomena.Text
                                                       );
                VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(verifikacija);
                MessageBox.Show("Lek uspešno poslat lekaru na verifikaciju.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
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
            NamapirajSadrzajVerifikacije(TabelaVerifikacija.SelectedIndex);
        }

        private void PretraziOpremu(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostPretrage(NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex))
            {
                TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().PretraziProstorijePoOpremi
                                                                     (NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex);
                TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = false;
            }
        }

        private void ResetujPretraguOpreme(object sender, RoutedEventArgs e)
        {
            TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = true;
            NazivPretraga.Text = "";
            KolicinaPretraga.Text = "";
            TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
        }

        private void NamapirajDinamickuOpremuMagacina(int index)
        {
            List<PotrosnaOpremaDTO> PotrosnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna;
            NazivDinamickeOpremeIzmeni.Text = PotrosnaMagacin[index].TipOpreme;
            KolicinaDinamickeOpremeIzmeni.Text = PotrosnaMagacin[index].KolicinaOpreme.ToString();
        }

        private void NamapirajOpremuZaPrebacivanjeIzProstorije(int indexOpreme, int indexProstorije)
        {
            List<StacionarnaOpremaDTO> StacionarnaOpremaOdKojeSeUzima = ProstorijeKontroler.GetInstance().GetAll()[indexProstorije].Staticka;
            NazivOpremeSKojomSeRadi_Copy.Text = StacionarnaOpremaOdKojeSeUzima[indexOpreme].TipStacionarneOpreme;
        }

        private void NamapirajOpremuZaPrebacivanjeUProstoriju(int indexOpreme, int indexProstorije)
        {
            List<StacionarnaOpremaDTO> StacionarnaOpremaUKojuSeDodaje = ProstorijeKontroler.GetInstance().GetAll()[indexProstorije].Staticka;
            NazivOpremeSKojomSeRadi.Text = StacionarnaOpremaUKojuSeDodaje[indexOpreme].TipStacionarneOpreme;
        }

        public void NamapirajStatickuOpremuMagacina(int index)
        {
            List<StacionarnaOpremaDTO> StacionarnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Staticka;
            NazivStatickeOpremeIzmeni.Text = StacionarnaMagacin[index].TipStacionarneOpreme;
            KolicinaStatickeOpremeIzmeni.Text = StacionarnaMagacin[index].Kolicina.ToString();
        }

        public void NamapirajSadrzajVerifikacije(int index)
        {
            List<VerifikacijaLekaDTO> SveVerifikacijeLekova = VerifikacijaLekaKontroler.GetInstance().GetAll();
            Sadrzaj.Text = SveVerifikacijeLekova[index].Sadrzaj;
        }

        public void NamapirajVrstuLeka(int index)
        {
            List<LekDTO> SviLekovi = LekKontroler.GetInstance().GetAll();
            if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Gel)
            {
                VrstaLekaIzmeni.Text = "Gel";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Kapsula)
            {
                VrstaLekaIzmeni.Text = "Kapsula";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Sirup)
            {
                VrstaLekaIzmeni.Text = "Sirup";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Sprej)
            {
                VrstaLekaIzmeni.Text = "Sprej";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.SumecaTableta)
            {
                VrstaLekaIzmeni.Text = "Šumeća tableta";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Tableta)
            {
                VrstaLekaIzmeni.Text = "Tableta";
            }
        }

        public void NamapirajKlasuLeka(int index)
        {
            List<LekDTO> SviLekovi = LekKontroler.GetInstance().GetAll();
            if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Analgetik)
            {
                KlasaLekaIzmeni.Text = "Analgetik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antibiotik)
            {
                KlasaLekaIzmeni.Text = "Antibiotik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antimalarijski_Lek)
            {
                KlasaLekaIzmeni.Text = "Antimalarijski lek";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antipiretik)
            {
                KlasaLekaIzmeni.Text = "Antipiretik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antiseptik)
            {
                KlasaLekaIzmeni.Text = "Antiseptik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Hormonska_Zamena)
            {
                KlasaLekaIzmeni.Text = "Hormonska zamena";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Oralni_Kontraceptiv)
            {
                KlasaLekaIzmeni.Text = "Oralni kontraceptiv";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Stabilizator_Raspolozenja)
            {
                KlasaLekaIzmeni.Text = "Stabilizator raspoloženja";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Statin)
            {
                KlasaLekaIzmeni.Text = "Statin";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Stimulant)
            {
                KlasaLekaIzmeni.Text = "Stimulant";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Trankvilajzer)
            {
                KlasaLekaIzmeni.Text = "Trankvilajzer";
            }
        }

        public void NamapirajInfoOLeku(int index)
        {
            List<LekDTO> SviLekovi = LekKontroler.GetInstance().GetAll();
            NazivLekaIzmeni.Text = SviLekovi[index].NazivLeka;
            JacinaLekaIzmeni.Text = SviLekovi[index].JacinaLeka.ToString();
            KolicinaLekaIzmeni.Text = SviLekovi[index].KolicinaLeka.ToString();
            ZamenskiLekIzmeni.Text = SviLekovi[index].ZamenskiLek;
            SastavLekaIzmeni.Text = SviLekovi[index].SastavLeka;
        }

        private void NamapirajProstoriju(int index)
        {
            List<ProstorijaDTO> ListaProstorija = ProstorijeKontroler.GetInstance().GetAll();
            BrojProstorijeTextBoxIzmeni.Text = ListaProstorija[index].BrojSobe;
            SpratTextBoxIzmeni.Text = ListaProstorija[index].Sprat.ToString();
            KvadraturaTextBoxIzmeni.Text = ListaProstorija[index].Kvadratura.ToString();

            if (ListaProstorija[index].VrstaProstorije == Model.Enum.VrstaProstorije.Soba_za_preglede)
            {
                VrstaProstorijeComboBoxIzmeni.Text = "Soba za preglede";
            }
            else if (ListaProstorija[index].VrstaProstorije == Model.Enum.VrstaProstorije.Operaciona_sala)
            {
                VrstaProstorijeComboBoxIzmeni.Text = "Operaciona sala";
            }
            else if (ListaProstorija[index].VrstaProstorije == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
            {
                VrstaProstorijeComboBoxIzmeni.Text = "Soba za bolesnike";
            }
            else if (ListaProstorija[index].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
            {
                VrstaProstorijeComboBoxIzmeni.Text = "Magacin";
            }
        }

            private void OsveziPrikazProstorija()
            {
                TabelaProstorija.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
                TabelaProstorijaIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
                TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
                TabelaProstorijeUKojuSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
                BrojProstorijeRenoviranje.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
            }

            private void OsveziPrikazOpreme()
            {
                TabelaStatickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Staticka;
                TabelaStatickeMagacinIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Staticka;
                TabelaDinamickeMagacinIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna;
                TabelaDinamickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna;
            }

            private void OsveziPrikazLekova()
            {
                TabelaLekova.ItemsSource = LekKontroler.GetInstance().GetAll();
                TabelaLekovaIzmeni.ItemsSource = LekKontroler.GetInstance().GetAll();
            }

            private void OsveziPrikazRenoviranja()
            {
                TabelaRenoviranja.ItemsSource = ProstorijeKontroler.GetInstance().GetAllRenoviranja();
            }

            private void OsveziPrikazVerifikacijaLeka()
            {
                TabelaVerifikacija.ItemsSource = VerifikacijaLekaKontroler.GetInstance().GetObavestenjaByJmbg("1903999772025");
                LekariLekovi.ItemsSource = LekarKontroler.getInstance().GetAll();
                LekariLekoviIzmeni.ItemsSource = LekarKontroler.getInstance().GetAll();
            }

            private void OsveziPrikazZakazanihPreraspodela()
            {
                TabelaZakazanihPrebacivanjaOpreme.ItemsSource = ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().GetAll();
            }
            private void OcistiTextPoljaProstorije()
            {
                BrojProstorijeTextBox.Clear();
                SpratTextBox.Clear();
                KvadraturaTextBox.Clear();
                BrojProstorijeTextBoxIzmeni.Clear();
                SpratTextBoxIzmeni.Clear();
                KvadraturaTextBoxIzmeni.Clear();
            }

            private void OcistiTextPoljaRenoviranja()
            {
                BrojProstorijeRenoviranje.Text = "";
                DatumZavrsetkaRenoviranja.Text = "";
                DatumPocetkaRenoviranja.Text = "";

            }

            private void OcistiTextPoljaStatickeOpremeUMagacinu()
            {
                KolicinaStatickeOpreme.Clear();
                KolicinaStatickeOpremeIzmeni.Clear();
                NazivStatickeOpreme.Clear();
                NazivStatickeOpremeIzmeni.Clear();
            }

            private void OcistiTextPoljaDinamickeOpremeUMagacinu()
            {
                KolicinaDinamickeOpreme.Clear();
                KolicinaDinamickeOpremeIzmeni.Clear();
                NazivDinamickeOpreme.Clear();
                NazivDinamickeOpremeIzmeni.Clear();
            }

            private void OcistiTextPoljaLekova() { }

            private void OsveziPrikazTabelaOpreme(int indexIzKojeProstorije, int indexUKojuProstoriju)
            {
                if (indexIzKojeProstorije != -1)
                    TabelaOpremeIzKojeSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetAll()[indexIzKojeProstorije].Staticka;
                TabelaOpremeUKojuSePrebacuje.ItemsSource = ProstorijeKontroler.GetInstance().GetAll()[indexUKojuProstoriju].Staticka;
            }

            private void OsveziPrikazPreraspodeleOpreme()
            {
                TabelaZakazanihPrebacivanjaOpreme.ItemsSource = ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().GetAll();
            }

        private void OdjavaTab_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("u");
            this.Close();
            s.Show();
        }
    }
}
