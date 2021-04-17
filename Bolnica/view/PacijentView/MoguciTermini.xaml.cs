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

namespace Bolnica.view.PacijentView
{
    /// <summary>
    /// Interaction logic for MoguciTermini.xaml
    /// </summary>
    public partial class MoguciTermini : UserControl
    {
        public MoguciTermini()
        {
            InitializeComponent();
            prikazMogucih.ItemsSource = new ObservableCollection<Termin>(ZakazivanjeTerminaP.getInstance().moguciTermini);

        }

        private void Zakaži_Click(object sender, RoutedEventArgs e)
        {
            TerminKontroler.getInstance().ZakaziTermin((Termin)prikazMogucih.SelectedItem);
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
        }
    }
}
