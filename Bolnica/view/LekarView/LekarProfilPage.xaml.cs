using Bolnica.DTOs;
using Kontroler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for LekarProfilPage.xaml
    /// </summary>
    public partial class LekarProfilPage : Page
    {
        public static bool isToolTipVisible = true;

        private static LekarProfilPage instance = null;


        public static LekarProfilPage getInstance()
        {
            return instance;
        }
        public LekarProfilPage()
        {   

            InitializeComponent();    
            txt1.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Ime;
            txt2.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Prezime;
            txt3.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Adresa;
            txt4.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().BrojTelefona;
            txt5.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Specijalizacija.ToString();
            txt6.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Korisnik.KorisnickoIme;
            txt7.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Korisnik.Lozinka;
            instance = this;
            setToolTip(LekarProfilPage.isToolTipVisible);
        }
        private bool Validiraj(Regex sablon, String unos)
        {
            if (sablon.IsMatch(unos))
                return true;
            else
                return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Validiraj(new Regex(@"^[A-Z]{1}[a-z]{1,19}$"), txt1.Text)){
                if (Validiraj(new Regex(@"^[A-Za-z\s]{1,40}$"), txt2.Text))
                {
                    if (Validiraj(new Regex(@"^[A-Za-z\s0-9/]{1,69}$"), txt3.Text))
                    {
                        if (Validiraj(new Regex(@"^[0-9]{1,15}$"), txt4.Text))
                        {
                            LekarDTO trenutniLekar = LekarKontroler.getInstance().trenutnoUlogovaniLekar();
                            LekarDTO lekar = new LekarDTO
                            {
                                Ime = txt1.Text,
                                Prezime = txt2.Text,
                                BracnoStanje = trenutniLekar.BracnoStanje,
                                Adresa = txt3.Text,
                                Jmbg = trenutniLekar.Jmbg,
                                Zanimanje = trenutniLekar.Zanimanje,
                                Pol = trenutniLekar.Pol,
                                DatumRodjenja = trenutniLekar.DatumRodjenja,
                                BrojTelefona = txt4.Text,
                                Email = trenutniLekar.Email,
                                NazivGrada = trenutniLekar.NazivGrada,
                                Korisnik = new KorisnikDTO() { KorisnickoIme = txt6.Text, Lozinka = txt7.Text },
                                Specijalizacija = txt5.Text,
                                BrojSlobodnihDana = trenutniLekar.BrojSlobodnihDana,
                                IdOrdinacija = trenutniLekar.IdOrdinacija,
                                FullName = "dr " + txt1.Text + " " + txt2.Text,
                                ImeiSpecijalizacija = "dr " + txt1.Text + " " + txt2.Text + "-" + txt3.Text


                            };
                            LekarKontroler.getInstance().IzmeniLekara(trenutniLekar.Jmbg, lekar);
                            txt1.Text = lekar.Ime;
                            txt2.Text = lekar.Prezime;
                            txt3.Text = lekar.Adresa;
                            txt4.Text = lekar.BrojTelefona;
                            txt5.Text = lekar.Specijalizacija.ToString();
                            txt6.Text = lekar.Korisnik.KorisnickoIme;
                            txt7.Text = lekar.Korisnik.Lozinka;

                        }
                        else
                            MessageBox.Show("Broj telefojna se sastoji od brojeva, maksimalno 15!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else
                        MessageBox.Show("Adresa se sastoji samo od slova i brojeva, maksimalno 60!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Prezime se sastoji samo od slova i maksimalno 40!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
                MessageBox.Show("Ime se sastoji samo od slova i maksimalno 20!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }

        private void MenuItem_Click_Lekovi(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekoviPage();
        }

        private void MenuItem_Click_Obavestenja(object sender, RoutedEventArgs e)

        {
            LekarWindow.getInstance().Frame1.Content = new LekarObavestenjaPage();
        }
        private void MenuItem_Click_Pacijenti(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
        }
        private void MenuItem_Click_Termini(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().trenutnoUlogovaniLekar());
        }

        private void Button_Click_DA(object sender, RoutedEventArgs e)
        {
            isToolTipVisible = true;
        }  

        private void Button_Click_NE(object sender, RoutedEventArgs e)
        {
            
           
                    isToolTipVisible = false;

            
        }
        private void setToolTip(bool Prikazi)
        {


            if (Prikazi)
            {
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();
                this.Resources.Add(typeof(ToolTip), style);


            }
            else
            {
                this.Resources.Remove(typeof(ToolTip));
            }
        }
    }
}
