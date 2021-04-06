using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private Sekretar sekretar;

        public SekretarWindow(Sekretar sekretar)
        {
            InitializeComponent();

            this.sekretar = sekretar;
            
            imeS.Content = sekretar.Ime;
            prezimeS.Content = sekretar.Prezime;

            pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            terminiPrikaz.ItemsSource = SkladisteZaTermine.getInstance().GetBuduciTerminPacLekar();

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
            var s = new DodavanjeGostujucegPacijenta(pacijentiPrikaz);
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijenta(pacijentiPrikaz);
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();

            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaPacijenta(pacijentiPrikaz);
                s.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();

            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                pacijenti.RemoveAt(pacijentiPrikaz.SelectedIndex);
                SkladistePacijenta.GetInstance().SaveAll(pacijenti);
                pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            }
        }
    }
}
