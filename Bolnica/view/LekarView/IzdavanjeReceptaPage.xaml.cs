using Bolnica.DTO;
using Bolnica.view.LekarView;
using Kontroler;
using Model;
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
using static Bolnica.DTO.ReceptDTO;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for IzdavanjeReceptaPage.xaml
    /// </summary>
    public partial class IzdavanjeReceptaPage : Page
    {
        public Recept recept { get; set; }
        public Pacijent pacijent { get; set; }
        public Pacijent pacijent1;
        public PacijentKontroler kontroler;

        public IzdavanjeReceptaPage()
        {
            InitializeComponent();
            pacijent = (Pacijent)PacijentInfoPage.getInstance().ComboBox1.SelectedItem;
            pacijent1 = (Pacijent)PacijentInfoPage.getInstance().ComboBox1.SelectedItem;
            txt1.Text = pacijent.Ime;
            txt2.Text = pacijent.Prezime;
            txt3.Text = pacijent.DatumRodjenja.ToShortDateString();
            txt5.Text = LekarWindow.getInstance().lekar1.FullName;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<int> terminiInt = LekarKontroler.getInstance().DobijTerminePijenja(txt8.Text);
            ReceptiDTO parametri = new ReceptiDTO()
            {
                terminiUzimanjaTokomDana = terminiInt,
                ImeLeka = txtImeLeka.Text,
                SifraLeka = txt7.Text,
                DodatneNapomene = txt11.Text,
                DatumIzdavanja = Datum.DisplayDate,
                BrojDana = int.Parse(txt4.Text),
                Doza = int.Parse(txt10.Text),
                Dijagnoza = txt12.Text,
                ImeDoktora = LekarWindow.getInstance().lekar1.FullName,
                p = pacijent,
                p1 = pacijent1
            };



            ObavestenjaKontroler.getInstance().PosaljiAnketuOLekaru(pacijent.Jmbg, LekarWindow.getInstance().lekar1.Jmbg);

            LekarKontroler.getInstance().IzdajRecept(parametri);
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }

        private void txtImeLeka_LostFocus(object sender, RoutedEventArgs e)
        {
            bool daLiJeAlergican = false;
            foreach (String alergen in pacijent.ZdravstveniKarton.Alergeni)
                if (txtImeLeka.Text.ToLower().Equals(alergen.ToLower()))
                    daLiJeAlergican = true;

            if (daLiJeAlergican)
            {
                MessageBox.Show("Pacijent alergican na uneti lek, unesi drugi lek!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                PotvrdiBtn.IsEnabled = false;
            }
            else
                PotvrdiBtn.IsEnabled = true;
        }
    }
}

