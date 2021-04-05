using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PregledWindow.xaml
    /// </summary>
    public partial class PregledWindow : Window
    {
        private static PregledWindow instance = null;

        public static PregledWindow getInstance()
        {
            return instance;
        }
        public List<Termin> Termini
        {
            get;
            set;
        }



        public DataGrid GetPregledi_Table()
        {
            return Pregledi_Table;
        }

        public void SetPregledi_Table(DataGrid value)
        {
            Pregledi_Table = value;
        }

        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        public PregledWindow()
        {
            InitializeComponent();
            this.DataContext = this;                         
            Termini = skladiste.getByJmbgLekar("6667");
            
            instance = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var s = new KreirajTerminWindow(this);
            s.Show();
            Pregledi_Table.Items.Refresh();
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            var s = new AzurirajTerminWindow(this);
            s.Show();
            Pregledi_Table.Items.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (GetPregledi_Table().SelectedIndex != -1)
            {
                SkladisteZaTermine.getInstance().RemoveByID(((Termin)Pregledi_Table.SelectedItem).IDTermina);
                PregledWindow.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbgLekar(((Termin)Pregledi_Table.SelectedItem).lekar.Jmbg));


            }
            else
                Console.WriteLine("Niste odabrali nijedan red.");
            Pregledi_Table.Items.Refresh();
        }

    }
}
