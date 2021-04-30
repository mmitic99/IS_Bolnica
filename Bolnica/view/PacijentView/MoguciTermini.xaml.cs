using Bolnica.viewActions;
using Kontroler;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MoguciTermini.xaml
    /// </summary>
    public partial class MoguciTermini : UserControl
    {
        public List<Termin> moguciTermini;
        public MoguciTermini()
        {
            InitializeComponent();
            prikazMogucih.ItemsSource = new ObservableCollection<Termin>(MainViewModel.getInstance().MoguciTerminiVM.terminiZaPrikazivanje);

        }

        private void Zakaži_Click(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.getInstance().MoguciTerminiVM.pozivaoc != null)
            {
                TerminKontroler.getInstance().IzmeniTermin((Termin)prikazMogucih.SelectedItem, ((Termin)PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem).IDTermina);
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.GetInstance().DodajOdlaganje(PacijentMainWindow.getInstance().pacijent.Jmbg);
            }
            else
            {
                if (KorisnickeAktivnostiPacijentaKontroler.GetInstance().DobaviBrojZakazanihPregledaUBuducnosti(PacijentMainWindow.getInstance().pacijent.Jmbg)>=4)
                {
                    var s = new UpozorenjePredBan();
                    s.Owner = PacijentMainWindow.getInstance();
                    s.ShowDialog();
                }
                TerminKontroler.getInstance().ZakaziTermin((Termin)prikazMogucih.SelectedItem);
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.GetInstance().DodajZakazivanje(PacijentMainWindow.getInstance().pacijent.Jmbg);
            }
                
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.getInstance().MoguciTerminiVM.pozivaoc != null)
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PomeranjeTerminaVM;

            }
            else
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentZakaziVM;
            }
        }
    }
}
