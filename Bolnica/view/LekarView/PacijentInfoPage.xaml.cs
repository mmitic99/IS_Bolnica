using Bolnica.model;
using Kontroler;
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
    /// Interaction logic for PacijentInfoPage.xaml
    /// </summary>
    public partial class PacijentInfoPage : Page
    {
        private static PacijentInfoPage instance = null;
        public ZdravstveniKarton Karton;
        public String Jmbg;

        public static PacijentInfoPage getInstance()
        {
            return instance;
        }
        public PacijentInfoPage(String jmbg)
        {
           
            
            InitializeComponent();
            instance = this;
            int k = 0;
            Jmbg = jmbg;
            ComboBox1.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            if (jmbg != null) {

                Pacijent pacijent = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
                ComboBox1.SelectedItem = pacijent.FullName;
                foreach (Pacijent p in SkladistePacijenta.GetInstance().GetAll())
                {
                    if (p.Jmbg.Equals(jmbg))
                    {
                        break;
                    }
                    k++;
                }
                ComboBox1.SelectedIndex = k;
                txt1.Text = pacijent.Ime;
                txt2.Text = pacijent.Prezime;
                if (pacijent.Pol == Model.Enum.Pol.Muski)
                    txt3.Text = "M";
                else
                    txt3.Text = "Ž";
                txt4.Text = pacijent.DatumRodjenja.ToString();
                txt5.Text = pacijent.Adresa;
                txt6.Text = "Oženjen";
                txt7.Text = "Posao";
                txt8.Text = pacijent.BrojTelefona;
                ObservableCollection<String> alergeni = new ObservableCollection<String>(pacijent.zdravstveniKarton.Alergeni);
                for (int i = 0; i < alergeni.Count(); i++)
                {
                    if (alergeni[i] == "Alergeni")
                        continue;

                    txt9.Text += alergeni[i];
                }
        }
    }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new IzdavanjeReceptaPage();
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pacijent pacijent = (Pacijent)ComboBox1.SelectedItem;
            Jmbg = pacijent.Jmbg;
            txt1.Text = pacijent.Ime;
            txt2.Text = pacijent.Prezime;
            if (pacijent.Pol == Model.Enum.Pol.Muski)
                txt3.Text = "M";
            else
                txt3.Text = "Ž";
            txt4.Text = pacijent.DatumRodjenja.ToString();
            txt5.Text = pacijent.Adresa;
            txt6.Text = "Oženjen";
            txt7.Text = "Posao";
            txt8.Text = pacijent.BrojTelefona;
            txt9.Text = "Alergeni";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarWindow.getInstance().lekar1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String dijalog = AnamnezaTxt.Text;
            Anamneza anamneza = new Anamneza(dijalog, DateTime.Today, LekarWindow.getInstance().lekar1.FullName);
            Pacijent pacijent = (Pacijent)ComboBox1.SelectedItem;
            pacijent.zdravstveniKarton.Anamneze.Add(anamneza);
            Pacijent pacijent1 = pacijent;
            PacijentKontroler.getInstance().izmeniPacijenta(pacijent, pacijent1);
            AnamnezaTxt.Clear();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(Jmbg);
        }
    }
}
