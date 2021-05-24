using Model;
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
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using Model.Enum;
using System.Collections.ObjectModel;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Repozitorijum;
using Bolnica.viewActions;
using Kontroler;
using Bolnica.view.PacijentView;

namespace Bolnica.view
{
    public partial class PacijentZakazaniTermini : UserControl
    {

        private PacijentTerminiViewModel ViewModel;
        private MainViewModel MainViewModel;
        private KorisnickeAktivnostiPacijentaKontroler KorisnickeAktivnostiPacijentaKontroler;
        private TerminKontroler TerminKontroler;
        private static PacijentZakazaniTermini instance = null;
        public const int MAX_BROJ_OTKAZIVANJA = 3;

        public static PacijentZakazaniTermini getInstance()
        {
            return instance;
        }


        public PacijentZakazaniTermini()
        {
            InitializeComponent();
            instance = this;
            this.ViewModel = MainViewModel.getInstance().PacijentTerminiVM;
            this.MainViewModel = MainViewModel.getInstance();
            this.TerminKontroler = new TerminKontroler();
            this.KorisnickeAktivnostiPacijentaKontroler = new KorisnickeAktivnostiPacijentaKontroler();
            prikazTermina.ItemsSource = ViewModel.ZakazaniTerminiPacijenta;            

        }

        private void ZakazivanjeTerminaBC(object sender, RoutedEventArgs e)
        {
            if (KorisnickeAktivnostiPacijentaKontroler.DaLiJeMoguceZakazatiNoviTermin(ViewModel.JmbgPacijenta))
                zakaziButton.Command = MainViewModel.PacijentZakaziTerminCommand;
            else
            {
                var DijalogUpozorenja = new Upozorenje(KorisnickeAktivnostiPacijentaKontroler.DobaviPorukuZabrane(ViewModel.JmbgPacijenta));
                DijalogUpozorenja.Owner = PacijentMainWindow.getInstance();
                DijalogUpozorenja.ShowDialog();
            }    
        }
        
        private void IzmenaTerminaBC(object sender, RoutedEventArgs e)
        {
            MainViewModel.PomeranjeTerminaVM.SelektovanJeTermin(prikazTermina.SelectedItem);
            if (KorisnickeAktivnostiPacijentaKontroler.DaLiJeMoguceOdlozitiZakazaniTermin(ViewModel.JmbgPacijenta))
                if(KorisnickeAktivnostiPacijentaKontroler.DobaviBrojOtkazivanjaUProteklihMesecDana(ViewModel.JmbgPacijenta)>=MAX_BROJ_OTKAZIVANJA-1)
                {
                    var DijjalogPredBan = new UpozorenjePredBan("p", prikazTermina.SelectedItem);
                    DijjalogPredBan.Owner = PacijentMainWindow.getInstance();
                    DijjalogPredBan.ShowDialog();
                }
                else
                {
                    izmeniButton.Command = MainViewModel.PomeranjeTerminaCommand;

                }
            else
            {
                var DijalogUpozorenja = new Upozorenje(KorisnickeAktivnostiPacijentaKontroler.DobaviPorukuZabrane(ViewModel.JmbgPacijenta));
                DijalogUpozorenja.Owner = PacijentMainWindow.getInstance();          
                DijalogUpozorenja.ShowDialog();
            }

        }

        private void OtkazivanjeTerminaBC(object sender, RoutedEventArgs e)
        {
            if (KorisnickeAktivnostiPacijentaKontroler.DaLiJeMoguceOdlozitiZakazaniTermin(ViewModel.JmbgPacijenta))
            {
                if (KorisnickeAktivnostiPacijentaKontroler.DobaviBrojOtkazivanjaUProteklihMesecDana(ViewModel.JmbgPacijenta) >= MAX_BROJ_OTKAZIVANJA-1)
                {
                    var DijjalogPredBan = new UpozorenjePredBan("o", prikazTermina.SelectedItem);
                    DijjalogPredBan.Owner = PacijentMainWindow.getInstance();
                    DijjalogPredBan.ShowDialog();
                } 
                else
                {
                    TerminKontroler.RemoveSelected(prikazTermina.SelectedItem);
                    RefresujPrikazTermina();
                    KorisnickeAktivnostiPacijentaKontroler.DodajOdlaganje(ViewModel.JmbgPacijenta);
                }
            }
            else
            {
                var DijalogUpozorenja = new Upozorenje(KorisnickeAktivnostiPacijentaKontroler.DobaviPorukuZabrane(ViewModel.JmbgPacijenta));
                DijalogUpozorenja.Owner = PacijentMainWindow.getInstance();
                DijalogUpozorenja.ShowDialog();
            }
            
        }

        public void RefresujPrikazTermina()
        {
            prikazTermina.ItemsSource = ViewModel.ZakazaniTerminiPacijenta;
        }

        private void prikazTermina1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (prikazTermina.SelectedItem == null || !TerminKontroler.DaLiJeMoguceOtkazatiTermin(prikazTermina.SelectedItem))
            {
                otkaziButton.IsEnabled = false;
                izmeniButton.IsEnabled = false;
            }
            else
            {
                izmeniButton.IsEnabled = true;
                 otkaziButton.IsEnabled = true;
            }
          
        }

    }
}
