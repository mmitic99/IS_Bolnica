using Bolnica.view;
using Model;
using Repozitorijum;
using System.Collections.Generic;
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
            SkladisteUpravnikXml.GetInstance().Save(upravnik);
            Sekretar sekretar = new Sekretar { Korisnik = new Korisnik { KorisnickoIme = "sekretar", Lozinka = "sekretar" } };
            SkladisteSekretaraXml.GetInstance().Save(sekretar);
            Lekar lekar = new Lekar { Korisnik = new Korisnik { KorisnickoIme = "lekar", Lozinka = "lekar" } };
            SkladisteZaLekaraXml.GetInstance().Save(lekar);*/

            /*SkladisteZaSpecijalizacijuXml skladisteZaSpecijalizaciju = new SkladisteZaSpecijalizacijuXml();
            List<Specijalizacija> specijalizacije = new List<Specijalizacija>();
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "opšta medicina" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "interna medicina" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "pedijatrija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "kardiohirurgija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "neurohirurgija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "otorinolaringologija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "oftalmologija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "imunologija" });
            specijalizacije.Add(new Specijalizacija { VrstaSpecijalizacije = "epidemiologija" });
            skladisteZaSpecijalizaciju.SaveAll(specijalizacije);*/

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
