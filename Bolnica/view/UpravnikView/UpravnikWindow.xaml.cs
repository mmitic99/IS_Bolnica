using Kontroler;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
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
                if (ProstorijeKontroler.GetInstance().ProveriValidnostIzmenjeneOpreme(KolicinaStatickeOpremeIzmeni.Text))
                    ProstorijeKontroler.GetInstance().IzmeniStacionarnuOpremuUMagacinu(TabelaStatickeMagacinIzmeni.SelectedIndex, Int32.Parse(KolicinaStatickeOpremeIzmeni.Text));
            }
        }

        private void IzmeniPotrosnuOpremuMagacin(object sender, RoutedEventArgs e)
        {
            if (TabelaDinamickeMagacinIzmeni.SelectedIndex != -1)
            {
                if (ProstorijeKontroler.GetInstance().ProveriValidnostIzmenjeneOpreme(KolicinaDinamickeOpremeIzmeni.Text))
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
    }
}
