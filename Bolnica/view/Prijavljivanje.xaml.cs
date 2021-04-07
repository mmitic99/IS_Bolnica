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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (uloga.Equals("u"))
            {
                List<Upravnik> upravnici = SkladisteUpravnik.GetInstance().GetAll();

                bool korisnickoImeOK = false;
                bool lozinkaOK = false;
                Upravnik upravnik = new Upravnik();

                foreach (Upravnik upravnik1 in upravnici)
                {
                    if (upravnik1.Korisnik.KorisnickoIme.Equals(korIme.Text))
                    {
                        upravnik = upravnik1;
                        korisnickoImeOK = true;
                        if (upravnik1.Korisnik.Lozinka.Equals(lozinka.Password))
                        {
                            lozinkaOK = true;
                        }
                        break;
                    }
                }
                if (korisnickoImeOK && lozinkaOK)
                {
                    var s = new UpravnikWindow(upravnik);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
                else
                {
                    if (!korisnickoImeOK)
                    {
                        MessageBox.Show("Neispravno korisničko ime!");
                        korIme.Text = "";
                        lozinka.Password = "";
                        korIme.Focus();
                    }
                    else if (!lozinkaOK)
                    {
                        MessageBox.Show("Neispravna lozinka!");
                        lozinka.Password = "";
                        lozinka.Focus();
                    }
                }
            }
            else if (uloga.Equals("s"))
            {
                List<Sekretar> sekretari = SkladisteSekretara.GetInstance().GetAll();

                bool korisnickoImeOK = false;
                bool lozinkaOK = false;
                Sekretar sekretar = new Sekretar();

                foreach (Sekretar sekretar1 in sekretari)
                {
                    if (sekretar1.Korisnik.KorisnickoIme.Equals(korIme.Text))
                    {
                        sekretar = sekretar1;
                        korisnickoImeOK = true;
                        if (sekretar1.Korisnik.Lozinka.Equals(lozinka.Password))
                        {
                            lozinkaOK = true;
                        }
                        break;
                    }
                }
                if (korisnickoImeOK && lozinkaOK)
                {
                    var s = new SekretarWindow(sekretar);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
                else
                {
                    if (!korisnickoImeOK)
                    {
                        MessageBox.Show("Neispravno korisničko ime!");
                        korIme.Text = "";
                        lozinka.Password = "";
                        korIme.Focus();
                    }
                    else if (!lozinkaOK)
                    {
                        MessageBox.Show("Neispravna lozinka!");
                        lozinka.Password = "";
                        lozinka.Focus();
                    }
                }
            }
            else if (uloga.Equals("l"))
            {
                List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();

                bool korisnickoImeOK = false;
                bool lozinkaOK = false;
                Lekar lekar = new Lekar();

                foreach (Lekar lekar1 in lekari)
                {
                    if (lekar1.Korisnik.KorisnickoIme.Equals(korIme.Text))
                    {
                        lekar = lekar1;
                        korisnickoImeOK = true;
                        if (lekar1.Korisnik.Lozinka.Equals(lozinka.Password))
                        {
                            lozinkaOK = true;
                        }
                        break;
                    }
                }
                if (korisnickoImeOK && lozinkaOK)
                {
                    var s = new PregledWindow(lekar);
                    potvrdi = true;
                    this.Close();
                    s.Show();
                }
                else
                {
                    if (!korisnickoImeOK)
                    {
                        MessageBox.Show("Neispravno korisničko ime!");
                        korIme.Text = "";
                        lozinka.Password = "";
                        korIme.Focus();
                    }
                    else if (!lozinkaOK)
                    {
                        MessageBox.Show("Neispravna lozinka!");
                        lozinka.Password = "";
                        lozinka.Focus();
                    }
                }
            }
            else if (uloga.Equals("p"))
            {
                List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();

                bool korisnickoImeOK = false;
                bool lozinkaOK = false;
                Pacijent pacijent = new Pacijent();

                foreach (Pacijent pacijent1 in pacijenti)
                {
                    if (pacijent1.Korisnik.KorisnickoIme.Equals(korIme.Text))
                    {
                        pacijent = pacijent1;
                        korisnickoImeOK = true;
                        if (pacijent1.Korisnik.Lozinka.Equals(lozinka.Password))
                        {
                            lozinkaOK = true;
                        }
                        break;
                    }
                }
                if (korisnickoImeOK && lozinkaOK)
                {
                   // var s = new PacijentWindow(pacijent);
                    var s1 = new PacijentMainWindow(pacijent);
                    potvrdi = true;
                    this.Close();
                   // s.Show();
                    s1.Show();
                }
                else
                {
                    if (!korisnickoImeOK)
                    {
                        MessageBox.Show("Neispravno korisničko ime!");
                        korIme.Text = "";
                        lozinka.Password = "";
                        korIme.Focus();
                    }
                    else if (!lozinkaOK)
                    {
                        MessageBox.Show("Neispravna lozinka!");
                        lozinka.Password = "";
                        lozinka.Focus();
                    }
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
