using Bolnica.DTO;
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

namespace Bolnica.view.UpravnikView
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        Upravnik upravnik;
        public List<StacionarnaOprema> StacionarnaMagacin { get; set; }
        public List<StacionarnaOprema> StacionarnaOpremaOdKojeSeUzima { get; set; }
        public List<StacionarnaOprema> StacionarnaOpremaUKojuSeDodaje { get; set; }
        public List<PotrosnaOprema> PotrosnaMagacin { get; set; }
        public List<Prostorija> ListaProstorija { get; set; }
        public List<ZakazanaPreraspodelaStatickeOpreme> PreraspodeleStatickeOpreme { get; set; }
        public List<Lek> SviLekovi { get; set; }
        public List<VerifikacijaLeka> SveVerifikacijeLekova { get; set; }

        public List<Renoviranje> SvaRenoviranja { get; set; }
        public List<Prostorija> PretrazeneProstorije { get; set; }

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
            ListaProstorija = new List<Prostorija>();
            PretrazeneProstorije = new List<Prostorija>();
            StacionarnaMagacin = new List<StacionarnaOprema>();
            PotrosnaMagacin = new List<PotrosnaOprema>();
            StacionarnaOpremaOdKojeSeUzima = new List<StacionarnaOprema>();
            StacionarnaOpremaUKojuSeDodaje = new List<StacionarnaOprema>();
            PreraspodeleStatickeOpreme = new List<ZakazanaPreraspodelaStatickeOpreme>();
            SvaRenoviranja = new List<Renoviranje>();
            SviLekovi = new List<Lek>();
            SveVerifikacijeLekova = new List<VerifikacijaLeka>();            
            ListaProstorija = ProstorijeKontroler.GetInstance().GetAll();
            StacionarnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Staticka_;
            PotrosnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna_;
            PreraspodeleStatickeOpreme = SkladisteZaZakazanuPreraspodeluStatickeOpreme.GetInstance().GetAll();
            SvaRenoviranja = SkladisteZaRenoviranja.GetInstance().GetAll();
            SviLekovi = SkladisteZaLekove.GetInstance().GetAll();
            SveVerifikacijeLekova = SkladisteZaVerifikacijuLeka.GetInstance().GetObavestenjaByJmbg("1903999772025");
            BrojProstorijeRenoviranje.ItemsSource = ListaProstorija;
            LekariLekovi.ItemsSource = SkladisteZaLekara.GetInstance().GetAll();
            LekariLekoviIzmeni.ItemsSource = SkladisteZaLekara.GetInstance().GetAll();

            ProstorijeKontroler.GetInstance().AzurirajRenoviranjaProstorija();
            ProstorijeKontroler.GetInstance().AzurirajStanjeOpremeAkoJeBiloPrebacivanja();
            ListaProstorija = SkladisteZaProstorije.GetInstance().GetAll();
        }

        // metoda za dodavanje nove prostorije
        private void DodajProstoriju(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostProstorije(BrojProstorijeTextBox.Text, SpratTextBox.Text, VrstaProstorijeComboBox.SelectedIndex, KvadraturaTextBox.Text))
            {
                Prostorija p = new Prostorija(BrojProstorijeTextBox.Text, Int32.Parse(SpratTextBox.Text), ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBox.SelectedIndex), Double.Parse(KvadraturaTextBox.Text));
                ProstorijeKontroler.GetInstance().DodajProstoriju(p);
            }
        }

        // metoda za izmenu selektovane prostorije
        private void IzmeniProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaProstorijaIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostIzmeneProstorije(BrojProstorijeTextBoxIzmeni.Text, SpratTextBoxIzmeni.Text, VrstaProstorijeComboBoxIzmeni.SelectedIndex, KvadraturaTextBoxIzmeni.Text))
                {
                    Prostorija p = new Prostorija(BrojProstorijeTextBoxIzmeni.Text, Int32.Parse(SpratTextBoxIzmeni.Text), ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBoxIzmeni.SelectedIndex), Double.Parse(KvadraturaTextBoxIzmeni.Text));
                    ProstorijeKontroler.GetInstance().IzmeniProstoriju(TabelaProstorijaIzmeni.SelectedIndex, p);
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
                int indexSelektovaneProstorije = TabelaProstorijaIzmeni.SelectedIndex;
                BrojProstorijeTextBoxIzmeni.Text = ListaProstorija[indexSelektovaneProstorije].BrojSobe_;
                SpratTextBoxIzmeni.Text = ListaProstorija[indexSelektovaneProstorije].Sprat_.ToString();
                KvadraturaTextBoxIzmeni.Text = ListaProstorija[indexSelektovaneProstorije].Kvadratura_.ToString();

                if (ListaProstorija[indexSelektovaneProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_preglede)
                {
                    VrstaProstorijeComboBoxIzmeni.Text = "Soba za preglede";
                }
                else if (ListaProstorija[indexSelektovaneProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Operaciona_sala)
                {
                    VrstaProstorijeComboBoxIzmeni.Text = "Operaciona sala";
                }
                else if (ListaProstorija[indexSelektovaneProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
                {
                    VrstaProstorijeComboBoxIzmeni.Text = "Soba za bolesnike";
                }
                else if (ListaProstorija[indexSelektovaneProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    VrstaProstorijeComboBoxIzmeni.Text = "Magacin";
                }
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
            {
                int indexSelektovaneOpreme = TabelaStatickeMagacinIzmeni.SelectedIndex;
                NazivStatickeOpremeIzmeni.Text = StacionarnaMagacin[indexSelektovaneOpreme].TipStacionarneOpreme_;
                KolicinaStatickeOpremeIzmeni.Text = StacionarnaMagacin[indexSelektovaneOpreme].Kolicina_.ToString();
            }
        }

        private void TabelaDinamickeMagacinIzmeni_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
            {
                int indexSelektovaneOpreme = TabelaDinamickeMagacinIzmeni.SelectedIndex;
                NazivDinamickeOpremeIzmeni.Text = PotrosnaMagacin[indexSelektovaneOpreme].TipOpreme_;
                KolicinaDinamickeOpremeIzmeni.Text = PotrosnaMagacin[indexSelektovaneOpreme].KolicinaOpreme_.ToString();
            }
        }

        private void PrikaziProzorZaSelektovanuProstoriju(object sender, RoutedEventArgs e)
        {
            if ((TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex != -1) && (TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex != TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex))
            {
                StacionarnaOpremaOdKojeSeUzima = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex);
                StacionarnaOpremaUKojuSeDodaje = ProstorijeKontroler.GetInstance().GetStacionarnaOpremaProstorije(TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex);
                TabelaOpremeUKojuSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(StacionarnaOpremaUKojuSeDodaje);
                TabelaOpremeIzKojeSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(StacionarnaOpremaOdKojeSeUzima);
                ProstorijaIzKojeSePrebacujeLabel.Content = ListaProstorija[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe_
                    + " - " + VrstaProstorijeLepIspis(ListaProstorija[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].VrstaProstorije);
                ProstorijaUKojuSePrebacujeLabel.Content = ListaProstorija[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe_
                    + " - " + VrstaProstorijeLepIspis(ListaProstorija[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].VrstaProstorije_);
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
                    if (ListaProstorija[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].Staticka_[TabelaOpremeIzKojeSePrebacuje.SelectedIndex].Kolicina_ - Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text) >= 0)
                    {
                        ZakazanaPrebacivanjaOpremeTab.IsEnabled = true;
                        ZakazanaPrebacivanjaOpremeTab.IsSelected = true;
                        ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text = ListaProstorija[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe_;
                        ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text = ListaProstorija[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe_;
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
                //NazivOpremeSKojomSeRadi_Copy.Clear();
                //KolicinaOpremeSKojomSeRadi_Copy.Clear();
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
            {
                int indexSelektovaneOpreme = TabelaOpremeIzKojeSePrebacuje.SelectedIndex;
                NazivOpremeSKojomSeRadi_Copy.Text = StacionarnaOpremaOdKojeSeUzima[indexSelektovaneOpreme].TipStacionarneOpreme_;
            }
        }

        private void TabelaOpremeUKojuSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
            {
                int indexSelektovaneOpreme = TabelaOpremeUKojuSePrebacuje.SelectedIndex;
                NazivOpremeSKojomSeRadi.Text = StacionarnaOpremaUKojuSeDodaje[indexSelektovaneOpreme].TipStacionarneOpreme_;
            }
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

        private void ZakaziPrebacivanjeOpreme_Click(object sender, RoutedEventArgs e)
        {
            ZakazanaPreraspodelaStatickeOpreme preraspodela = new ZakazanaPreraspodelaStatickeOpreme();
            if (DatumPreraspodele.SelectedDate != null)
            {
                DateTime datumVreme = (DateTime)DatumPreraspodele.SelectedDate;
                int sati = (int)Sat.SelectedIndex + 6;
                int minuti = 30;
                if (Minut.SelectedIndex == 0)
                {
                    minuti = 0;
                }
                DateTime odabranDatumIVreme = new DateTime(datumVreme.Year, datumVreme.Month, datumVreme.Day, sati, minuti, 0);
                int IdPrveProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text);
                int IdDrugeProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text);
                double trajanjePreraspodele = 60;
                if ((ProstorijeKontroler.GetInstance().DaLiJeSLobodnaProstorija(IdPrveProstorije, odabranDatumIVreme, trajanjePreraspodele)) &&
                    ((ProstorijeKontroler.GetInstance().DaLiJeSLobodnaProstorija(IdDrugeProstorije, odabranDatumIVreme, trajanjePreraspodele))))
                {
                    preraspodela = new ZakazanaPreraspodelaStatickeOpreme(ZakazivanjeProstorijaKojaGubiOpremuTextBox.Text, ZakazivanjeProstorijaKojaDobijaOpremuTextBox.Text, odabranDatumIVreme, 60, NazivOpremeZakazivanje.Text, Int32.Parse(KolicinaOpremeZakazivanje.Text));
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
                String Broj = ((Prostorija)BrojProstorijeRenoviranje.SelectedItem).BrojSobe_;
                DateTime DatumPocetka = (DateTime)DatumPocetkaRenoviranja.SelectedDate;
                DateTime DatumKraja = (DateTime)DatumZavrsetkaRenoviranja.SelectedDate;
                ProstorijeKontroler.GetInstance().RenovirajProstoriju(Broj, DatumPocetka, DatumKraja);
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
            LekDTO lekZaValidaciju = new LekDTO(VrstaLeka.SelectedIndex, KolicinaLeka.Text, NazivLeka.Text, KlasaLeka.SelectedIndex, JacinaLeka.Text, ZamenskiLek.Text, SastavLeka.Text);
            Lek lekZaDodavanje;
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                lekZaDodavanje = new Lek(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka),Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
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
                LekDTO lekZaValidaciju = new LekDTO(VrstaLekaIzmeni.SelectedIndex, KolicinaLekaIzmeni.Text, NazivLekaIzmeni.Text, KlasaLekaIzmeni.SelectedIndex, JacinaLekaIzmeni.Text, ZamenskiLekIzmeni.Text, SastavLekaIzmeni.Text);
                Lek lekZaIzmenu;
                if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "izmeni"))
                {
                    lekZaIzmenu = new Lek(LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka), Int64.Parse(lekZaValidaciju.KolicinaLeka), lekZaValidaciju.NazivLeka, LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka), Int32.Parse(lekZaValidaciju.JacinaLeka), lekZaValidaciju.ZamenskiLek, lekZaValidaciju.SastavLeka);
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
                int indexSelektovanogLeka = TabelaLekovaIzmeni.SelectedIndex;
                NazivLekaIzmeni.Text = SviLekovi[indexSelektovanogLeka].NazivLeka;
                JacinaLekaIzmeni.Text = SviLekovi[indexSelektovanogLeka].JacinaLeka.ToString();
                KolicinaLekaIzmeni.Text = SviLekovi[indexSelektovanogLeka].KolicinaLeka.ToString();
                ZamenskiLekIzmeni.Text = SviLekovi[indexSelektovanogLeka].ZamenskiLek;
                SastavLekaIzmeni.Text = SviLekovi[indexSelektovanogLeka].SastavLeka;
                NamapirajVrstuLeka(indexSelektovanogLeka);
                NamapirajKlasuLeka(indexSelektovanogLeka);
            }
        }

        private void NamapirajVrstuLeka(int index)
        {
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

        private void NamapirajKlasuLeka(int index)
        {
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
        private void PosaljiLekNaProveru(object sender, RoutedEventArgs e)
        {
            LekDTO lekZaValidaciju = new LekDTO(VrstaLeka.SelectedIndex, KolicinaLeka.Text, NazivLeka.Text, KlasaLeka.SelectedIndex, JacinaLeka.Text, ZamenskiLek.Text, SastavLeka.Text);
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj"))
            {
                String infoOLeku = "Vrsta:" + VrstaLekaLepIspis(lekZaValidaciju.VrstaLeka) + " Jačina:" + lekZaValidaciju.JacinaLeka + " mg" + 
                                   " Zamenski lek:" + lekZaValidaciju.ZamenskiLek + " Sastav:" + lekZaValidaciju.SastavLeka;
                VerifikacijaLeka verifikacija = new VerifikacijaLeka(DateTime.Now, lekZaValidaciju.NazivLeka, infoOLeku, "1903999772025", GetJmbgLekaraZaValidaciju(LekariLekovi.SelectedIndex), Napomena.Text);
                VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(verifikacija);
                OsveziPrikazVerifikacijaLeka();
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
            List<Lekar> lekari = LekarKontroler.getInstance().GetAll();
            return lekari[index].Jmbg;
        }

        private void TabelaVerifikacija_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Sadrzaj.Text = SveVerifikacijeLekova[TabelaVerifikacija.SelectedIndex].Sadrzaj;
        }

        private void OsveziPrikazVerifikacijaLeka()
        {
            TabelaVerifikacija.ItemsSource = new ObservableCollection<VerifikacijaLeka>(SveVerifikacijeLekova);
        }

        private void PretraziOpremu(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostPretrage(NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex))
            {
                PretrazeneProstorije = ProstorijeKontroler.GetInstance().PretraziProstorijePoOpremi(NazivPretraga.Text, KolicinaPretraga.Text, UpitPretrage.SelectedIndex);
                TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(PretrazeneProstorije);
                TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = false;
            }
        }

        private void ResetujPretraguOpreme(object sender, RoutedEventArgs e)
        {
            TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = true;
            NazivPretraga.Text = "";
            KolicinaPretraga.Text = "";
            TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(ListaProstorija);
        }
    }
}
