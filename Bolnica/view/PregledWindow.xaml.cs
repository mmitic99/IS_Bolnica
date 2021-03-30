using Model;
using Model.Skladista;
using System;
using System.Collections.Generic;
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

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PregledWindow.xaml
    /// </summary>
    public partial class PregledWindow : Window
    {
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
            Termini = skladiste.GetAll();
            Pregledi_Table.Items.Refresh();
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
                Termini.RemoveAt(GetPregledi_Table().SelectedIndex);
                skladiste.SaveAll(Termini);
            }
            else
                Console.WriteLine("Niste odabrali nijedan red.");
            Pregledi_Table.Items.Refresh();
        }

    }
}
