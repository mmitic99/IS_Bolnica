using Model;
using Repozitorijum;
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
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using Model.Enum;
using System.Collections.ObjectModel;
using Repozitorijum;
using Bolnica.viewActions;
using Kontroler;

namespace Bolnica.view
{


    public partial class PacijentZakazaniTermini : UserControl
    {
        Pacijent pacijent { get; set; }
        public String JmbgPacijenta { get; set; }

        private static PacijentZakazaniTermini instance = null;

        public static PacijentZakazaniTermini getInstance()
        {
            return instance;
        }

        public PacijentZakazaniTermini()
        {

            InitializeComponent();
            instance = this;
            this.pacijent = PacijentMainWindow.getInstance().pacijent;
            // Lekar l = new Lekar("Dragana", "Dusanovic", "2366");
            // SkladistePacijenta.GetInstance().Save(p);
            /* Prostorija pr = new Prostorija(Sprat.Cetvrti, "407B");
             Termin t = new Termin(pr, l, p, new DateTime(2021, 6, 27), 0.5, VrstaPregleda.Operacija);
             List<Termin> termins = new List<Termin>();
             termins.Add(t);
            SkladisteZaTermine.getInstance().SaveAll(termins);
             */
            //DataContext = SkladistePacijenta.GetInstance().getByJmbg(pacijent.Jmbg);
            DataContext = pacijent;

           prikazTermina1.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(pacijent.Jmbg));
           
            this.JmbgPacijenta = pacijent.Jmbg;
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentZakaziVM;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (prikazTermina1.SelectedIndex != -1 && ((Termin)prikazTermina1.SelectedItem).VrstaTermina != VrstaPregleda.Operacija)
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PomeranjeTerminaVM;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (prikazTermina1.SelectedIndex != -1)
            {
                TerminKontroler.getInstance().RemoveByID(((Termin)prikazTermina1.SelectedItem).IDTermina);
                prikazTermina1.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(JmbgPacijenta));
            }
        }

        private void prikazTermina1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (prikazTermina1.SelectedItem == null || (((Termin)prikazTermina1.SelectedItem).DatumIVremeTermina.Date.AddDays(-1) < DateTime.Today || ((Termin)prikazTermina1.SelectedItem).VrstaTermina == VrstaPregleda.Operacija))
            {
                otkaziButton.IsEnabled = false;
                izmeniButton.IsEnabled = false;
            }
            else
            {
                izmeniButton.IsEnabled = true;
                 otkaziButton.IsEnabled = true;
            }
            
        }
    }
}
