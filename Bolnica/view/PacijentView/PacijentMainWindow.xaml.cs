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
            Thread t = new Thread(nabaviNoveRecepte);
            t.Start();
            //DataContext = pacijent;
            /* if (pacijent.Pol == Model.Enum.Pol.Muski)
             {
                 fotPacijenta.Source = new BitmapImage(new Uri("...//...//Images//patient.png");
             } else
             {
                 fotPacijenta.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/placeholder.png"));
             }*/
           //cekajObavestenja();
        }

        public async void nabaviNoveRecepte()
        {
            /*//ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds)
            List<Recept> recepti= PacijentKontroler.getInstance().dobaviRecepePacijenta(pacijent.Jmbg);
            List<DateTime> terminiUzimanja = new List<DateTime>();
            if(recepti.Count >0)
            { 
                foreach(Recept r in recepti)
                {
                    foreach(int i in r.terminiUzimanjaTokomDana)
                    {
                        TimeSpan ts = new TimeSpan(i, 0, 0);
                        DateTime datumIVremeUzimanja = DateTime.Today + ts;
                        terminiUzimanja.Add(datumIVremeUzimanja);
                        if(datumIVremeUzimanja > DateTime.Now)
                        {
                            TimeSpan napraviObavestenje = (datumIVremeUzimanja - DateTime.Now).Subtract(new TimeSpan(1,0,0));
                            LoopThroughtNumbers((int)napraviObavestenje.TotalMilliseconds);
                            //WaitHandle(WaitHandle)
                            //while(!DateTime.Now.Equals(napraviObavestenje))
                            ObavestenjaKontroler.getInstance().napraviPodsetnik(pacijent.Jmbg, r, datumIVremeUzimanja.Hour);
                            primiObavestenja();
                           // if(Oba)

                        }
                        

                    }
                }
            }

            DateTime ponoc = DateTime.Today.AddDays(0);
            LoopThroughtNumbers((int)(ponoc-DateTime.Now).TotalMilliseconds); //u ponoc se dobavljaju nova obavestenja
           // nabaviNoveRecepte();*/
        }

        private void Odjava(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }

        public void primiObavestenja()
        {
            this.Dispatcher.Invoke(() =>
            {
                zvonceSaObevestenjem.Visibility = Visibility.Visible;
                zvonce.Visibility = Visibility.Hidden;
            });

        }

        public async void LoopThroughtNumbers(int count)
        {
            Thread.Sleep(10000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zvonceSaObevestenjem.Visibility = Visibility.Hidden;
            zvonce.Visibility = Visibility.Visible;
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().ObavestenjaVM;
        }
    }
}
