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
using Bolnica.model;

namespace Bolnica.view.PacijentView
{
public partial class PomeranjeTermina : UserControl
    {
        private MainViewModel MainViewModel;
        private PomeranjeTerminaViewModel ViewModel;
        private TerminKontroler TerminKontroler;
        private LekarKontroler LekarKontroler;

        public PomeranjeTermina()
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.ViewModel = MainViewModel.PomeranjeTerminaVM;
            this.TerminKontroler = new TerminKontroler();
            this.LekarKontroler = new LekarKontroler();
            SetStartPage();
       
        }

        private void SetStartPage()
        {
            DataContext = ViewModel.TerminZaPomeranje;
            kalendar.DisplayDateStart = TerminKontroler.PrviMoguciDanZakazivanja(ViewModel.TerminZaPomeranje);
            kalendar.DisplayDateEnd = TerminKontroler.PoslednjiMoguciDanZakazivanja(ViewModel.TerminZaPomeranje);
            izabraniLekar.ItemsSource = LekarKontroler.GetAll();
            izabraniLekar.SelectedIndex = LekarKontroler.DobaviIndeksSelektovanogLekara(ViewModel.TerminZaPomeranje);
        }

        private void Nazad_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
            {
                PrethodnoZakazaniTermin = ViewModel.TerminZaPomeranje,
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
            List<TerminDTO> moguciTermini = TerminKontroler.NadjiTermineZaParametre(parametriDTO);
            if(moguciTermini.Count>0)
            {
                MainViewModel.MoguciTerminiVM = new MoguciTerminiViewModel(moguciTermini, "izmena", moguciTermini[0].JmbgPacijenta);
                MainViewModel.CurrentView = MainViewModel.MoguciTerminiVM;
            }
            else
            {
                var s = new Upozorenje("Nema slobodnih termina po kriterijumima pretrage!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }

            
        }
    }
}
