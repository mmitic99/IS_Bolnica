using Bolnica.viewActions;
using Kontroler;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PacijentMainWindow.xaml
    /// </summary>
    public partial class PacijentMainWindow : Window
    {
        public static PacijentMainWindow instance = null;


        public static PacijentMainWindow getInstance()
        {
            return instance;
        }
        public Pacijent pacijent { get; set; }

        public PacijentMainWindow(Pacijent pacijent)
        {
            InitializeComponent();
            instance = this;
            this.pacijent = pacijent;
            Ime.DataContext = pacijent;
            DataContext = new MainViewModel();
            brojObavestenja.Text = "0";

            nabaviNovePodsetnike1();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(nabaviNovePodsetnike);
            timer.Interval = new TimeSpan(1, 0, 0);
            timer.Start();
        }

        public void nabaviNovePodsetnike(object sender, EventArgs e)
        {
            List<Recept> recepti= PacijentKontroler.getInstance().dobaviRecepePacijenta(pacijent.Jmbg);
            List<DateTime> terminiUzimanja = new List<DateTime>();
            int brojNovihPodsetnika = 0;
            if (recepti.Count > 0)
            {
                foreach (Recept r in recepti)
                {
                    DateTime dt = (r.DatumIzdavanja.AddDays(r.BrojDana)).Date;
                    if ((r.DatumIzdavanja.AddDays(r.BrojDana)).Date > DateTime.Today)
                    {
                        TimeSpan satVremena = new TimeSpan(1, 1, 0);
                        TimeSpan nula = new TimeSpan(0, 0, 0);
                         foreach(int i in r.TerminiUzimanjaLeka)
                        {
                            if((DateTime.Today.AddHours(i)- DateTime.Now) < satVremena && (DateTime.Today.AddHours(i) - DateTime.Now)> nula)
                            {
                                ObavestenjaKontroler.getInstance().napraviPodsetnik(pacijent.Jmbg, r, i);
                                brojNovihPodsetnika++;
                            }
                        }
                    }
                }
            }
            if(brojNovihPodsetnika>0)
            {
                primiObavestenja(brojNovihPodsetnika);
                if (MainViewModel.getInstance().CurrentView == MainViewModel.getInstance().ObavestenjaVM)
                {
                    view.Obavestenja.getInstance().PodsetnikTerapija.ItemsSource = ObavestenjaKontroler.getInstance().DobaviPodsetnikeZaTerapiju(pacijent.Jmbg);
                }

            }
        }

        public void nabaviNovePodsetnike1()
        {
            List<Recept> recepti = PacijentKontroler.getInstance().dobaviRecepePacijenta(pacijent.Jmbg);
            List<DateTime> terminiUzimanja = new List<DateTime>();
            int brojNovihPodsetnika = 0;
            if (recepti.Count > 0)
            {
                foreach (Recept r in recepti)
                {
                    DateTime dt = (r.DatumIzdavanja.AddDays(r.BrojDana)).Date;
                    if ((r.DatumIzdavanja.AddDays(r.BrojDana)).Date > DateTime.Today)
                    {
                        TimeSpan satVremena = new TimeSpan(1, 1, 0);
                        TimeSpan nula = new TimeSpan(0, 0, 0);
                        foreach (int i in r.TerminiUzimanjaLeka)
                        {
                            if ((DateTime.Today.AddHours(i) - DateTime.Now) < satVremena && (DateTime.Today.AddHours(i) - DateTime.Now) > nula)
                            {
                                if( ObavestenjaKontroler.getInstance().napraviPodsetnik(pacijent.Jmbg, r, i))
                                brojNovihPodsetnika++;
                            }
                        }
                    }
                }
            }
            if (brojNovihPodsetnika > 0)
            {
                primiObavestenja(brojNovihPodsetnika);
                if (MainViewModel.getInstance().CurrentView == MainViewModel.getInstance().ObavestenjaVM)
                {
                    view.Obavestenja.getInstance().PodsetnikTerapija.ItemsSource = ObavestenjaKontroler.getInstance().DobaviPodsetnikeZaTerapiju(pacijent.Jmbg);
                }

            }
        }

        private void Odjava(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }

        public void primiObavestenja(int brojPodsetnika)
        {
            int broj = Int32.Parse(brojObavestenja.Text);
            zvonceSaObevestenjem.Visibility = Visibility.Visible;
            brojObavestenja.Visibility = Visibility.Visible;
            broj += brojPodsetnika;
            brojObavestenja.Text = broj.ToString();
            okvirZaBrojObavestenja.Visibility = Visibility.Visible;
            zvonce.Visibility = Visibility.Hidden;

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zvonceSaObevestenjem.Visibility = Visibility.Hidden;
            okvirZaBrojObavestenja.Visibility = Visibility.Hidden;
            zvonce.Visibility = Visibility.Visible;
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().ObavestenjaVM;
        }
    }
}
