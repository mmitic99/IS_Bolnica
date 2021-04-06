using Bolnica.view;
using Model;
using Repozitorijum;
using System.Windows;

namespace Bolnica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*Upravnik upravnik = new Upravnik { Korisnik = new Korisnik { KorisnickoIme = "upravnik", Lozinka = "upravnik" } };
            SkladisteUpravnik.GetInstance().Save(upravnik);
            Sekretar sekretar = new Sekretar { Korisnik = new Korisnik { KorisnickoIme = "sekretar", Lozinka = "sekretar" } };
            SkladisteSekretara.GetInstance().Save(sekretar);
            Lekar lekar = new Lekar { Korisnik = new Korisnik { KorisnickoIme = "lekar", Lozinka = "lekar" } };
            SkladisteZaLekara.GetInstance().Save(lekar);*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("u");
            this.Close();
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("s");
            this.Close();
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            this.Close();
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }
    }
}
