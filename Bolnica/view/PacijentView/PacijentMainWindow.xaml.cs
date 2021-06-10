using Bolnica.Kontroler;
using Bolnica.viewActions;
using Kontroler;
using Model;
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
    public partial class PacijentMainWindow : Window
    {
        public static PacijentMainWindow instance = null;
        private ObavestenjaKontroler ObavestenjaKontroler;
        private PodsetniciKontroler PodsetniciKontroler;
        private MainViewModel MainViewModel;
        private KorisnickeAktivnostiPacijentaKontroler KorisnickeAktivnostiPacijentaKontroler;
        private AnketeKvartalneKontroler AnketeKvartalneKontroler;
        private AnketeOLekaruKontroler AnketeOLekaruKontroler;


        public static PacijentMainWindow getInstance()
        {
            return instance;
        }
        

        public PacijentMainWindow(MainViewModel PacijentMainViewModel)
        {
            InitializeComponent();
            instance = this;
            this.MainViewModel = PacijentMainViewModel;
            this.ObavestenjaKontroler = new ObavestenjaKontroler();
            this.PodsetniciKontroler = new PodsetniciKontroler();
            this.KorisnickeAktivnostiPacijentaKontroler = new KorisnickeAktivnostiPacijentaKontroler(MainViewModel.JmbgPacijenta);
            this.AnketeOLekaruKontroler = new AnketeOLekaruKontroler();
            this.AnketeKvartalneKontroler = new AnketeKvartalneKontroler();
            ZapocniRadAplikacije();

        }

        private void ZapocniRadAplikacije()
        {
            Ime.DataContext = MainViewModel.Pacijent;
            DataContext = MainViewModel;
            brojObavestenja.Text = "0";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(NabaviNovePodsetnike);
            timer.Tick += new EventHandler(PosaljiKvartalneAnkete);
            timer.Tick += new EventHandler(OdblokirajKorisnike);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void OdblokirajKorisnike(object sender, EventArgs e)
        {
            KorisnickeAktivnostiPacijentaKontroler.OdblokirajKorisnika();
        }

        private void PosaljiKvartalneAnkete(object sender, EventArgs e)
        {
            if (AnketeKvartalneKontroler.DaLiJeVremeZaKvartalnuAnketu())
            {
                ObavestenjaKontroler.PosaljiKvartalnuAnketu();
                PrimiObavestenja(1);
            }              
        }

        public void NabaviNovePodsetnike(object sender, EventArgs e)
        {
            int brojNovihPodsetnika = PodsetniciKontroler.nabaviNovePodsetnike(MainViewModel.Pacijent);
            if(brojNovihPodsetnika > 0)
            {
                PrimiObavestenja(brojNovihPodsetnika);
                if (MainViewModel.CurrentView == MainViewModel.ObavestenjaVM)
                {
                    view.Obavestenja.instance.RefresujPrikaz();
                }
            }
        }

        private void Odjava(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }

        public void PrimiObavestenja(int brojPodsetnika)
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
            Obavestenja.IsChecked = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MainViewModel.PrekiniDemo();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.PrekiniDemo();
        }
    }
}
