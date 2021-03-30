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
    /// Interaction logic for AzurirajTerminWindow.xaml
    /// </summary>
    public partial class AzurirajTerminWindow : Window
    {
        
        private PregledWindow pregledWindow;
        Termin t = null;
        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        public AzurirajTerminWindow(PregledWindow pr)
        {
            InitializeComponent();
            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            //ComboBox2.ItemsSource = SkladisteZaProstorija.getAll();
            pregledWindow = pr;
            List<Termin> termini = skladiste.GetAll();
            
            this.DataContext = this;
            if (pregledWindow.Pregledi_Table.SelectedIndex != -1)
            {
                t = pregledWindow.Termini[pregledWindow.Pregledi_Table.SelectedIndex];
                txt1.Text = t.DatumIVremeTermina.ToString();
                txt2.Text = t.TrajanjeTermina.ToString();
                ComboBox1.SelectedItem = t.VrstaTermina;
               // termini.Remove(t);
               // pr.Termini.RemoveAt(pregledWindow.Pregledi_Table.SelectedIndex);
                //skladiste.SaveAll(termini);
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            t.SetProstorija(p);
            pregledWindow.Pregledi_Table.Items.Refresh();
            skladiste.Save(t);
            
            this.Close();
            
        }
    }
}
