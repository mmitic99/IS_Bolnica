using Bolnica.viewActions;
using Kontroler;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bolnica.DTOs;

namespace Bolnica.view.PacijentView
{
    public partial class DodavanjePodsetnika : UserControl
    {
        private MainViewModel MainViewModel;
        private DodavanjePodsetnikaViewModel ViewModel;
        private ObavestenjaKontroler ObavestenjaKontroler;
        public DodavanjePodsetnika()
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.ViewModel = MainViewModel.DodavanjePodsetnikaVM;
            this.ObavestenjaKontroler = new ObavestenjaKontroler();
            SetStartView();
        }

        private void SetStartView()
        {
            kalendar.SelectedDate = DateTime.Today;
            kalendar.DisplayDateStart = DateTime.Today;
            Nazad.Command = MainViewModel.ObavestenjaCommand;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            KorisnickiPodsetnikFrontDTO PodsetnikDTO = new KorisnickiPodsetnikFrontDTO()
            {
                Datumi = kalendar.SelectedDates,
                JmbgKorisnika = ViewModel.pacijent,
                Minut = minut.Text,
                Sat = sat.Text,
                Naslov = naslov.Text,
                Sadrzaj = sadrzaj.Text
            };
            ObavestenjaKontroler.NapraviKorisnickiPodsetnik(PodsetnikDTO);
            MainViewModel.CurrentView = MainViewModel.ObavestenjaVM;
        }
    }
}
