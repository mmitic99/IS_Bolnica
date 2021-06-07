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
using Bolnica.DTOs;
using Bolnica.model;

namespace Bolnica.view.PacijentView
{

    public partial class MoguciTermini : UserControl
    {

        public static MoguciTermini instance = null;
        private MoguciTerminiViewModel ViewModel;
        private MainViewModel MainViewModel;
        private TerminKontroler TerminKontroler;
        private KorisnickeAktivnostiPacijentaKontroler KorisnickeAktivnostiPacijentaKontroler;
        public const int MAX_BROJ_ZAKAZANIH = 5;

        public static MoguciTermini GetInstance()
        {
            return instance;
        }
        public MoguciTermini()
        {
            InitializeComponent();
            instance = this;
            ViewModel = MainViewModel.getInstance().MoguciTerminiVM;
            MainViewModel = MainViewModel.getInstance();
            prikazMogucih.ItemsSource = new ObservableCollection<TerminDTO>(MainViewModel.getInstance().MoguciTerminiVM.terminiZaPrikazivanje);
            this.TerminKontroler = new TerminKontroler();
            this.KorisnickeAktivnostiPacijentaKontroler = new KorisnickeAktivnostiPacijentaKontroler();

        }

        private void Zakaži_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.pozivaoc != null)
            {
                TerminKontroler.IzmeniTermin((TerminDTO) prikazMogucih.SelectedItem, PacijentZakazaniTermini.getInstance().prikazTermina.SelectedItem);
                MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.DodajOdlaganje(ViewModel.jmbg);
            }
            else
            {
                if (KorisnickeAktivnostiPacijentaKontroler.DobaviBrojZakazanihPregledaUBuducnosti(ViewModel.jmbg) >= MAX_BROJ_ZAKAZANIH-1)
                {
                    var s = new UpozorenjePredBan("z", prikazMogucih.SelectedItem);
                    s.Owner = PacijentMainWindow.getInstance();
                    s.ShowDialog();
                }
                else
                {
                    TerminKontroler.ZakaziTermin((TerminDTO)prikazMogucih.SelectedItem);
                    MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
                    KorisnickeAktivnostiPacijentaKontroler.GetInstance().DodajZakazivanje(ViewModel.jmbg);
                }
            }               
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.pozivaoc != null)
            {
                MainViewModel.CurrentView = MainViewModel.PomeranjeTerminaVM;

            }
            else
            {
                MainViewModel.CurrentView = MainViewModel.PacijentZakaziVM;
            }
        }
    }
}
