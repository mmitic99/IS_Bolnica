using Bolnica.DTOs;
using Bolnica.viewActions;
using Kontroler;
using Model;
using Servis;
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
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.view.PacijentView
{
    public partial class ZakazivanjeTerminaP : UserControl
    {
        private PacijentZakaziTermin ViewModel;
        private MainViewModel MainViewModel;
        private TerminKontroler TerminKontroler;

        public ZakazivanjeTerminaP()
        {
            InitializeComponent();
            MainViewModel = MainViewModel.getInstance();
            ViewModel = MainViewModel.PacijentZakaziVM;
            this.TerminKontroler = new TerminKontroler();
            SetStartupPage();    
        }

        private void SetStartupPage()
        {
            izabraniLekar.ItemsSource = new ObservableCollection<LekarDTO>(LekarKontroler.getInstance().GetAll());
            izabraniLekar.SelectedIndex = 0;
            kalendar.DisplayDateStart = DateTime.Today.AddDays(1);
            kalendar.DisplayDateEnd = DateTime.Today.AddMonths(3); //moze se unapred zakazati termin 3 meseca
            satnicaPocetak.SelectedIndex = 0;
            satnicaKraj.SelectedIndex = 14;
            minutPocetak.SelectedIndex = 0;
            minutKraj.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
            {
                Pacijent = ViewModel.JmbgPacijenta,
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
            List<TerminDTO> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametriDTO);
            if (moguciTermini.Count > 0)
            {
                MainViewModel.MoguciTerminiVM = new MoguciTerminiViewModel(moguciTermini,null,ViewModel.JmbgPacijenta);
                MainViewModel.CurrentView = MainViewModel.MoguciTerminiVM;
            }
            else
            {
                var s = new Upozorenje("Nema slobodnih termina po traženim kriterujumima!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            PacijentZakazaniTermini.getInstance().RefresujPrikazTermina();
            MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM; 
        }
    }
}
