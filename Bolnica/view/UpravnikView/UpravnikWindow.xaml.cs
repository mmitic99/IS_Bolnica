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
using System.Text.RegularExpressions;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System.Drawing;
using System.Data;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace Bolnica.view.UpravnikView
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        Upravnik upravnik;

        List<BolnicaDTO> SveBolnice = new List<BolnicaDTO>();
        List<PraksaDTO> SvePrakse = new List<PraksaDTO>();
        private static UpravnikWindow instance = null;
        private bool isDragging = false;

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
            ProstorijeKontroler.GetInstance().AzurirajNaprednaRenoviranjaProstorija();
            PrikaziTabele();
            SveBolnice.Add(new BolnicaDTO("Opsta bolnica - Novi Sad", "Tolstojeva 3", "021-555/6666"));
            SveBolnice.Add(new BolnicaDTO("Opsta bolnica - Veternik", "Nikole Tesle 73", "021-558/6836"));
            SveBolnice.Add(new BolnicaDTO("Laza Lazarevic - Novi Sad", "Balzakova 26", "021-555/9453"));
            SvePrakse.Add(new PraksaDTO("Opšta medicina", DateTime.Now.AddDays(2), DateTime.Now.AddDays(14), "Opšta bolnica - Novi Sad", "60"));
            SvePrakse.Add(new PraksaDTO("Opšta medicina", DateTime.Now, DateTime.Now.AddDays(10), "Opšta bolnica - Veternik", "20"));
            SvePrakse.Add(new PraksaDTO("Opšta medicina", DateTime.Now.AddDays(7), DateTime.Now.AddDays(23), "Bolnica Mileva Marić - Novi Sad", "80"));
            Bolnice.ItemsSource = SveBolnice;
            Prakse.ItemsSource = SvePrakse;
            ImeUpravnika.Header = upravnik.Ime + " " + upravnik.Prezime;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
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

        private void DodajProstoriju(object sender, RoutedEventArgs e)
        {
            ProstorijaValidacijaDTO prostorijaZaValidaciju = new ProstorijaValidacijaDTO
                                                                 (
                                                                 BrojProstorijeTextBox.Text.Trim(),
                                                                 SpratTextBox.Text.Trim(),
                                                                 VrstaProstorijeComboBox.SelectedIndex,
                                                                 KvadraturaTextBox.Text.Trim()
                                                                 );
            if (ProstorijeKontroler.GetInstance().ProveriValidnostProstorije(prostorijaZaValidaciju))
            {
                ProstorijaDTO prostorija = new ProstorijaDTO
                                               (
                                               BrojProstorijeTextBox.Text.Trim(),
                                               Int32.Parse(SpratTextBox.Text.Trim()),
                                               ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBox.SelectedIndex),
                                               Double.Parse(KvadraturaTextBox.Text.Trim())
                                               );
                ProstorijeKontroler.GetInstance().DodajProstoriju(prostorija);
                OsveziPrikazProstorija();
                OcistiTextPoljaProstorije();
            }
        }

        private void IzmeniProstoriju(object sender, RoutedEventArgs e)
        {

            if (TabelaProstorijaIzmeni.SelectedIndex != -1)
            {
                ProstorijaValidacijaDTO prostorijaZaValidaciju = new ProstorijaValidacijaDTO
                                                                     (
                                                                     BrojProstorijeTextBoxIzmeni.Text.Trim(),
                                                                     SpratTextBoxIzmeni.Text.Trim(),
                                                                     VrstaProstorijeComboBoxIzmeni.SelectedIndex,
                                                                     KvadraturaTextBoxIzmeni.Text.Trim()
                                                                     );
                if (ProstorijeKontroler.GetInstance().ProveriValidnostIzmeneProstorije(prostorijaZaValidaciju, TabelaProstorijaIzmeni.SelectedIndex))
                {
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da izmenite prostoriju ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        ProstorijaDTO prostorija = new ProstorijaDTO
                                                   (
                                                   BrojProstorijeTextBoxIzmeni.Text.Trim(),
                                                   Int32.Parse(SpratTextBoxIzmeni.Text.Trim()),
                                                   ProstorijeKontroler.GetInstance().GetVrstuProstorije(VrstaProstorijeComboBoxIzmeni.SelectedIndex),
                                                   Double.Parse(KvadraturaTextBoxIzmeni.Text.Trim())
                                                   );
                        ProstorijeKontroler.GetInstance().IzmeniProstoriju(TabelaProstorijaIzmeni.SelectedIndex, prostorija);
                        OsveziPrikazProstorija();
                        OcistiTextPoljaProstorije();
                    }
                }
            }
            else
            {
                MessageBox.Show("Označite prostoriju koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ObrisiProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaProstorija.SelectedIndex != -1)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete prostoriju ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().IzbrisiProstoriju(TabelaProstorija.SelectedIndex);
                    OsveziPrikazProstorija();
                }
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
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete statičku opremu ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzMagacina(TabelaStatickeMagacin.SelectedIndex);
                    OsveziPrikazOpreme();
                }
            }
            else
                MessageBox.Show("Označite statičku opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajStatickuOpremuUMagacin(object sender, RoutedEventArgs e)
        {
            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivStatickeOpreme.Text.Trim().ToLower(), KolicinaStatickeOpreme.Text.Trim()))
            {
                String naziv = NazivStatickeOpreme.Text.Trim().ToLower();
                naziv = char.ToUpper(naziv[0]) + naziv.Substring(1);
                ProstorijeKontroler.GetInstance().DodajStacionarnuOpremuUMagacin(naziv, Int32.Parse(KolicinaStatickeOpreme.Text.Trim()));
                OsveziPrikazOpreme();
                OcistiTextPoljaStatickeOpremeUMagacinu();
            }
        }

        private void ObrisiPotrosnuOpremuIzMagacina(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacin.SelectedIndex != -1)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete potrošnu opremu ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().IzbrisiPotrosnuOpremuIzMagacina(TabelaDinamickeMagacin.SelectedIndex);
                    OsveziPrikazOpreme();
                }
            }
            else
                MessageBox.Show("Označite potrošnu opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajPotrosnuOpremuUMagacin(object sender, RoutedEventArgs e)
        {

            if (ProstorijeKontroler.GetInstance().ProveriValidnostOpreme(NazivDinamickeOpreme.Text.Trim().ToLower(), KolicinaDinamickeOpreme.Text.Trim()))
            {
                String naziv = NazivDinamickeOpreme.Text.Trim().ToLower();
                naziv = char.ToUpper(naziv[0]) + naziv.Substring(1);
                ProstorijeKontroler.GetInstance().DodajPotrosnuOpremuUMagacin(naziv, Int32.Parse(KolicinaDinamickeOpreme.Text.Trim()));
                OsveziPrikazOpreme();
                OcistiTextPoljaDinamickeOpremeUMagacinu();
            }
        }

        private void IzmeniStatickuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaStatickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaStatickeOpremeIzmeni.Text.Trim()))
                {
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da izmenite stanje statičke opreme ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuUMagacinu(TabelaStatickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaStatickeOpremeIzmeni.Text.Trim()));
                        OsveziPrikazOpreme();
                        OcistiTextPoljaStatickeOpremeUMagacinu();
                    }
                }
            }
        }

        private void IzmeniPotrosnuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaDinamickeOpremeIzmeni.Text.Trim()))
                {
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da izmenite stanje potrošne opreme ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        ProstorijeKontroler.GetInstance().IzmeniDinamickuOpremuUMagacinu(TabelaDinamickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaDinamickeOpremeIzmeni.Text.Trim()));
                        OsveziPrikazOpreme();
                        OcistiTextPoljaDinamickeOpremeUMagacinu();
                    }
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
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpremePriPrebacivanju(KolicinaOpremeSKojomSeRadi_Copy.Text.Trim()))
                {
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da prebacite opremu ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        PrebacivanjeOpremeInfoDTO prebacivanjeInfo = new PrebacivanjeOpremeInfoDTO
                                                                     (
                                                                     TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex,
                                                                     TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex,
                                                                     NazivOpremeSKojomSeRadi_Copy.Text.Trim(),
                                                                     Int32.Parse(KolicinaOpremeSKojomSeRadi_Copy.Text.Trim())
                                                                     );
                        ProstorijeKontroler.GetInstance().PrebaciStacionarnuOpremuUProstoriju(prebacivanjeInfo, TabelaOpremeIzKojeSePrebacuje.SelectedIndex);
                        OsveziPrikazOpreme();
                        OsveziPrikazTabelaOpreme(prebacivanjeInfo.IndexIzKojeProstorije, prebacivanjeInfo.IndexUKojuProstoriju);
                        KolicinaOpremeSKojomSeRadi_Copy.Clear();
                        NazivOpremeSKojomSeRadi_Copy.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz leve kolone koju želite da prebacite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IzmeniStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            if (OpremaStanjeOpreme.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostKolicineOpreme(KolicinaOpremeSKojomSeRadi.Text.Trim()))
                {
                    IzmenaOpremeInfoDTO izmenaOpremeInfo = new IzmenaOpremeInfoDTO
                                                      (
                                                      ProstorijeStanjeOpreme.SelectedIndex,
                                                      OpremaStanjeOpreme.SelectedIndex,
                                                      Int32.Parse(KolicinaOpremeSKojomSeRadi.Text.Trim())
                                                      );
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da izmenite stanje opreme ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuProstorije(izmenaOpremeInfo);
                        OsveziPrikazOpreme();
                        OsveziPrikazTabelaOpreme(-1, izmenaOpremeInfo.IndexProstorije);
                        KolicinaOpremeSKojomSeRadi.Clear();
                        NazivOpremeSKojomSeRadi.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ObrisiStatickuOpremuIzProstorije(object sender, RoutedEventArgs e)
        {
            if (OpremaStanjeOpreme.SelectedIndex != -1)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete selektovanu opremu ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().IzbrisiStacionarnuOpremuIzProstorije(ProstorijeStanjeOpreme.SelectedIndex, OpremaStanjeOpreme.SelectedIndex);
                    OsveziPrikazOpreme();
                    OsveziPrikazTabelaOpreme(-1, ProstorijeStanjeOpreme.SelectedIndex);
                }
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

        private void OpremaStanjeOpreme_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OpremaStanjeOpreme.SelectedIndex != -1)
                NamapirajOpremuZaPrebacivanjeUProstoriju(OpremaStanjeOpreme.SelectedIndex, ProstorijeStanjeOpreme.SelectedIndex);
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
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da otkažete prebacivanje opreme ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ZakazanaPreraspodelaStatickeOpremeKontroler.GetInstance().OtkaziPreraspodeluStatickeOpreme(TabelaZakazanihPrebacivanjaOpreme.SelectedIndex);
                    OsveziPrikazPreraspodeleOpreme();
                }
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
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da otkažete renoviranje ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().ZavrsiRenoviranje(TabelaRenoviranja.SelectedIndex);
                    OsveziPrikazRenoviranja();
                    OsveziPrikazProstorija();
                }
            }
            else
                MessageBox.Show("Označite renoviranje iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DodajLek(object sender, RoutedEventArgs e)
        {
            LekValidacijaDTO lekZaValidaciju = new LekValidacijaDTO
                                                   (
                                                   VrstaLeka.SelectedIndex,
                                                   KolicinaLeka.Text.Trim(),
                                                   NazivLeka.Text.Trim().ToLower(),
                                                   KlasaLeka.SelectedIndex,
                                                   JacinaLeka.Text.Trim(),
                                                   ZamenskiLek.Text.Trim().ToLower(),
                                                   SastavLeka.Text.Trim()
                                                   );
            LekDTO lekZaDodavanje;
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj", -1))
            {
                String naziv = NazivLekaIzmeni.Text.Trim().ToLower();
                String zamenski = ZamenskiLekIzmeni.Text.Trim().ToLower();
                if (naziv.Length > 1)
                    naziv = char.ToUpper(naziv[0]) + naziv.Substring(1);
                if (zamenski.Length > 1)
                    zamenski = char.ToUpper(zamenski[0]) + zamenski.Substring(1);
                lekZaDodavanje = new LekDTO
                                     (
                                     LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka),
                                     Int64.Parse(lekZaValidaciju.KolicinaLeka),
                                     naziv,
                                     LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka),
                                     Int32.Parse(lekZaValidaciju.JacinaLeka),
                                     zamenski,
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
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete lek ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    LekKontroler.GetInstance().IzbrisiLek(TabelaLekova.SelectedIndex);
                    OsveziPrikazLekova();
                }
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
                                                       KolicinaLekaIzmeni.Text.Trim(),
                                                       NazivLekaIzmeni.Text.Trim().ToLower(),
                                                       KlasaLekaIzmeni.SelectedIndex,
                                                       JacinaLekaIzmeni.Text.Trim(),
                                                       ZamenskiLekIzmeni.Text.Trim().ToLower(),
                                                       SastavLekaIzmeni.Text.Trim()
                                                       );
                LekDTO lekZaIzmenu;
                if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "izmeni", TabelaLekovaIzmeni.SelectedIndex))
                {
                    String naziv = NazivLekaIzmeni.Text.Trim().ToLower();
                    String zamenski = ZamenskiLekIzmeni.Text.Trim().ToLower();
                    if (naziv.Length > 1)
                        naziv = char.ToUpper(naziv[0]) + naziv.Substring(1);
                    if (zamenski.Length > 1)
                        zamenski = char.ToUpper(zamenski[0]) + zamenski.Substring(1);
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da izmenite lek ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        lekZaIzmenu = new LekDTO
                                      (
                                      LekKontroler.GetInstance().GetVrstuLeka(lekZaValidaciju.VrstaLeka),
                                      Int64.Parse(lekZaValidaciju.KolicinaLeka),
                                      naziv,
                                      LekKontroler.GetInstance().GetKlasuLeka(lekZaValidaciju.KlasaLeka),
                                      Int32.Parse(lekZaValidaciju.JacinaLeka),
                                      zamenski,
                                      lekZaValidaciju.SastavLeka
                                      );
                        LekKontroler.GetInstance().IzmeniLek(TabelaLekovaIzmeni.SelectedIndex, lekZaIzmenu);
                        OsveziPrikazLekova();
                        OcistiTextPoljaLekova();
                    }
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
                                                   KolicinaLeka.Text.Trim(),
                                                   NazivLeka.Text.Trim().ToLower(),
                                                   KlasaLeka.SelectedIndex,
                                                   JacinaLeka.Text.Trim(),
                                                   ZamenskiLek.Text.Trim().ToLower(),
                                                   SastavLeka.Text.Trim()
                                                   );
            if (LekKontroler.GetInstance().ProveriValidnostLeka(lekZaValidaciju, "dodaj", -1))
            {
                String naziv = NazivLekaIzmeni.Text.Trim().ToLower();
                String zamenski = ZamenskiLekIzmeni.Text.Trim().ToLower();
                if (naziv.Length > 1)
                    naziv = char.ToUpper(naziv[0]) + naziv.Substring(1);
                if (zamenski.Length > 1)
                    zamenski = char.ToUpper(zamenski[0]) + zamenski.Substring(1);
                if (LekariLekovi.SelectedIndex != -1)
                { 
                    var potvrda = MessageBox.Show("Da li ste sigurni da želite da pošaljete lek na proveru izabranom lekaru ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (potvrda == MessageBoxResult.Yes)
                    {
                        String infoOLeku = "Vrsta:" + VrstaLekaLepIspis(lekZaValidaciju.VrstaLeka) + "   Jačina:" + lekZaValidaciju.JacinaLeka + " mg" +
                                       "   Zamenski lek:" + zamenski + "   Sastav:" + lekZaValidaciju.SastavLeka;
                        VerifikacijaLekaDTO verifikacija = new VerifikacijaLekaDTO
                                                               (
                                                               DateTime.Now,
                                                               naziv,
                                                               infoOLeku,
                                                               "1903999772025",
                                                               GetJmbgLekaraZaValidaciju(LekariLekovi.SelectedIndex),
                                                               Napomena.Text
                                                               );
                        VerifikacijaLekaKontroler.GetInstance().PosaljiVerifikacijuLeka(verifikacija);
                        OcistiTextPoljaLekova();
                        MessageBox.Show("Lek uspešno poslat lekaru na verifikaciju.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    MessageBox.Show("Odaberite lekara kome želite da pošaljete verifikaciju !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            { }
        }

        private String VrstaLekaLepIspis(int index)
        {
            if (index == 0)
                return "kapsula";
            else if (index == 1)
                return "Tableta";
            else if (index == 2)
                return "sirup";
            else if (index == 3)
                return "sprej";
            else if (index == 4)
                return "gel";
            else
                return "šumeća Tableta";
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
            if (ProstorijeKontroler.GetInstance().ProveriValidnostPretrage(NazivPretraga.Text.Trim(), KolicinaPretraga.Text.Trim(), UpitPretrage.SelectedIndex))
            {
                PretragaInfoDTO info = new PretragaInfoDTO(NazivPretraga.Text.Trim(), KolicinaPretraga.Text.Trim(), UpitPretrage.SelectedIndex);
                TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = ProstorijeKontroler.GetInstance().PretraziProstorijePoOpremi
                                                                     (info);
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
                VrstaLekaIzmeni.Text = "Šumeća Tableta";
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
            BrojProstorijeNaprednoRenoviranjeComboBox.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
            BrojProstorije1ComboBox.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
            BrojProstorije2ComboBox.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
            ProstorijeStanjeOpreme.ItemsSource = ProstorijeKontroler.GetInstance().GetAll();
        }

        private void OsveziPrikazOpreme()
        {
            TabelaStatickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Staticka;
            TabelaStatickeMagacinIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Staticka;
            TabelaDinamickeMagacinIzmeni.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna;
            TabelaDinamickeMagacin.ItemsSource = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna;
            if (ProstorijeStanjeOpreme.SelectedIndex != -1)
                OpremaStanjeOpreme.ItemsSource = ProstorijeKontroler.GetInstance().GetAll()[ProstorijeStanjeOpreme.SelectedIndex].Staticka;
        }

        private void OsveziPrikazLekova()
        {
            TabelaLekova.ItemsSource = LekKontroler.GetInstance().GetAll();
            TabelaLekovaIzmeni.ItemsSource = LekKontroler.GetInstance().GetAll();
        }

        private void OsveziPrikazRenoviranja()
        {
            TabelaRenoviranja.ItemsSource = ProstorijeKontroler.GetInstance().GetAllRenoviranja();
            TabelaNaprednogRenoviranja.ItemsSource = ProstorijeKontroler.GetInstance().GetAllNaprednaRenoviranja();
        }

        private void OsveziPrikazVerifikacijaLeka()
        {
            TabelaVerifikacija.ItemsSource = VerifikacijaLekaKontroler.GetInstance().GetVerifikacijePrikaz("1903999772025");
            LekariLekovi.ItemsSource = LekarKontroler.getInstance().GetAll();
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

        private void OcistiTextPoljaPodeleProstorije()
        {
            NaprednoRenoviranjeSoba1.Clear();
            NaprednoRenoviranjeSoba2.Clear();
        }

        private void OcistiTextPoljaSpajanjaProstorija()
        {
            NaprednoRenoviranjeNovaSoba.Clear();
        }

        private void OcistiTextPoljaLekova()
        {
            NazivLeka.Clear();
            KolicinaLeka.Clear();
            JacinaLeka.Clear();
            ZamenskiLek.Clear();
            SastavLeka.Clear();
            NazivLekaIzmeni.Clear();
            KolicinaLekaIzmeni.Clear();
            JacinaLekaIzmeni.Clear();
            ZamenskiLekIzmeni.Clear();
            SastavLeka.Clear();
            Napomena.Clear();
        }

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

        private void OsveziPrikazNaprednihRenoviranja()
        {
            TabelaNaprednogRenoviranja.ItemsSource = ProstorijeKontroler.GetInstance().GetAllNaprednaRenoviranja();
        }

        private void ZakaziPodeluProstorijeClick(object sender, RoutedEventArgs e)
        {
            if (BrojProstorijeNaprednoRenoviranjeComboBox.SelectedIndex != -1 && DatumPocetkaPodeleSobe.SelectedDate != null &&
                DatumZavrsetkaPodeleSobe.SelectedDate != null)
            {
                Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
                if (ProstorijeKontroler.GetInstance().ValidirajBrojProstorije(sablon, NaprednoRenoviranjeSoba1.Text.Trim()) &&
                    ProstorijeKontroler.GetInstance().ValidirajBrojProstorije(sablon, NaprednoRenoviranjeSoba2.Text.Trim()))
                {
                    if (!NaprednoRenoviranjeSoba1.Text.Trim().Equals(NaprednoRenoviranjeSoba2.Text.Trim()))
                    {
                        NaprednoRenoviranjeDTO naprednoDTO = new NaprednoRenoviranjeDTO()
                        {
                            BrojGlavneProstorije = ProstorijeKontroler.GetInstance().GetAll()[BrojProstorijeNaprednoRenoviranjeComboBox.SelectedIndex].BrojSobe,
                            BrojProstorije1 = NaprednoRenoviranjeSoba1.Text.Trim(),
                            BrojProstorije2 = NaprednoRenoviranjeSoba2.Text.Trim(),
                            DatumPocetkaRenoviranja = (DateTime)DatumPocetkaPodeleSobe.SelectedDate,
                            DatumZavrsetkaRenoviranja = (DateTime)DatumZavrsetkaPodeleSobe.SelectedDate,
                            Spajanje = false,
                            Podela = true
                        };
                        ProstorijeKontroler.GetInstance().DodajNaprednoRenoviranje(naprednoDTO);
                        OsveziPrikazNaprednihRenoviranja();
                        OcistiTextPoljaPodeleProstorije();
                    }
                    else
                        MessageBox.Show("Uneli ste iste brojeve za obe nove prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Niste uneli sve potrebnje podatke (brojeve prostorija ili datume) !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ZakaziSpajanjeProstorijaClick(object sender, RoutedEventArgs e)
        {
            if (BrojProstorije1ComboBox.SelectedIndex != -1 && BrojProstorije2ComboBox.SelectedIndex != -1 &&
                DatumPocetkaSpajanjaSoba.SelectedDate != null && DatumZavrsetkaSpajanjaSoba.SelectedDate != null)
            {
                Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
                if (ProstorijeKontroler.GetInstance().ValidirajBrojProstorije(sablon, NaprednoRenoviranjeNovaSoba.Text.Trim()))
                {
                    if (!BrojProstorije1ComboBox.SelectedIndex.Equals(BrojProstorije2ComboBox.SelectedIndex))
                    {
                        NaprednoRenoviranjeDTO naprednoDTO = new NaprednoRenoviranjeDTO()
                        {
                            BrojGlavneProstorije = NaprednoRenoviranjeNovaSoba.Text.Trim(),
                            BrojProstorije1 = ProstorijeKontroler.GetInstance().GetAll()[BrojProstorije1ComboBox.SelectedIndex].BrojSobe,
                            BrojProstorije2 = ProstorijeKontroler.GetInstance().GetAll()[BrojProstorije2ComboBox.SelectedIndex].BrojSobe,
                            DatumPocetkaRenoviranja = (DateTime)DatumPocetkaSpajanjaSoba.SelectedDate,
                            DatumZavrsetkaRenoviranja = (DateTime)DatumZavrsetkaSpajanjaSoba.SelectedDate,
                            Spajanje = true,
                            Podela = false
                        };
                        ProstorijeKontroler.GetInstance().DodajNaprednoRenoviranje(naprednoDTO);
                        OsveziPrikazNaprednihRenoviranja();
                        OcistiTextPoljaSpajanjaProstorija();
                    }
                    else
                        MessageBox.Show("Ne možete spojiti iste prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Niste uneli sve potrebnje podatke (broj prostorije ili datume) !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OtkaziNaprednoRenoviranjeClick(object sender, RoutedEventArgs e)
        {
            if (TabelaNaprednogRenoviranja.SelectedIndex != -1)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da otkažete renoviranje ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (potvrda == MessageBoxResult.Yes)
                {
                    ProstorijeKontroler.GetInstance().ObrisiNaprednoRenoviranje(TabelaNaprednogRenoviranja.SelectedIndex);
                    OsveziPrikazNaprednihRenoviranja();
                }
            }
            else
                MessageBox.Show("Označite renoviranje koje želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void SacuvajIzvestajTermina()
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.TableName = "TABELA ZAUZETOSTI PROSTORIJA ZA NAREDNIH 7 DANA - TERMINI";
                Table.Columns.Add("Prostorija");
                Table.Columns.Add("Datum i vreme početka termina");
                Table.Columns.Add("Trajanje termina");
                Table.Rows.Add(new string[] { "Prostorija", "Datum i vreme pocetka termina", "Trajanje termina" });
                foreach (Termin termin in SkladisteZaTermineXml.getInstance().GetAll())
                {
                    if (DateTime.Compare(termin.DatumIVremeTermina.Date, DateTime.Now.Date) > 0) /*&& DateTime.Compare(termin.DatumIVremeTermina.Date, DateTime.Now.Date.AddDays(7)) <= 0*///)
                        Table.Rows.Add(new string[] { termin.brojSobe, termin.DatumIVremeTermina.ToString(), "30" });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - termini.pdf");
                Document.Close(true);
            }
        }

        private void SacuvajIzvestajPreraspodela()
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.TableName = "TABELA ZAUZETOSTI PROSTORIJA ZA NAREDNIH 7 DANA - PRERASPODELE";
                Table.Columns.Add("Prostrija iz koje se prenosi oprema");
                Table.Columns.Add("Prostorija u koju se prenosi oprema");
                Table.Columns.Add("Datum i vreme početka preraspodele");
                Table.Columns.Add("Trajanje preraspodele");
                Table.Rows.Add(new string[] { "Prostorija iz koje se prenosi oprema", "Prostorija iz koje se prenosi oprema", "Datum i vreme početka preraspodele", "Trajanje preraspodele" });
                foreach (ZakazanaPreraspodelaStatickeOpreme preraspodela in SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll())
                {
                    // if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.Date, DateTime.Now.Date) > 0) /*&& DateTime.Compare(termin.DatumIVremeTermina.Date, DateTime.Now.Date.AddDays(7)) <= 0*///)
                    Table.Rows.Add(new string[] { preraspodela.BrojProstorijeIzKojeSePrenosiOprema, preraspodela.BrojProstorijeUKojuSePrenosiOprema, preraspodela.DatumIVremePreraspodele.ToString(), "60" });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - preraspodele.pdf");
                Document.Close(true);
            }
        }

        private void SacuvajIzvestajRenoviranja()
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.TableName = "TABELA ZAUZETOSTI PROSTORIJA ZA NAREDNIH 7 DANA - PRERASPODELE";
                Table.Columns.Add("Prostorija");
                Table.Columns.Add("Sprat");
                Table.Columns.Add("Datum i vreme početka renoviranja");
                Table.Columns.Add("Datum i vreme završetka renoviranja");
                Table.Rows.Add(new string[] { "Prostorija", "Sprat", "Datum i vreme početka renoviranja", "Datum i vreme kraja renoviranja" });
                foreach (Renoviranje renoviranje in SkladisteZaRenoviranjaXml.GetInstance().GetAll())
                {
                    // if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.Date, DateTime.Now.Date) > 0) /*&& DateTime.Compare(termin.DatumIVremeTermina.Date, DateTime.Now.Date.AddDays(7)) <= 0*///)
                    Table.Rows.Add(new string[] { renoviranje.BrojProstorije, renoviranje.Sprat.ToString(), renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - renoviranja.pdf");
                Document.Close(true);
            }
        }

        private void SacuvajIzvestajNaprednihRenoviranja()
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.TableName = "TABELA ZAUZETOSTI PROSTORIJA ZA NAREDNIH 7 DANA - PRERASPODELE";
                Table.Columns.Add("Glavna prostorija");
                Table.Columns.Add("Prostorija 1");
                Table.Columns.Add("Prostorija 2");
                Table.Columns.Add("Datum i vreme početka renoviranja");
                Table.Columns.Add("Datum i vreme završetka renoviranja");
                Table.Rows.Add(new string[] { "Glavna prostorija", "Prostorija 1", "Prostorija 2", "Datum i vreme početka renoviranja", "Datum i vreme kraja renoviranja" });
                foreach (NaprednoRenoviranje renoviranje in SkladisteZaNaprednaRenoviranjaXml.GetInstance().GetAll())
                {
                    // if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.Date, DateTime.Now.Date) > 0) /*&& DateTime.Compare(termin.DatumIVremeTermina.Date, DateTime.Now.Date.AddDays(7)) <= 0*///)
                    Table.Rows.Add(new string[] { renoviranje.BrojGlavneProstorije, renoviranje.BrojProstorije1, renoviranje.BrojProstorije2, renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - napredna renoviranja.pdf");
                Document.Close(true);
            }
        }

        private void Odjava_Ne_ButtonClick(object sender, RoutedEventArgs e)
        {
            PocetnaStrana.IsSelected = true;
        }

        private void GenerisiIzvestaje_Click(object sender, RoutedEventArgs e)
        {
            SacuvajIzvestajTermina();
            SacuvajIzvestajPreraspodela();
            SacuvajIzvestajRenoviranja();
            SacuvajIzvestajNaprednihRenoviranja();
            MessageBox.Show("Izveštaji o zauzetosti prostorija uspešno generisani !", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Odjava_Da_ButtonClick(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("u");
            UpravnikWindow.GetInstance().Close();
            s.Show();
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                timelineSlider.Value = TutorialVideo.Position.TotalSeconds;
            }
        }
        private void timelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TutorialVideo.Position = TimeSpan.FromSeconds(timelineSlider.Value);
        }

        private void TutorialVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Stop();
        }

        private void timelineSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void timelineSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            TutorialVideo.Position = TimeSpan.FromSeconds(timelineSlider.Value);
        }

        private void btnPusti_Click(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Play();
            lblStatus.Content = "Snimak pušten";
        }

        private void btnPauza_Click(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Pause();
            lblStatus.Content = "Snimak pauziran";
        }

        private void btnZaustavi_Click(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Stop();
            lblStatus.Content = "Snimak resetovan na početak";
        }

        private void ObrisiVerifikaciju_Click(object sender, RoutedEventArgs e)
        {
            var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete verifikaciju leka ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (potvrda == MessageBoxResult.Yes)
            { 
                
            }
        }

        private void ProstorijeStanjeOpreme_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ProstorijeStanjeOpreme.SelectedIndex != -1)
            {
                OsveziPrikazOpreme();
                NazivOpremeSKojomSeRadi.Clear();
                KolicinaOpremeSKojomSeRadi.Clear();
            }
        }

        private void TabelaProstorijaIzKojeSePrebacujeOprema_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex != -1)
                OpremaSoba1.Text = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijaIzKojeSePrebacujeOprema.SelectedIndex].BrojSobe;
        }

        private void TabelaProstorijeUKojuSePrebacujeOprema_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OpremaSoba2.Text = ProstorijeKontroler.GetInstance().GetAll()[TabelaProstorijeUKojuSePrebacujeOprema.SelectedIndex].BrojSobe;
        }
    }
}