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
using System.Windows.Shapes;
using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.viewActions;
using Kontroler;
using Model;
using Model.Enum;

namespace Bolnica.view.PacijentView
{
    public partial class UpozorenjePredBan : Window
    {
        private String upozorenjeZa { get; set; }
        private Object selectedItem;
        private TerminKontroler TerminKontroler;
        private KorisnickeAktivnostiPacijentaKontroler KorisnickeAktivnostiPacijentaKontroler;
        private MainViewModel MainViewModel;

        
       
        public UpozorenjePredBan(String upozorenjeZa, Object selectedItem)
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.upozorenjeZa = upozorenjeZa;
            this.selectedItem = selectedItem;
            this.TerminKontroler = new TerminKontroler();
            this.KorisnickeAktivnostiPacijentaKontroler = new KorisnickeAktivnostiPacijentaKontroler(MainViewModel.JmbgPacijenta);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if(upozorenjeZa.Equals("z"))
            {
                TerminKontroler.ZakaziTermin(selectedItem);
                MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.DodajZakazivanje(MainViewModel.Pacijent.Jmbg);
            }
           else if(upozorenjeZa.Equals("o"))
            {
                TerminKontroler.RemoveSelected(selectedItem);
                PacijentZakazaniTermini.getInstance().RefresujPrikazTermina();
                KorisnickeAktivnostiPacijentaKontroler.DodajOdlaganje(MainViewModel.Pacijent.Jmbg);
            }
            else if(upozorenjeZa.Equals("p"))
            {
                MainViewModel.CurrentView = MainViewModel.PomeranjeTerminaVM;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
        }
    }
}
