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
                ParametriZaMoguceTermineLekaraDTO parametriDTO = new ParametriZaMoguceTermineLekaraDTO()
                {
                    JmbgLekara = LekarWindow.getInstance().lekar1.Jmbg,
                    JmbgPacijenta = jmbgPacijenta,
                    DatumTermina = DatumPicker.SelectedDate.Value,
                    VremeskaDuzinaPrikaza = new TimeSpan(5, 0, 0),
                    TrajanjeUMinutama = 60,
                    VrstaTermina = (VrstaPregleda)TerminBox.SelectedItem,
                    DodatneNapomene = txt1.Text,
                    BrojSobe = ((Prostorija)ProstorijaBox.SelectedItem).BrojSobe,
                    HitnostTermina = false
                };
            }
            else
            {
                ParametriZaMoguceTermineLekaraDTO parametriDTO = new ParametriZaMoguceTermineLekaraDTO()
                {
                    JmbgLekara = LekarWindow.getInstance().lekar1.Jmbg,
                    JmbgPacijenta = jmbgPacijenta,
                    DatumTermina = DatumPicker.SelectedDate.Value,
                    VremeskaDuzinaPrikaza = new TimeSpan(5, 0, 0),
                    TrajanjeUMinutama = 30,
                    VrstaTermina = (VrstaPregleda)TerminBox.SelectedItem,
                    DodatneNapomene = txt1.Text,
                    BrojSobe = ((Prostorija)ProstorijaBox.SelectedItem).BrojSobe,
                    HitnostTermina = false
                };
            }

        }

    }
}
