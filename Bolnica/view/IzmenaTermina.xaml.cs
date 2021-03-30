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
    public partial class IzmenaTermina : Window
    {
        public String IDPregleda { get; set; }
        public Termin p { get; set; }

        public IzmenaTermina(String id)
        {

            InitializeComponent();
            this.IDPregleda = id;
            List<Lekar> lekari = new List<Lekar>();
            lekari.Add(new Lekar("Milos", "Marinkovic", "6667"));
            lekari.Add(new Lekar("Miroslav", "Mi", "4536"));
            izabraniLekar.ItemsSource = lekari;
            this.p = SkladisteZaTermine.getInstance().getById(id);
            DataContext = p;

            izabraniLekar.SelectedIndex = selectedDoc(lekari, p);
        }

        public int selectedDoc(List<Lekar> lekari, Termin t)
        {
            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari.ElementAt(i).Jmbg.Equals(t.lekar.Jmbg))
                {
                    return i;
                }
            }
            return 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Termin> termini = SkladisteZaTermine.getInstance().GetAll();
            foreach (Termin t in termini)
            {
                if (t.IDTermina != null)
                {
                    if (t.IDTermina.Equals(this.IDPregleda))
                    {
                        t.lekar = (Lekar)izabraniLekar.SelectedItem;
                        if (datum.SelectedDate != null)
                        {
                            t.DatumIVremeTermina = (DateTime)datum.SelectedDate;
                        }
                        else
                        {
                            t.DatumIVremeTermina = DateTime.Now.AddDays(3);
                        }
                        t.opisTegobe = tegobe.Text;
                        break;
                    }
                }
            }
            SkladisteZaTermine.getInstance().SaveAll(termini);
            PacijentWindow.getInstance().prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.pacijent.Jmbg));

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
