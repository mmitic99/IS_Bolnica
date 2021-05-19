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
    /// <summary>
    /// Interaction logic for UpozorenjePredBan.xaml
    /// </summary>
    public partial class UpozorenjePredBan : Window
    {
        private String upozorenjeZa { get; set; }
        public UpozorenjePredBan(String upozorenjeZa)
        {
            InitializeComponent();
            this.upozorenjeZa = upozorenjeZa;
        }

        //nastavi
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if(upozorenjeZa.Equals("z"))
            {
                TerminKontroler.getInstance().ZakaziTermin((TerminDTO)MoguciTermini.GetInstance().prikazMogucih.SelectedItem);
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.GetInstance().DodajZakazivanje(PacijentMainWindow.getInstance().pacijent.Jmbg);
            }
           else if(upozorenjeZa.Equals("o"))
            {
                TerminKontroler.getInstance().RemoveByID(((Termin)PacijentZakazaniTermini.getInstance().prikazTermina1.SelectedItem).IDTermina);
                PacijentZakazaniTermini.getInstance().prikazTermina1.ItemsSource = new ObservableCollection<Termin>(TerminKontroler.getInstance().GetByJmbg(PacijentMainWindow.getInstance().pacijent.Jmbg));
                KorisnickeAktivnostiPacijentaKontroler.GetInstance().DodajOdlaganje(PacijentMainWindow.getInstance().pacijent.Jmbg);
            }
            else if(upozorenjeZa.Equals("p"))
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PomeranjeTerminaVM;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
        }
    }
}
