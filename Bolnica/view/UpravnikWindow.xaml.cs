using Model;
using Model.Enum;
using Model.Skladista;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        SkladisteZaProstorije skladiste = new SkladisteZaProstorije();
        public List<Prostorija> lista
        {
            get;
            set;
        }
        public UpravnikWindow()
        {
            InitializeComponent();
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
