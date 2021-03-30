using Model;
using Model.Skladista;
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
using System.Windows.Shapes;
using Model.Enum;
using Model;
using Model.Skladista;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PacijentWindow.xaml
    /// </summary>
    public partial class PacijentWindow : Window
    {
        public String JmbgPacijenta { get; set; }

        private static PacijentWindow instance = null;

        public static PacijentWindow getInstance()
        {
            return instance;
        }
        public PacijentWindow()
        {

            InitializeComponent();

            String jmbg = "123456";
            Pacijent p = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
           Lekar l = new Lekar("Dragana", "Dusanovic", "2366");
            Prostorija pr = new Prostorija(Sprat.Cetvrti, "407B");
            Termin t = new Termin(pr, l, p, new DateTime(2021, 6, 27), 0.5, VrstaPregleda.Operacija);
            List<Termin> termins = new List<Termin>();
            termins.Add(t);
           SkladisteZaTermine.getInstance().SaveAll(termins);
            
            DataContext = SkladistePacijenta.GetInstance().getByJmbg(jmbg);

            prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(jmbg));
            this.JmbgPacijenta = jmbg;
            instance = this;


        }


        private void prikazTermina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTermina(this.JmbgPacijenta);
            s.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (prikazTermina.SelectedIndex != -1)
            {
                List<Termin> termini = SkladisteZaTermine.getInstance().GetAll();
                termini.Remove((Termin)prikazTermina.SelectedItem);
                SkladisteZaTermine.getInstance().SaveAll(termini);
                SkladisteZaTermine.getInstance().RemoveByID(((Termin)prikazTermina.SelectedItem).IDTermina);
                prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(JmbgPacijenta));
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (prikazTermina.SelectedIndex != -1 && ((Termin)prikazTermina.SelectedItem).VrstaTermina!=VrstaPregleda.Operacija)
            {
                var s = new IzmenaTermina(((Termin)prikazTermina.SelectedItem).IDTermina);
                s.Show();
            }
        }
    }
}
