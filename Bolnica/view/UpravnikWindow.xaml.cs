using Model;
using Repozitorijum;
using System.Collections.Generic;
using System.Windows;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        Upravnik upravnik;
        SkladisteZaProstorije skladiste = new SkladisteZaProstorije();
        public List<Prostorija> lista
        {
            get;
            set;
        }
        public UpravnikWindow(Upravnik upravnik)
        {
            InitializeComponent();

            this.upravnik = upravnik;

            this.DataContext = this;
            lista = skladiste.GetAll();
        }
        
        // metoda za dodavanje nove prostorije
        private void DodajProstoriju(object sender, RoutedEventArgs e) 
        {
            var s = new DodavanjeProstorijeWindow(this);
            s.Show();
        }

        // metoda za izmenu selektovane prostorije
        private void IzmeniProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaProstorija.SelectedIndex != -1)
            {
                var s = new IzmenaProstorijeWindow(this, TabelaProstorija.SelectedIndex);
                s.Show();
            }
            else
            {
                MessageBox.Show("Označite prostoriju koju želite da izmenite !", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // metoda za brisanje selektovane prostorije
        private void ObrisiProstoriju(object sender, RoutedEventArgs e)
        {
            if (TabelaProstorija.SelectedIndex != -1)
            {
                lista.RemoveAt(TabelaProstorija.SelectedIndex);
                skladiste.SaveAll(lista);
                TabelaProstorija.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Označite prostoriju koju želite da obrišete !", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
