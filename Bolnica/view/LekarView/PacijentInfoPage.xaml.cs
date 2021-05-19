﻿
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
using Bolnica.Repozitorijum.XmlSkladiste;
using Servis;

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
        public Pacijent pacijent;

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
            ImeDoktora.DataContext = LekarWindow.getInstance().lekar1;
            ComboBox1.ItemsSource = SkladistePacijentaXml.GetInstance().GetAll();
            if (jmbg != null)
            {

                 pacijent = SkladistePacijentaXml.GetInstance().GetByJmbg(jmbg);
                ComboBox1.SelectedItem = pacijent.FullName;
                foreach (Pacijent p in SkladistePacijentaXml.GetInstance().GetAll())
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

                txt4.Text = pacijent.DatumRodjenja.ToShortDateString();
                txt5.Text = pacijent.Adresa;
                txt6.Text = "Oženjen";
                txt7.Text = "Posao";
                txt8.Text = pacijent.BrojTelefona;
                ObservableCollection<String> alergeni = new ObservableCollection<String>(pacijent.ZdravstveniKarton.Alergeni);
                String alergeniString = String.Join(",", alergeni);
                txt9.Text = alergeniString;
            }
    }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new IzdavanjeReceptaPage();
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             pacijent = (Pacijent)ComboBox1.SelectedItem;
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
            ObservableCollection<String> alergeni = new ObservableCollection<String>(pacijent.ZdravstveniKarton.Alergeni);
            String alergeniString = String.Join(",", alergeni);
            txt9.Text = alergeniString;
        }

        private void MenuItem_Click_Termini(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarWindow.getInstance().lekar1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String dijalog = AnamnezaTxt.Text;
            Anamneza anamneza = new Anamneza(dijalog, DateTime.Now, LekarWindow.getInstance().lekar1.FullName);
            Pacijent pacijent = (Pacijent)ComboBox1.SelectedItem;
            pacijent.ZdravstveniKarton.Anamneze.Add(anamneza);
            Pacijent pacijent1 = pacijent;
            PacijentServis.GetInstance().IzmeniPacijenta(pacijent, pacijent1);
            AnamnezaTxt.Clear();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(Jmbg);
        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new ZakazivanjeTerminaPage(Jmbg);
        }

        private void MenuItem_Click_Lekovi(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekoviPage();
        }

        private void MenuItem_Click_Obavestenja(object sender, RoutedEventArgs e)

        {
            LekarWindow.getInstance().Frame1.Content = new LekarObavestenjaPage();
        }

    }
}
