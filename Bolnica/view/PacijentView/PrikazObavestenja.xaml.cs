using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.model;
using Bolnica.viewActions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bolnica.view.PacijentView
{
    public partial class PrikazObavestenja : UserControl
    {
        private PrikazJednogObavestenjaPacijentaViewModel ViewModel;
        private MainViewModel MainViewModel;
        private AnketeKvartalneKontroler AnketeKvartalneKontroler;
        private AnketeOLekaruKontroler AnketeOLekaruKontroler;
        
        public PrikazObavestenja()
        {
            InitializeComponent();
            MainViewModel = MainViewModel.getInstance();
            ViewModel = MainViewModel.PrikazObavestenjaVM;
            this.AnketeKvartalneKontroler = new AnketeKvartalneKontroler();
            this.AnketeOLekaruKontroler = new AnketeOLekaruKontroler();
            SetStarPage();

        }

        private void SetStarPage()
        {
            NaslovObavestenja.Text = ViewModel.obavestenje.Naslov;
            TekstObavestenja.Text = ViewModel.obavestenje.Sadrzaj;
            Nazad.Command = MainViewModel.ObavestenjaCommand;

            if (ViewModel.ZakacenaKvartalnaAnketa!=null)
            {
                KvartalnaAnketaDugme.Visibility = Visibility.Visible;
                MainViewModel.PrikazKvartalneAnketeVM = new PrikazKvartalneAnketeViewModel(ViewModel.ZakacenaKvartalnaAnketa);

            }
            else if (ViewModel.ZakacenaAnketaOLekaru != null)
            {
                AnketaLekarDugme.Visibility = Visibility.Visible;
                MainViewModel.AnketaOLekaruVM = new AnketaOLekaruViewModel(ViewModel.ZakacenaAnketaOLekaru);
            }
        }

        private void KvartalnaAnketaDugme_Click(object sender, RoutedEventArgs e)
        {
            if(AnketeKvartalneKontroler.DaLiJeKorisnikPopunioAnketu(MainViewModel.JmbgPacijenta, ViewModel.datumZakaceneKvartalne))
            {
                var s = new Upozorenje("Već ste popunili anketu!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
            else if (AnketeKvartalneKontroler.DaLiJeIstekloVremeZaPopunjavanjeAnkete(ViewModel.datumZakaceneKvartalne))
            {
                var s = new Upozorenje("Vreme za popunjavanje ove ankete je isteklo!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
            else
            {
                KvartalnaAnketaDugme.Command = MainViewModel.KvartalnaAnketaCommand;
            }
        }

        private void AnketaLekarDugme_Click(object sender, RoutedEventArgs e)
        {
            if (!AnketeOLekaruKontroler.DaLiJeKorisnikPopunioAnketu(ViewModel.obavestenje.anketaOLekaru))
            {
                AnketaLekarDugme.Command = MainViewModel.AnketaOLekaruCommand;
            }
            else
            {
                var s = new Upozorenje("Već ste popunili anketu!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }
    }
}
