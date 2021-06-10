using Bolnica.DTOs;
using Kontroler;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private SpecijalizacijaKontroler specijalizacijaKontroler;
        String jmbgPacijenta;
        public bool isHitan;
        public static ZakazivanjeTerminaPage getInstance()
        {
            return instance;
        }

        public ZakazivanjeTerminaPage(String jmbg)
        {
            InitializeComponent();
            instance = this;
            specijalizacijaKontroler = new SpecijalizacijaKontroler();
            LekariBox.ItemsSource = new ObservableCollection<LekarDTO>(LekarKontroler.getInstance().GetAll());
            LekariBox.SelectedIndex = 0;
            jmbgPacijenta = jmbg;
            TerminBox.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            instance = this;
            isHitan = false;
            if(SpecBox!=null)
            SpecBox.ItemsSource = specijalizacijaKontroler.GetAll();
            setToolTip(LekarProfilPage.isToolTipVisible);
        }

        private void LekariBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LekarDTO l = (LekarDTO)LekariBox.SelectedItem;

        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
            
        {
            if (TerminBox.SelectedItem != null) { 
            if (LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,3}$"), txt2.Text)) {
                int proveraVremena = int.Parse(txt2.Text);
                if (proveraVremena % 30 == 0)
                {

                    int vrstaTermina = 0;
                    if ((VrstaPregleda)TerminBox.SelectedItem == VrstaPregleda.Operacija)
                    {
                        vrstaTermina = 1;
                    }
                    int SadasnjiSat = DateTime.Now.Hour;
                    int ComboSat = 0;
                    if (SadasnjiSat < 19 && SadasnjiSat > 6)
                    {
                        ComboSat = SadasnjiSat - 6;
                    }
                    int SadasnjiMinut;


                    if (terminCheckBox.IsChecked == true)
                    {
                        Kalendar.SelectedDates.Add(DateTime.Today);
                        ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
                        {
                            Pacijent = jmbgPacijenta,
                            IzabraniDatumi = Kalendar.SelectedDates,
                            IzabraniLekar = LekariBox.SelectedItem,
                            PocetnaSatnica = ComboSat,
                            PocetakMinut = 0,
                            KrajnjaSatnica = ComboSat + 1,
                            KrajnjiMinuti = 0,
                            NemaPrioritet = false,
                            OpisTegobe = txt1.Text,
                            PrioritetLekar = false,
                            PriotitetVreme = true,
                            trajanjeUMinutama = int.Parse(txt2.Text),
                            vrstaTermina = vrstaTermina

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
                            PocetnaSatnica = 0,
                            PocetakMinut = 0,
                            KrajnjaSatnica = 14,
                            KrajnjiMinuti = 0,
                            NemaPrioritet = false,
                            OpisTegobe = txt1.Text,
                            PrioritetLekar = true,
                            PriotitetVreme = false,
                            trajanjeUMinutama = int.Parse(txt2.Text),
                            vrstaTermina = vrstaTermina

                        };
                        LekarWindow.getInstance().Frame1.Content = new PrikazDostupnihTermina(parametriDTO);
                    }
                }
                else
                    MessageBox.Show("Trajanje mora biti deljivo sa 30 i manje od 361 minut !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                     MessageBox.Show("Pogrešan unos podataka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Izaberi vrstu termina !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(jmbgPacijenta);
        }

        public void terminCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(terminCheckBox.IsChecked== true)
            {
                isHitan = true;
            }
        }
        private void setToolTip(bool Prikazi)
        {


            if (Prikazi)
            {
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();
                this.Resources.Add(typeof(ToolTip), style);


            }
            else
            {
                this.Resources.Remove(typeof(ToolTip));
            }
        }
    }
}
