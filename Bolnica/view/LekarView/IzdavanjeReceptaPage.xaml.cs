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

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for IzdavanjeReceptaPage.xaml
    /// </summary>
    public partial class IzdavanjeReceptaPage : Page
    {
        private Recept recept;
        public Pacijent pacijent;
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
            txt5.Text = LekarWindow.getInstance().lekar1.FullName ;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            String imeLeka = txt9.Text;
            int brojDana = int.Parse(txt4.Text);
            DateTime datumIzdavanja = Datum.DisplayDate;
            String dijagonoza = txt12.Text;
            String sifraLeka = txt7.Text;
            
            String[] termini = txt8.Text.Split(',');
            List<int> terminiInt = new List<int>();
            for(int i = 0; i < termini.Length; i++)
            {
               String k = termini[i];
                terminiInt.Add(int.Parse(k));

            }
            String dodatneNapomene = txt11.Text;
            int doza = int.Parse(txt10.Text);
            recept = new Recept(imeLeka,sifraLeka,dodatneNapomene,datumIzdavanja,brojDana,doza,terminiInt,dijagonoza
                ,LekarWindow.getInstance().lekar1.FullName);
            List<Recept> recepti = new List<Recept>();
            recepti.Add(recept);
            Izvestaj izvestaj = new Izvestaj(recepti);
            List<Izvestaj> izvestaji = new List<Izvestaj>();
            izvestaji.Add(izvestaj);
            pacijent1.zdravstveniKarton.izvestaj = izvestaji;
            PacijentKontroler.getInstance().izmeniPacijenta(pacijent,pacijent1);
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }
    }
}
