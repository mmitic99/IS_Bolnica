using Model;
using System;
using Repozitorijum;
using Kontroler;
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
using Servis;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for AzurirajAnamnezuPage.xaml
    /// </summary>
    public partial class AzurirajAnamnezuPage : Page
    {
        public Anamneza anamneza1;
        public AzurirajAnamnezuPage(Anamneza anamneza)
        {
            InitializeComponent();
            this.DataContext = this;
            txt1.Text = anamneza.AnamnezaDijalog;
            anamneza1 = anamneza;
        }

       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(PacijentInfoPage.getInstance().Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            Pacijent pacijent = SkladistePacijenta.GetInstance().getByJmbg(PacijentInfoPage.getInstance().Jmbg);
            Pacijent p1 = pacijent;
            pacijent.zdravstveniKarton.IzmenaAnamneze(anamneza1.IdAnamneze, txt1.Text);
            PacijentServis.GetInstance().IzmeniPacijenta(pacijent, p1);
            //LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(pacijent.Jmbg);
        }
    }
}
