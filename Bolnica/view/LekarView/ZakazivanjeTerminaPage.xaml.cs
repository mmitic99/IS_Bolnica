using Bolnica.DTOs;
using Kontroler;
using Model;
using Model.Enum;
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

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for ZakazivanjeTerminaPage.xaml
    /// </summary>
    public partial class ZakazivanjeTerminaPage : Page

    {
        public static ZakazivanjeTerminaPage instance = null;
        String jmbgPacijenta;

        public ZakazivanjeTerminaPage(String jmbg)
        {
            InitializeComponent();
            instance = this;
            LekariBox.ItemsSource = new ObservableCollection<Lekar>(LekarKontroler.getInstance().GetAll());
            LekariBox.SelectedIndex = 0;
            jmbgPacijenta = jmbg;
            ProstorijaBox.ItemsSource = new ObservableCollection<Prostorija>(ProstorijeKontroler.GetInstance().GetAll());
            TerminBox.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
        }

        private void LekariBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lekar l = (Lekar)LekariBox.SelectedItem;
            int idProstorije = l.IdOrdinacija;
            int k = 0;
            if (idProstorije != null)
            {

                Prostorija prostorija = SkladisteZaProstorije.GetInstance().getById(idProstorije);
                //ComboBox1.SelectedItem = pacijent.FullName;
                foreach (Prostorija p in SkladisteZaProstorije.GetInstance().GetAll())
                {
                    if (p.IdProstorije.Equals(idProstorije))
                    {
                        break;
                    }
                    k++;
                }
                ProstorijaBox.SelectedIndex = k;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
            
        {

            
            if (TerminBox.SelectedItem.Equals(VrstaPregleda.Pregled))
            {
                ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
                {
                    Pacijent = jmbgPacijenta,
                    IzabraniDatumi =Kalendar.SelectedDates,
                    IzabraniLekar = LekariBox.SelectedItem,
                    PocetnaSatnica = 6,
                    PocetakMinut = 0,
                    KrajnjaSatnica = 20,
                    KrajnjiMinuti = 0,
                    NemaPrioritet = false,
                    OpisTegobe = txt1.Text,
                    PrioritetLekar = true,
                    PriotitetVreme = false,
                    trajanjeUMinutama = 30,
                    vrstaTermina = 0

                };
                LekarWindow.getInstance().Frame1.Content = new PrikazDostupnihTermina(parametriDTO);
            }
            else
            {
                ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
                {
                    Pacijent = jmbgPacijenta,
                    IzabraniDatumi = Kalendar.SelectedDates,
                    IzabraniLekar = LekariBox.SelectedItem,
                    PocetnaSatnica = 6,
                    PocetakMinut = 0,
                    KrajnjaSatnica = 20,
                    KrajnjiMinuti = 0,
                    NemaPrioritet = false,
                    OpisTegobe = txt1.Text,
                    PrioritetLekar = true,
                    PriotitetVreme = false,
                    trajanjeUMinutama = 60,
                    vrstaTermina = 1

                };
                LekarWindow.getInstance().Frame1.Content = new PrikazDostupnihTermina(parametriDTO);
            }
           

        }

    }
}
