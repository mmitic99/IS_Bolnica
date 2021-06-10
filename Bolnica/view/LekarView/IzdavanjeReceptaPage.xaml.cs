using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.view.LekarView;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static Bolnica.DTOs.ReceptDTO;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for IzdavanjeReceptaPage.xaml
    /// </summary>
    public partial class IzdavanjeReceptaPage : Page
    {
        public ReceptDTO recept { get; set; }
        public PacijentDTO pacijent { get; set; }
        public PacijentDTO pacijent1;
        public PacijentKontroler PacijentKontroler;
        public ObavestenjaKontroler ObavestenjaKontroler;

        public IzdavanjeReceptaPage()
        {
            InitializeComponent();
            ObavestenjaKontroler = new ObavestenjaKontroler();
            pacijent = (PacijentDTO)PacijentInfoPage.getInstance().ComboBox1.SelectedItem;
            pacijent1 = pacijent;
            txt1.Text = pacijent.Ime;
            txt2.Text = pacijent.Prezime;
            Datum.Text = DateTime.Today.ToShortDateString();
            txt3.Text = pacijent.DatumRodjenja.ToShortDateString();
            txt5.Text = LekarKontroler.getInstance().trenutnoUlogovaniLekar().FullName;
            BolnickiLekBox.ItemsSource = LekKontroler.GetInstance().GetAll();
            setToolTip(LekarProfilPage.isToolTipVisible);



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,2}$"), txt8.Text) || LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,2}[,]{1}[0-9]{1,2}$"), txt8.Text) || LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}$"), txt8.Text) || LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}$"), txt8.Text) || LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}[,]{1}[0-9]{1,2}$"), txt8.Text))
            {
                List<int> terminiInt = LekarKontroler.getInstance().DobijTerminePijenja(txt8.Text);
                bool greska = false;
                foreach(int i in terminiInt)
                {
                    if (i > 24)
                        greska = true;
                }
                if (terminiInt.Count > 5)
                    greska = true;
                if (!greska)
                {
                    if (LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,3}$"), txt4.Text))
                    {
                        if (LekarKontroler.getInstance().Validiraj(new Regex(@"^[0-9]{1,3}$"), txt10.Text) && (int.Parse(txt10.Text) < 7))
                        {
                            ReceptiDTO parametri = new ReceptiDTO()
                            {
                                terminiUzimanjaTokomDana = terminiInt,
                                ImeLeka = txtImeLeka.Text,
                                SifraLeka = txt7.Text,
                                DodatneNapomene = txt11.Text,
                                DatumIzdavanja = DateTime.Today,
                                BrojDana = int.Parse(txt4.Text),
                                Doza = int.Parse(txt10.Text),
                                Dijagnoza = txt12.Text,
                                ImeDoktora = LekarKontroler.getInstance().trenutnoUlogovaniLekar().FullName,
                                p = pacijent,
                                p1 = pacijent

                            };
                            ObavestenjaKontroler.PosaljiAnketuOLekaru(pacijent.Jmbg, LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
                            LekarKontroler.getInstance().IzdajRecept(parametri);
                            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
                        }
                        else
                            MessageBox.Show("Ne može se piti više od 7 lekova odjednom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Izaberite broj dana izmedju 1 i 999 !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Maksimalan sat je 24 i maksimalan broj termina je 5 !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            MessageBox.Show("Neispravno uneti termini pića!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            




            
            
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
                txtImeLeka.Text = "";
            }
            else
                PotvrdiBtn.IsEnabled = true;
        }

        private void BolnickiLekBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BolnickiLekBox.SelectedItem != null)
            {
                LekDTO lek = (LekDTO)BolnickiLekBox.SelectedItem;
                txt7.Text = lek.SiftaLeka;
                txtImeLeka.Text = lek.NazivLeka;
                bool daLiJeAlergican = false;
                String[] sastojci = lek.SastavLeka.Split(',');
                List<String> sastojciList = new List<String>();
                for (int i = 0; i < sastojci.Length; i++)
                {
                    String k = sastojci[i];
                    sastojciList.Add(k);

                }

                foreach (String alergen in pacijent.ZdravstveniKarton.Alergeni)
                {
                    foreach(String sastojak in sastojciList)
                    if (sastojak.Trim().ToLower().Equals(alergen.Trim().ToLower()))
                        daLiJeAlergican = true;

                    if (daLiJeAlergican)
                    {
                        MessageBox.Show("Pacijent alergican na sastojak unetog leka, unesi drugi lek!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        PotvrdiBtn.IsEnabled = false;
                        BolnickiLekBox.SelectedItem = null;
                        txtImeLeka.Text = "";
                        txt7.Text = "";
                    }
                    else
                        PotvrdiBtn.IsEnabled = true;
                }

            }
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

