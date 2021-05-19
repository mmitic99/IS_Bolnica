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
using Bolnica.model;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for TerminiPage.xaml
    /// </summary>
    public partial class TerminiPage : Page
    {
        private static TerminiPage instance = null;
        public String FullIme;


        public static TerminiPage getInstance()
        {
            return instance;
        }
        public List<Termin> Termini
        {
            get;
            set;
        }





        public TerminiPage(Lekar lekar)
        {
            InitializeComponent();
            DatePicker1.SelectedDate = DateTime.Today;
            this.DataContext = this;
            Termini = SkladisteZaTermine.getInstance().getByDateForLekar(DateTime.Now.Date, lekar.Jmbg);
            ImeDoktora.DataContext = lekar;
            instance = this;
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
                LekarView.TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByDateForLekar(DatePicker1.SelectedDate.Value, ((Termin)Pregledi_Table.SelectedItem).JmbgLekara));


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



        private void MenuItem_Click_Pacijenti(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DateTime datum = DateTime.Parse(DatePicker1.Text);
            Termini = SkladisteZaTermine.getInstance().getByDateForLekar(datum.Date, LekarWindow.getInstance().lekar1.Jmbg);
            LekarView.TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(Termini);
            //TerminiPage.getInstance().DatumTermina.SortDirection = 0;
            TerminiPage.getInstance().VremeTermina.SortDirection = System.ComponentModel.ListSortDirection.Ascending;

        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }

        private void MenuItem_Click_Lekovi(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekoviPage();
        }

        private void MenuItem_Click_Obavestenja(object sender, RoutedEventArgs e)

        {
            LekarWindow.getInstance().Frame1.Content = new LekarObavestenjaPage();
        }


    }
       
}
