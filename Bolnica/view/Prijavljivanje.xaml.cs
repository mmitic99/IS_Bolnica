using Bolnica.view.SekretarView;
using Kontroler;
using Model;
using Repozitorijum;
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
    /// Interaction logic for Prijavljivanje.xaml
    /// </summary>
    public partial class Prijavljivanje : Window
    {
        private String uloga;
        private bool potvrdi = false;

        private UpravnikKontroler upravnikKontroler;
        private SekretarKontroler sekretarKontroler;
        private LekarKontroler lekarKontroler;
        private PacijentKontroler pacijentKontroler;

        public Prijavljivanje(String uloga)
        {
            InitializeComponent();
            this.uloga = uloga;
            korIme.Focus();
            if (uloga.Equals("u"))
            {
                this.Title += " - Upravnik";
            }
            else if (uloga.Equals("s"))
            {
                this.Title += " - Sekretar";

            }
            else if (uloga.Equals("l"))
            {
                this.Title += " - Lekar";

            }
            else if (uloga.Equals("p"))
            {
                this.Title += " - Pacijent";

            }

            upravnikKontroler = new UpravnikKontroler();
            sekretarKontroler = new SekretarKontroler();
            lekarKontroler = new LekarKontroler();
            pacijentKontroler = new PacijentKontroler();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (uloga.Equals("u"))
            {
                Upravnik prijavljeniUpravnik = (Upravnik)upravnikKontroler.PrijavljivanjeKorisnika(korIme.Text, lozinka.Password);

                if(prijavljeniUpravnik == null)
                {
                    MessageBox.Show("Neispravno korisničko ime ili lozinka!");
                    korIme.Focus();
                }
                else
                {
                    var s = new UpravnikWindow(prijavljeniUpravnik);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
            }
            else if (uloga.Equals("s"))
            {
                Sekretar prijavljeniSekretar = (Sekretar)sekretarKontroler.PrijavljivanjeKorisnika(korIme.Text, lozinka.Password);

                if (prijavljeniSekretar == null)
                {
                    MessageBox.Show("Neispravno korisničko ime ili lozinka!");
                    korIme.Focus();
                }
                else
                {
                    var s = new SekretarWindow(prijavljeniSekretar);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
            }
            else if (uloga.Equals("l"))
            {
                Lekar prijavljeniLekar = (Lekar)lekarKontroler.PrijavljivanjeKorisnika(korIme.Text, lozinka.Password);

                if (prijavljeniLekar == null)
                {
                    MessageBox.Show("Neispravno korisničko ime ili lozinka!");
                    korIme.Focus();
                }
                else
                {
                    var s = new LekarWindow(prijavljeniLekar);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
            }
            else if (uloga.Equals("p"))
            {
                Pacijent prijavljeniPacijent = (Pacijent)pacijentKontroler.PrijavljivanjeKorisnika(korIme.Text, lozinka.Password);

                if (prijavljeniPacijent == null)
                {
                    MessageBox.Show("Neispravno korisničko ime ili lozinka!");
                    korIme.Focus();
                }
                else
                {

                    var s1 = new PacijentMainWindow(prijavljeniPacijent);
                    potvrdi = true;
                    this.Close();
                    s1.Show();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            potvrdi = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!potvrdi)
            {
                var s = new MainWindow();
                s.Show();
            }
        }
    }
}
