using Bolnica.viewActions;
using Kontroler;
using Model;
using Repozitorijum;
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
    /// Interaction logic for ZakazivanjeTerminaP.xaml
    /// </summary>
    public partial class ZakazivanjeTerminaP : UserControl
    {
        public Pacijent p;
        public List<Termin> moguciTermini;
        public static ZakazivanjeTerminaP instance = null;
        public static ZakazivanjeTerminaP getInstance()
        {
            if(instance==null)
            {
                return new ZakazivanjeTerminaP();
            }
            else
            {
                return instance;
            }    
        }
        public ZakazivanjeTerminaP()
        {
            InitializeComponent();
            instance = this;
            izabraniLekar.ItemsSource = new ObservableCollection<Lekar>( LekarKontroler.getInstance().GetAll());
            izabraniLekar.SelectedIndex = 0;
            kalendar.DisplayDateStart = DateTime.Today.AddDays(1);
            kalendar.DisplayDateEnd = DateTime.Today.AddMonths(3); //moze se unapred zakazati termin 3 meseca
            satnicaPocetak.SelectedIndex = 0;
            satnicaKraj.SelectedIndex =14;
            minutPocetak.SelectedIndex = 0;
            minutKraj.SelectedIndex = 0;
            p = PacijentMainWindow.getInstance().pacijent;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String jmbgPacijenta = this.p.Jmbg;
            String jmbgLekara = ((Lekar)izabraniLekar.SelectedItem).Jmbg;
            List<DateTime> datumi = new List<DateTime>(kalendar.SelectedDates);
            int satnicaZaKraj = satnicaKraj.SelectedIndex + 6;
            int minutiZaKraj = minutKraj.SelectedIndex * 30;
            int satnicaZaPocetak = satnicaPocetak.SelectedIndex + 6;
            int minutiZaPocetak = minutPocetak.SelectedIndex * 30;
            TimeSpan kraj = new TimeSpan(satnicaZaKraj, minutiZaKraj, 0);
            TimeSpan pocetak = new TimeSpan(satnicaZaPocetak, minutiZaPocetak, 0);
            int prioritet;
            String opisTegobe = tegobe.Text;
            if ((bool)nema.IsChecked) {
                prioritet = 0;
            } 
            else if((bool)lekar.IsChecked) {
                prioritet = 1;
            } else
            {
                prioritet = 2;
            }
            this.moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(jmbgLekara, jmbgPacijenta, datumi, pocetak, kraj, prioritet, opisTegobe);
            if (moguciTermini.Count > 0)
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().MoguciTerminiVM;
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
      
            PacijentZakazaniTermini.getInstance().prikazTermina1.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.Jmbg));
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM; 

        }
    }
}
