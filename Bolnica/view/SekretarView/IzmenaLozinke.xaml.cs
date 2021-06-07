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
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for IzmenaLozinke.xaml
    /// </summary>
    public partial class IzmenaLozinke : Window
    {
        private SekretarDTO sekretar;
        private SekretarKontroler sekretarKontroler;
        public IzmenaLozinke(SekretarDTO sekretar)
        {
            InitializeComponent();
            this.sekretar = sekretar;
            DataContext = sekretar;
            sekretarKontroler = new SekretarKontroler();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (staraLozinka.Password != sekretar.Korisnik.Lozinka)
            {
                MessageBox.Show("Stara lozinka nije ispravna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (novaLozinka.Password != ponovoNovaLozinka.Password)
            {
                MessageBox.Show("Nove lozinke se ne poklapaju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (staraLozinka.Password.Length == 0 || novaLozinka.Password.Length == 0 ||
                ponovoNovaLozinka.Password.Length == 0)
            {
                MessageBox.Show("Lozinke ne mogu biti prazne.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool uspesno = sekretarKontroler.IzmenaLozinke(sekretar.Jmbg, staraLozinka.Password, novaLozinka.Password);
            if (uspesno)
            {
                sekretar.Korisnik.Lozinka = novaLozinka.Password;
                MessageBox.Show("Lozinka uspešno izmenjena.");
                this.Close();
            }
        }

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
