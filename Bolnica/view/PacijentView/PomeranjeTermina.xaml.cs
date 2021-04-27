using Bolnica.viewActions;
using Kontroler;
using Model;
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
    /// Interaction logic for PomeranjeTermina.xaml
    /// </summary>
    public partial class PomeranjeTermina : UserControl
    {
        public List<Termin> moguciTermini;
        public Termin termin;
        public PomeranjeTermina()
        {
            InitializeComponent();
            this.termin = (Termin)PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem;
            DataContext = termin;
            satnicaKraj.SelectedIndex = 13;
            satnicaPocetak.SelectedIndex = 0;
            minutKraj.SelectedIndex = 1;
            minutPocetak.SelectedIndex = 0;
            kalendar.DisplayDateStart = termin.DatumIVremeTermina.AddDays(-3);
            kalendar.DisplayDateEnd = termin.DatumIVremeTermina.AddDays(3);
            kalendar.SelectedDate = termin.DatumIVremeTermina;
            izabraniLekar.ItemsSource = new ObservableCollection<Lekar>(LekarKontroler.getInstance().GetAll());
            izabraniLekar.SelectedIndex = selectedDoc(LekarKontroler.getInstance().GetAll(), termin);

        }

        public int selectedDoc(List<Lekar> lekari, Termin t)
        {
            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari.ElementAt(i).Jmbg.Equals(t.JmbgLekara))
                {
                    return i;
                }
            }
            return 0;
        }

            private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String jmbgPacijenta = this.termin.JmbgPacijenta;
            String jmbgLekara = ((Lekar)izabraniLekar.SelectedItem).Jmbg;
            List<DateTime> datumi = new List<DateTime>();
            datumi = new List<DateTime>(kalendar.SelectedDates);
            int satnicaZaKraj = satnicaKraj.SelectedIndex + 6;
            int minutiZaKraj = minutKraj.SelectedIndex * 30;
            int satnicaZaPocetak = satnicaPocetak.SelectedIndex + 6;
            int minutiZaPocetak = minutPocetak.SelectedIndex * 30;
            TimeSpan kraj = new TimeSpan(satnicaZaKraj, minutiZaKraj, 0);
            TimeSpan pocetak = new TimeSpan(satnicaZaPocetak, minutiZaPocetak, 0);
            int prioritet;
            String opisTegobe = tegobe.Text;
            if ((bool)nema.IsChecked)
            {
                prioritet = 0;
            }
            else if ((bool)lekar.IsChecked)
            {
                prioritet = 1;
            }
            else
            {
                prioritet = 2;
            }
            moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(jmbgLekara, jmbgPacijenta, datumi, pocetak, kraj, prioritet, opisTegobe, termin);
            if (moguciTermini.Count>0)
            {
                MainViewModel.getInstance().MoguciTerminiVM = new MoguciTerminiViewModel(moguciTermini, "izmena");
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().MoguciTerminiVM;
            }
        }
    }
}
