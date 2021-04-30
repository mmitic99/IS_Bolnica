using Bolnica.DTOs;
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


public partial class PomeranjeTermina : UserControl
    {
        public PomeranjeTermina()
        {
            InitializeComponent();
            //this.termin = (Termin)PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem;
            DataContext = PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem;
            kalendar.DisplayDateStart = TerminKontroler.getInstance().PrviMoguciDanZakazivanja(PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem);
            kalendar.DisplayDateEnd = TerminKontroler.getInstance().PoslednjiMoguciDanZakazivanja(PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem);
            izabraniLekar.ItemsSource = LekarKontroler.getInstance().GetAll();
            izabraniLekar.SelectedIndex = LekarKontroler.getInstance().DobaviIndeksSelektovanogLekara(PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem);
        }



        private void Nazad_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
            {
                PrethodnoZakazaniTermin = PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem,
                IzabraniDatumi = kalendar.SelectedDates,
                IzabraniLekar = izabraniLekar.SelectedItem,
                PocetnaSatnica = satnicaPocetak.SelectedIndex,
                PocetakMinut = minutPocetak.SelectedIndex,
                KrajnjaSatnica = satnicaKraj.SelectedIndex,
                KrajnjiMinuti = minutKraj.SelectedIndex,
                NemaPrioritet = nema.IsChecked,
                OpisTegobe = tegobe.Text,
                PrioritetLekar = lekar.IsChecked,
                PriotitetVreme = vreme.IsChecked,
                trajanjeUMinutama = 30,
                vrstaTermina = 0
                
            };
            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametriDTO);
                MainViewModel.getInstance().MoguciTerminiVM = new MoguciTerminiViewModel(moguciTermini, "izmena");
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().MoguciTerminiVM;
            
        }
    }
}
