﻿using Bolnica.Kontroler;
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
            MainViewModel mVM = new MainViewModel();
            mVM.ZdravstveniKartonVM = new ZdravstveniKartonViewModel(pacijent);
            DataContext = mVM;
            brojObavestenja.Text = "0";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(nabaviNovePodsetnike);
            timer.Tick += new EventHandler(posaljiKvartalneAnkete);
            timer.Tick += new EventHandler(odblokirajKorisnike);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void odblokirajKorisnike(object sender, EventArgs e)
        {
            KorisnickeAktivnostiPacijentaKontroler.GetInstance().OdblokirajKorisnike();
        }

        private void posaljiKvartalneAnkete(object sender, EventArgs e)
        {
            if (((DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(3)) //anketa prvog kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(6)) //anketa drugog kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(9)) //anketa treceg kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(12))) //anketa cetvrtog kvartala
                && !AnketeKontroler.GetInstance().DaLiJePoslataKvartalnaAnketa(DateTime.Today))
            {
                ObavestenjaKontroler.getInstance().PosaljiKvartalnuAnketu();
                primiObavestenja(1);
            }
                
        }

        public void nabaviNovePodsetnike(object sender, EventArgs e)
        {
            List<Recept> recepti= PacijentKontroler.GetInstance().DobaviRecepePacijenta(pacijent.Jmbg);
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
                                if(ObavestenjaKontroler.getInstance().napraviPodsetnik(pacijent.Jmbg, r, i))
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
            List<Recept> recepti = PacijentKontroler.GetInstance().DobaviRecepePacijenta(pacijent.Jmbg);
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
            Obavestenja.IsChecked = true;
        }
    }
}
