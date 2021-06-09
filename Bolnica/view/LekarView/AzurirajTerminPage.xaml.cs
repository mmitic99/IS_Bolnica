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
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;
using Bolnica.DTOs;
using System.Text.RegularExpressions;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for AzurirajTerminPage.xaml
    /// </summary>
    public partial class AzurirajTerminPage : Page
    {
        public TerminDTO selektovani { get; set; }

        TerminDTO t = null;
        SkladisteZaTermineXml skladiste = new SkladisteZaTermineXml();
        SkladisteZaProstorijeXml skladprost = new SkladisteZaProstorijeXml();
        public AzurirajTerminPage(TerminiPage pr)
        {
            InitializeComponent();

            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox2.ItemsSource = skladprost.GetAll();

            this.selektovani = (TerminDTO)TerminiPage.getInstance().Pregledi_Table.SelectedItem;
            List<Termin> termini = skladiste.GetAll();
            setToolTip(LekarProfilPage.isToolTipVisible);
            this.DataContext = this;
            if (TerminiPage.getInstance().Pregledi_Table.SelectedIndex != -1)
            {
                List<String> sati = new List<String>();
                for (int i = 7; i <= 20; i++)
                {
                    sati.Add(i.ToString());
                }
                List<String> minuti = new List<String>();
                minuti.Add("00");
                minuti.Add("30");
                SatiBox.ItemsSource = sati;
                MinutiBox.ItemsSource = minuti;
                Kalendar.DisplayDateStart = DateTime.Today.AddDays(1);
                t = (TerminDTO)TerminiPage.getInstance().Pregledi_Table.SelectedItem;
               
                txt2.Text = t.TrajanjeTermina.ToString();
                ComboBox1.SelectedItem = t.VrstaTermina;



            }
        }
        private bool Validiraj(Regex sablon, String unos)
        {
            if (sablon.IsMatch(unos))
                return true;
            else
                return false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {


        if(SatiBox.SelectedItem!=null && MinutiBox.SelectedItem!=null && ComboBox2.SelectedItem != null) { 
            if (Validiraj(new Regex(@"^[0-9]{1,3}$"), txt2.Text))
            {
                int proveraVremena = int.Parse(txt2.Text);
                if (proveraVremena % 30 == 0)
                {
                    List<Termin> termini = SkladisteZaTermineXml.getInstance().GetAll();
                    foreach (Termin t in termini)
                    {
                        if (t.IDTermina != null)
                        {
                            if (t.IDTermina.Equals(this.selektovani.IDTermina))
                            {
                                DateTime datum = Kalendar.SelectedDate.Value;
                                String vreme = datum.ToString() + " " + SatiBox.SelectedItem + ":" + MinutiBox;
                                String trajanje = txt2.Text;
                                Double trajanjeDou = Double.Parse(trajanje);
                                VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
                                Prostorija p = (Prostorija)ComboBox2.SelectedItem;
                                int sat = int.Parse((String)SatiBox.SelectedItem);
                                int minut = int.Parse((String)MinutiBox.SelectedItem);
                                DateTime vremeDataTime = new DateTime(datum.Year, datum.Month, datum.Day, sat, minut, 0);
                                t.DatumIVremeTermina = vremeDataTime;
                                t.TrajanjeTermina = trajanjeDou;
                                t.VrstaTermina = pre;
                                t.IdProstorije = p.IdProstorije;


                            }
                        }
                    }

                    SkladisteZaTermineXml.getInstance().SaveAll(termini);

                    TerminiPage.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermineXml.getInstance().GetByJmbgLekar(t.JmbgLekara));
                    LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().GetByJmbg(t.JmbgLekara));
                }
                else
                    MessageBox.Show("Trajanje mora biti deljivo sa 30 i manje od 360 !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
                MessageBox.Show("Trajanje mora biti deljivo sa 30 i manje od 360 !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Popunite sve!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void setToolTip(bool Prikazi)
        {


            if (Prikazi)
            {
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();
                this.Resources.Add(typeof(ToolTip), style);


            }
            else
            {
                this.Resources.Remove(typeof(ToolTip));
            }
        }
    }
}
