using Bolnica.DTOs;
using Kontroler;
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
    /// Interaction logic for LekarObavestenjaPage.xaml
    /// </summary>
    public partial class LekarObavestenjaPage : Page
    {
        public LekarObavestenjaPage()
        {
            InitializeComponent();
            List<ObavestenjeDTO> obavestenja = ObavestenjaKontroler.getInstance().GetOavestenjaByJmbg(LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
            List<ObavestenjeDTO> novaObavestenja = new List<ObavestenjeDTO>();
            foreach (ObavestenjeDTO obavestenje in obavestenja)
            {
                if (obavestenje.VremeObavestenja.Date.Equals(DateTime.Today))
                {
                    novaObavestenja.Add(obavestenje);
                }
            }
            ObavestenjaPrikaz.ItemsSource = new ObservableCollection<ObavestenjeDTO>(novaObavestenja);
            DatePicker1.SelectedDate = DateTime.Today;
            setToolTip(LekarProfilPage.isToolTipVisible);
        }



        private void MenuItem_Click_Pacijenti(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
        }


        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            LekarWindow.getInstance().Close();
            s.Show();
        }

        private void MenuItem_Click_Lekovi(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekoviPage();
        }

        private void MenuItem_Click_Termini(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarKontroler.getInstance().trenutnoUlogovaniLekar());
        }

        private void Button_Click_Prikazi(object sender, RoutedEventArgs e)
        {
            DateTime datum = DateTime.Parse(DatePicker1.Text);
            List<ObavestenjeDTO> obavestenja = ObavestenjaKontroler.getInstance().GetOavestenjaByJmbg(LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
            List<ObavestenjeDTO> novaObavestenja = new List<ObavestenjeDTO>();
            foreach(ObavestenjeDTO obavestenje in obavestenja)
            {
                if (obavestenje.VremeObavestenja.Date.Equals(DatePicker1.SelectedDate))
                {
                    novaObavestenja.Add(obavestenje);
                }
            }
            ObavestenjaPrikaz.ItemsSource = new ObservableCollection<ObavestenjeDTO>(novaObavestenja);
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ObavestenjaPrikaz.SelectedIndex != -1)
            {
                ObavestenjaKontroler.getInstance().ObrisiObavestenje((ObavestenjeDTO)ObavestenjaPrikaz.SelectedItem);
                List<ObavestenjeDTO> obavestenja = ObavestenjaKontroler.getInstance().GetOavestenjaByJmbg(LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg);
                List<ObavestenjeDTO> novaObavestenja = new List<ObavestenjeDTO>();
                foreach (ObavestenjeDTO obavestenje in obavestenja)
                {
                    if (obavestenje.VremeObavestenja.Date.Equals(DatePicker1.SelectedDate))
                    {
                        novaObavestenja.Add(obavestenje);
                    }
                }
                ObavestenjaPrikaz.ItemsSource = new ObservableCollection<ObavestenjeDTO>(novaObavestenja);
                
            }

            else MessageBox.Show("Označite pbaveštenje koji želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void MenuItem_Click_Profil(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new LekarProfilPage();
        }
    }
}
    

