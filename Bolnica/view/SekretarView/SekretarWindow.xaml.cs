using Bolnica.viewActions;
using Kontroler;
using Model;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private Sekretar sekretar;
        private PacijentKontroler pacijentKontroler;
        private TerminKontroler terminKontroler;

        public SekretarWindow(Sekretar sekretar)
        {
            InitializeComponent();

            pacijentKontroler = new PacijentKontroler();
            terminKontroler = new TerminKontroler();

            this.sekretar = sekretar;

            imeS.Content = sekretar.Ime;
            prezimeS.Content = sekretar.Prezime;

            pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();

            statusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            statusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeGostujucegPacijenta(pacijentiPrikaz, terminiPrikaz);
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijenta(pacijentiPrikaz);
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaPacijenta(pacijentiPrikaz);
                s.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                bool uspesno = pacijentKontroler.obrisiPacijentaNaIndeksu(pacijentiPrikaz.SelectedIndex);

                pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, null, false);
            s.Show();
        }

        private void izmeniTer_Click(object sender, RoutedEventArgs e)
        {
            if (terminiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaTermina(terminiPrikaz);
                s.Show();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (terminiPrikaz.SelectedIndex != -1)
            {
                terminKontroler.OtkaziTermin(((TerminPacijentLekar)terminiPrikaz.SelectedItem).termin);
                terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("s");
            this.Close();
            s.Show();
        }

        private void pacijenti_Selected(object sender, RoutedEventArgs e)
        {
            pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
        }

        private void termini_Selected(object sender, RoutedEventArgs e)
        {
            terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, (Pacijent)pacijentiPrikaz.SelectedItem, false);
            s.Show();
        }
    }
}
