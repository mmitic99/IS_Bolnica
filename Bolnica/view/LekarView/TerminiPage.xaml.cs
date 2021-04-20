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

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for TerminiPage.xaml
    /// </summary>
    public partial class TerminiPage : Page
    {
        private static TerminiPage instance = null;
        
        

        public static TerminiPage getInstance()
        {
            return instance;
        }
        public List<Termin> Termini
        {
            get;
            set;
        }


        SkladisteZaTermine skladiste = new SkladisteZaTermine();


        public TerminiPage(Lekar lekar)
        {
            InitializeComponent();
            DatePicker1.SelectedDate = DateTime.Today;
            this.DataContext = this;
            Termini = skladiste.getByJmbgLekar(lekar.Jmbg);
           
            instance = this;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            LekarWindow.getInstance().Frame1.Content = new KreirajTerminPage(this);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AzurirajTerminPage(this);


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (TerminiPage.getInstance().Pregledi_Table.SelectedIndex != -1)
            {
                SkladisteZaTermine.getInstance().RemoveByID(((Termin)Pregledi_Table.SelectedItem).IDTermina);
                LekarView.TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbgLekar(((Termin)Pregledi_Table.SelectedItem).JmbgLekara));


            }
            else
                Console.WriteLine("Niste odabrali nijedan red.");
            Pregledi_Table.Items.Refresh();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (TerminiPage.getInstance().Pregledi_Table.SelectedIndex != -1)
            {
                String jmbg = ((Termin)Pregledi_Table.SelectedItem).JmbgPacijenta;
                LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(jmbg);
            }
        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            /*String datum = DatePicker1.SelectedDate.ToString();
             var tbx = sender as DatePicker;
             if(tbx != DatePicker1.SelectedDate.Date)
             {

             }*/
             
        }
    }
}
