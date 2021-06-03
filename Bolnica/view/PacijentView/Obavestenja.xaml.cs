using Bolnica.viewActions;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Bolnica.view
{
    public partial class Obavestenja : UserControl
    {
        private ObavestenjaViewModel ViewModel;
        private MainViewModel MainViewModel;
        private ObavestenjaKontroler ObavestenjaKontroler;
        public static Obavestenja instance{get; set;}

        public Obavestenja()
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.ViewModel = MainViewModel.ObavestenjaVM;
            this.ObavestenjaKontroler = new ObavestenjaKontroler();
            instance = this;
            SetStartPage();
        }

        private void SetStartPage()
        {
            obavestenjaPacijenta.ItemsSource = ViewModel.obavestenja;
            PodsetnikTerapija.ItemsSource = ViewModel.podsetnici;
            dodajPodsetnikButton.Command = MainViewModel.DodavnjePodsetnikaCommand;
        }

        private void obavestenjaPacijenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (obavestenjaPacijenta.SelectedIndex != -1)
            {
                MainViewModel.PrikazObavestenjaVM = new PrikazJednogObavestenjaPacijentaViewModel(obavestenjaPacijenta.SelectedItem);
                MainViewModel.CurrentView = MainViewModel.PrikazObavestenjaVM;
            }
        }

        internal void RefresujPrikaz()
        {
            obavestenjaPacijenta.ItemsSource = ViewModel.obavestenja;
            PodsetnikTerapija.ItemsSource = ViewModel.podsetnici;
        }

        private void PodsetnikTerapija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
