﻿using Bolnica.view.SekretarView.Obavestenja;
using Bolnica.viewActions;
using Kontroler;
using Model;
using System;
using System.Windows;
using System.Windows.Threading;
using Bolnica.view.SekretarView.Lekari;
using Bolnica.view.SekretarView.Pacijenti;
using Bolnica.view.SekretarView.Termini;

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
        private ObavestenjaKontroler obavestenjaKontroler;
        private LekarKontroler lekarKontroler;

        public SekretarWindow(Sekretar sekretar)
        {
            InitializeComponent();

            pacijentKontroler = new PacijentKontroler();
            terminKontroler = new TerminKontroler();
            obavestenjaKontroler = new ObavestenjaKontroler();
            lekarKontroler = new LekarKontroler();

            this.sekretar = sekretar;

            imeS.Content = sekretar.Ime;
            prezimeS.Content = sekretar.Prezime;

            pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
            LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
            
            statusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();


            obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg(sekretar.Jmbg);


        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            statusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
        }

        private void dodavanjeGostujuceg_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeGostujucegPacijenta(pacijentiPrikaz, terminiPrikaz);
            s.ShowDialog();
        }

        private void dodavanjePacijenta_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijenta(pacijentiPrikaz);
            s.ShowDialog();
        }

        private void izmenaPacijenta_Click(object sender, RoutedEventArgs e)
        {
            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaPacijenta(pacijentiPrikaz);
                s.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da izmenite.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void brisanjePacijenta_Click(object sender, RoutedEventArgs e)
        {
            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabranog pacijenta?", "Brisanje pacijenta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    bool uspesno = pacijentKontroler.obrisiPacijenta(((Pacijent)pacijentiPrikaz.SelectedItem).Jmbg);

                    pacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void zakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, null, false);
            s.ShowDialog();
        }

        private void izmeniTer_Click(object sender, RoutedEventArgs e)
        {
            if (terminiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaTermina(terminiPrikaz);
                s.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati termin koji želite da izmenite.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void otkakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            if (terminiPrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da otkažete izabrani termin?", "Otkazivanje termina", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    terminKontroler.OtkaziTermin(((TerminPacijentLekar)terminiPrikaz.SelectedItem).termin);
                    terminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati termin koji želite da otkažete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void prijavljivanje_Click(object sender, RoutedEventArgs e)
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

        private void zakazivanjeTerminaZaOdabranogPacijenta_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, (Pacijent)pacijentiPrikaz.SelectedItem, false);
            s.ShowDialog();
        }

        private void pocetna_Selected(object sender, RoutedEventArgs e)
        {
            obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
        }

        private void dodajObavestenje_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeObavestenja(obavestenjaPrikaz);
            s.ShowDialog();
        }

        private void izmeniObavestenje_Click(object sender, RoutedEventArgs e)
        {
            if (obavestenjaPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaObavestenja(obavestenjaPrikaz);
                s.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da izmenite.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void obrisiObavestenje_Click(object sender, RoutedEventArgs e)
        {
            if(obavestenjaPrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete izabrano obaveštenje?", "Brisanje obaveštenja", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    bool uspesno = obavestenjaKontroler.obrisiObavestenje((Obavestenje)obavestenjaPrikaz.SelectedItem);
                obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void pogledajObavestenje_Click(object sender, EventArgs e)
        {
            if (obavestenjaPrikaz.SelectedIndex != -1)
            {
                var s = new PogledajObavestenje((Obavestenje)obavestenjaPrikaz.SelectedItem);
                s.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da pogledate.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Lekari_OnSelected(object sender, RoutedEventArgs e)
        {
            LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
        }

        private void dodavanjeLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeLekara(LekariPrikaz);
            s.ShowDialog();
        }

        private void izmenaLekara_Click(object sender, RoutedEventArgs e)
        {
            if (LekariPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaLekara(LekariPrikaz);
                s.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati lekara koga želite da izmenite.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void brisanjeLekara_Click(object sender, RoutedEventArgs e)
        {
            if (LekariPrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabranog lekara?", "Brisanje lekara", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    bool uspesno = lekarKontroler.ObrisiLekara(((Lekar)LekariPrikaz.SelectedItem).Jmbg);
                    if(uspesno)
                        LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
                    else
                    {
                        MessageBox.Show("Lekar nije uspešno obrisan.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ZakazivanjeTerminaZaOdabranogLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(terminiPrikaz, null, false, (Lekar)LekariPrikaz.SelectedItem);
            s.ShowDialog();
        }
    }
}
