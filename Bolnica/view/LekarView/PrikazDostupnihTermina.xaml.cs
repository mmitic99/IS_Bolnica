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
using Bolnica.Repozitorijum.XmlSkladiste;

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
            List<TerminDTO> moguciTermini = new List<TerminDTO>();
            if (ZakazivanjeTerminaPage.getInstance().isHitan)
            {
               
                moguciTermini = TerminKontroler.getInstance().NadjiHitanTermin(PacijentInfoPage.getInstance().pacijent.Jmbg, ((LekarDTO)ZakazivanjeTerminaPage.getInstance().LekariBox.SelectedItem).Specijalizacija.ToString());
            }
            else
            {
                 moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametriDTO);
            }
            prikazMogucih.ItemsSource = new ObservableCollection<TerminDTO>(moguciTermini);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new ZakazivanjeTerminaPage(PacijentInfoPage.getInstance().pacijent.Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TerminKontroler.getInstance().ZakaziTermin((TerminDTO)prikazMogucih.SelectedItem);
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(((TerminDTO)prikazMogucih.SelectedItem).JmbgPacijenta);
        }

        
    }
}
