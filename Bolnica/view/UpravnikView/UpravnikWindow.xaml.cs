using Kontroler;
using Model;
using Repozitorijum;
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
            StacionarnaMagacin = new List<StacionarnaOprema>();
            PotrosnaMagacin = new List<PotrosnaOprema>();
            StacionarnaOpremaOdKojeSeUzima = new List<StacionarnaOprema>();
            StacionarnaOpremaUKojuSeDodaje = new List<StacionarnaOprema>();
            ListaProstorija = SkladisteZaProstorije.GetInstance().GetAll();
            StacionarnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Staticka_;
            PotrosnaMagacin = ProstorijeKontroler.GetInstance().GetMagacin().Potrosna_;
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

        }

        private void RaspodelaOpremeTab_LostFocus(object sender, RoutedEventArgs e)
        {
            RaspodelaOpremeTab.IsEnabled = false;
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
               // KolicinaOpremeSKojomSeRadi_Copy.Text = StacionarnaOpremaOdKojeSeUzima[indexSelektovaneOpreme].Kolicina_.ToString();
            }
        }

        private void TabelaOpremeUKojuSePrebacuje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabelaOpremeUKojuSePrebacuje.SelectedIndex != -1)
            {
                int indexSelektovaneOpreme = TabelaOpremeUKojuSePrebacuje.SelectedIndex;
                NazivOpremeSKojomSeRadi.Text = StacionarnaOpremaUKojuSeDodaje[indexSelektovaneOpreme].TipStacionarneOpreme_;
                //KolicinaOpremeSKojomSeRadi.Text = StacionarnaOpremaUKojuSeDodaje[indexSelektovaneOpreme].Kolicina_.ToString();
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
    }
}
