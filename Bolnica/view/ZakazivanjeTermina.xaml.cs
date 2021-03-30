using Model;
using Model.Skladista;
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
using System.Windows.Shapes;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for ZakazivanjeTermina.xaml
    /// </summary>
    public partial class ZakazivanjeTermina : Window
    {
        public Pacijent p;

        public ZakazivanjeTermina(String jmbg)
        {
            p = new Pacijent(jmbg);
            InitializeComponent();
            List<Lekar> lekari = new List<Lekar>();
            lekari.Add(new Lekar("Milos", "Marinkovic", "6667"));
            lekari.Add(new Lekar("Miroslav", "Mi", "4536"));
            izabraniLekar.ItemsSource = lekari;
            izabraniLekar.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Termin p = new Termin
            {
                pacijent = this.p,
                lekar = (Lekar)izabraniLekar.SelectedItem,
                TrajanjeTermina = 30,
                prostorija = new Prostorija(Model.Enum.Sprat.Drugi, "205"),
                VrstaTermina = Model.Enum.VrstaPregleda.Pregled, //jer pacijent sebi moze da zakaze samo pregled, ali ne moze i operaciju
                opisTegobe = tegobe.Text


            };
            if (datum.SelectedDate != null)
            {
                p.DatumIVremeTermina = (DateTime)datum.SelectedDate;
            }
            else
            {
                p.DatumIVremeTermina = DateTime.Now.AddDays(3);
            }

            p.IDTermina = p.generateRandId();
            SkladisteZaTermine.getInstance().Save(p);
            this.Close();
            PacijentWindow.getInstance().prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.pacijent.Jmbg));

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            PacijentWindow.getInstance().prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.Jmbg));

        }


    }
}
