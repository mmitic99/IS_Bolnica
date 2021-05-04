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
    /// <summary>
    /// Interaction logic for PrikazObavestenja.xaml
    /// </summary>
    public partial class PrikazObavestenja : UserControl
    {
        public PrikazObavestenja()
        {
            InitializeComponent();
            NaslovObavestenja.Text = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.Naslov;
            TekstObavestenja.Text = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.Sadrzaj;
          /*  KvartalnaAnketaDugme.Visibility = Visibility.Visible;
            MainViewModel.getInstance().PrikazKvartalneAnketeVM = new PrikazKvartalneAnketeViewModel(MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.kvartalnaAnketa);

            KvartalnaAnketaDugme.Command = MainViewModel.getInstance().KvartalnaAnketaCommand;*/


            Nazad.Command = MainViewModel.getInstance().ObavestenjaCommand;            
            if (MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.kvartalnaAnketa != new DateTime(0))
            {
                KvartalnaAnketaDugme.Visibility = Visibility.Visible;
                KvartalnaAnketa anketa = AnketeKontroler.GetInstance().GetByDate(MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.kvartalnaAnketa);
                MainViewModel.getInstance().PrikazKvartalneAnketeVM = new PrikazKvartalneAnketeViewModel(anketa);

            }
            else if(MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru!=null)
            {
                AnketaLekarDugme.Visibility = Visibility.Visible;
                AnketeKontroler.GetInstance().GrtAnketaOLekaruByJmbg(MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru.JmbgLekara);
                PrikacenaAnketaPoslePregledaDTO anketa = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru;
                MainViewModel.getInstance().AnketaOLekaruVM = new AnketaOLekaruViewModel(anketa);
            }
        }

        private void KvartalnaAnketaDugme_Click(object sender, RoutedEventArgs e)
        {
            if (!AnketeKontroler.GetInstance().DaLiJeKorisnikPopunioAnketu(PacijentMainWindow.getInstance().pacijent, MainViewModel.getInstance().PrikazKvartalneAnketeVM.anketa)
                && !AnketeKontroler.GetInstance().DaLiJeIstekloVremeZaPopunjavanjeAnkete(MainViewModel.getInstance().PrikazKvartalneAnketeVM.anketa))
            {
                KvartalnaAnketaDugme.Command = MainViewModel.getInstance().KvartalnaAnketaCommand;
            }
            else if(AnketeKontroler.GetInstance().DaLiJeKorisnikPopunioAnketu(PacijentMainWindow.getInstance().pacijent, MainViewModel.getInstance().PrikazKvartalneAnketeVM.anketa))
            {
                var s = new Upozorenje("Već ste popunili anketu!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
            else if (AnketeKontroler.GetInstance().DaLiJeIstekloVremeZaPopunjavanjeAnkete(MainViewModel.getInstance().PrikazKvartalneAnketeVM.anketa))
            {
                var s = new Upozorenje("Vreme za popunjavanje ove ankete je isteklo!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
            
        }

        private void AnketaLekarDugme_Click(object sender, RoutedEventArgs e)
        {
            if (!AnketeKontroler.GetInstance().DaLiJeKorisnikPopunioAnketu(MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru))
            {
                AnketaLekarDugme.Command = MainViewModel.getInstance().AnketaOLekaruCommand;
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
