using Bolnica.view.LekarView;
using Model;
using Model.Enum;
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

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for KreirajTerminPage.xaml
    /// </summary>
    public partial class KreirajTerminPage : Page
    {
        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        SkladisteZaProstorije skladprod = new SkladisteZaProstorije();
        public TerminiPage terminiPage;
        Lekar l;
        Termin termin;
        List<Termin> termini;
        public KreirajTerminPage(TerminiPage tp)
        {
            InitializeComponent();
            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox1.SelectedItem = null;
            ComboBox1.Text = "---izaberi--";
            ComboBox3.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            List<Prostorija> prostorije = skladprod.GetAll();
            ComboBox2.ItemsSource = prostorije;
            this.DataContext = this;


            ComboBox2.SelectedIndex = 0;



        }
        public void UcitajPodatke()
        {
            String vreme = txt1.Text;
            String trajanje = txt2.Text;
            Double trajanjeDou = Double.Parse(trajanje);
            VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
            Pacijent pa = new Pacijent { Ime = "Mihailo", Prezime = "Majstorovic", Jmbg = "123456789" };
            l = new Lekar("Milos", "Marinkovic", "6667");
            Prostorija p = (Prostorija)ComboBox2.SelectedItem;

            var vremeDataTime = DateTime.Parse(vreme);
            termin = new Termin { DatumIVremeTermina = vremeDataTime, IdProstorije = p.IdProstorije, TrajanjeTermina = trajanjeDou, VrstaTermina = pre, JmbgPacijenta = pa.Jmbg, JmbgLekara = l.Jmbg };
            termin.IDTermina = termin.generateRandId();
            SkladisteZaTermine.getInstance().Save(termin);
            TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbgLekar(termin.JmbgLekara));
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke();





        }
    }
}
