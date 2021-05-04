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

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for PrikazDostupnihTermina.xaml
    /// </summary>
    public partial class PrikazDostupnihTermina : Page
    {
        public PrikazDostupnihTermina(ParametriZaTrazenjeMogucihTerminaDTO parametriDTO)
        {
            InitializeComponent();
            this.DataContext = this;
            List<Termin> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametriDTO);
            prikazMogucih.ItemsSource = new ObservableCollection<Termin>(moguciTermini);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new ZakazivanjeTerminaPage(PacijentInfoPage.getInstance().pacijent.Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TerminKontroler.getInstance().ZakaziTermin((Termin)prikazMogucih.SelectedItem);
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(((Termin)prikazMogucih.SelectedItem).JmbgPacijenta);
        }

        
    }
}
