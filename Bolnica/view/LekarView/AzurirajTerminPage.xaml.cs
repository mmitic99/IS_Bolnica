using Model;
using Model.Enum;
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
using Kontroler;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for AzurirajTerminPage.xaml
    /// </summary>
    public partial class AzurirajTerminPage : Page
    {
        public Termin selektovani { get; set; }

        Termin t = null;
        private TerminKontroler terminKontroler = new TerminKontroler();
        private ProstorijeKontroler prostorijeKontroler = new ProstorijeKontroler();
        public AzurirajTerminPage(TerminiPage pr)
        {
            InitializeComponent();

            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox2.ItemsSource = prostorijeKontroler.GetAll();

            this.selektovani = (Termin)TerminiPage.getInstance().Pregledi_Table.SelectedItem;
            List<Termin> termini = terminKontroler.GetAll();

            this.DataContext = this;
            if (TerminiPage.getInstance().Pregledi_Table.SelectedIndex != -1)
            {
                t = (Termin)TerminiPage.getInstance().Pregledi_Table.SelectedItem;
                txt1.Text = t.DatumIVremeTermina.ToString();
                txt2.Text = t.TrajanjeTermina.ToString();
                ComboBox1.SelectedItem = t.VrstaTermina;



            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {




            List<Termin> termini = terminKontroler.GetAll();
            foreach (Termin t in termini)
            {
                if (t.IDTermina != null)
                {
                    if (t.IDTermina.Equals(this.selektovani.IDTermina))
                    {
                        String vreme = txt1.Text;
                        String trajanje = txt2.Text;
                        Double trajanjeDou = Double.Parse(trajanje);
                        VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
                        Prostorija p = (Prostorija)ComboBox2.SelectedItem;


                        var vremeDataTime = DateTime.Parse(vreme);
                        t.DatumIVremeTermina = vremeDataTime;
                        t.TrajanjeTermina = trajanjeDou;
                        t.VrstaTermina = pre;
                        t.IdProstorije = p.IdProstorije;


                    }
                }
            }

            terminKontroler.SaveAll(termini);

            TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(terminKontroler.GetByJmbgLekar(t.JmbgLekara));
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().GetByJmbg(t.JmbgLekara));
        }
    }
}
