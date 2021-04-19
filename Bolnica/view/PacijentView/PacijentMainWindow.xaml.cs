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
            //DataContext = pacijent;
            /* if (pacijent.Pol == Model.Enum.Pol.Muski)
             {
                 fotPacijenta.Source = new BitmapImage(new Uri("...//...//Images//patient.png");
             } else
             {
                 fotPacijenta.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/placeholder.png"));
             }*/
            //cekajObavestenja();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(nabaviNovePodsetnike);
            timer.Interval = new TimeSpan(0, 0, 5);
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
                    if ((r.DatumIzdavanja.AddDays(r.BrojDana)).Date >= DateTime.Today)
                    {
                        TimeSpan satVremena = new TimeSpan(1, 1, 0);
                         foreach(int i in r.TerminiUzimanjaLeka)
                        {
                            if((DateTime.Now - DateTime.Today.AddHours(i)) < satVremena || 1==1 )
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
                primiObavestenja();
            }
        }

       private void Odjava(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }

       public void primiObavestenja()
        {
                zvonceSaObevestenjem.Visibility = Visibility.Visible;
            zvonce.Visibility = Visibility.Hidden;         

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zvonceSaObevestenjem.Visibility = Visibility.Hidden;
            zvonce.Visibility = Visibility.Visible;
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().ObavestenjaVM;
        }
    }
}
