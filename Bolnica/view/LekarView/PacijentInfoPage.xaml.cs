using Model;
using Repozitorijum;
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

        public static PacijentInfoPage getInstance()
        {
            return instance;
        }
        public PacijentInfoPage()
        {
            InitializeComponent();
            instance = this;
            ComboBox1.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            ComboBox1.SelectedIndex = 0;
            Pacijent pacijent = (Pacijent)ComboBox1.SelectedItem;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new IzdavanjeReceptaPage();
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pacijent pacijent = (Pacijent)ComboBox1.SelectedItem;
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

     
    }
}
