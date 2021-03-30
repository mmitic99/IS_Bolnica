using Model;
using Model.Enum;
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
    /// Interaction logic for KreirajTerminWindow.xaml
    /// </summary>
   
    public partial class KreirajTerminWindow : Window
    {
       
        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        SkladisteZaProstorije skladprod = new SkladisteZaProstorije();
        public PregledWindow pregledWindow;
        Termin termin;
        List<Termin> termini;
        public KreirajTerminWindow(PregledWindow pw)
        {
            InitializeComponent();
            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox1.SelectedItem = null;
            ComboBox1.Text = "---izaberi--";
            ComboBox2.ItemsSource = skladprod.GetAll();
            this.DataContext = this;
            pregledWindow = pw;
            
            

        }
       public void UcitajPodatke()
        {
            String vreme = txt1.Text;
            String trajanje = txt2.Text;
            Double trajanjeDou = Double.Parse(trajanje);
            VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
            Pacijent pa = new Pacijent { Ime = "Mihailo", Prezime = "Majstorovic", Jmbg = "1234546789" };
            Prostorija p = (Prostorija)ComboBox2.SelectedItem;
            
            var vremeDataTime = DateTime.Parse(vreme);
            termin = new Termin { DatumIVremeTermina = vremeDataTime, prostorija = p, TrajanjeTermina = trajanjeDou, VrstaTermina = pre , pacijent = pa};
            
            pregledWindow.Pregledi_Table.Items.Refresh();
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke();
            pregledWindow.Termini.Add(termin);
            
            skladiste.Save(termin);
            pregledWindow.Pregledi_Table.Items.Refresh();
            this.Close();

        }
    }
}
