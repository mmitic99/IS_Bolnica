using Model;
using System;
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
using Bolnica.Repozitorijum.XmlSkladiste;
using Servis;
using Bolnica.DTOs;
using Bolnica.Kontroler;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for AzurirajAnamnezuPage.xaml
    /// </summary>
    public partial class AzurirajAnamnezuPage : Page
    {
        public AnamnezaDTO anamnezaDTONova;
        public AzurirajAnamnezuPage(AnamnezaDTO anamneza)
        {
            InitializeComponent();
            this.DataContext = this;
            txt1.Text = anamneza.AnamnezaDijalog;
            anamnezaDTONova = anamneza;
            setToolTip(LekarProfilPage.isToolTipVisible);
        }

       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new AnamnezaPage(PacijentInfoPage.getInstance().Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String dijalog = txt1.Text;
            PacijentDTO pacijent = PacijentKontroler.GetInstance().GetByJmbg(PacijentInfoPage.getInstance().Jmbg);
            
            AnamnezaKontroler.GetInstance().IzmenaAnamneze(anamnezaDTONova.IdAnamneze, dijalog, pacijent);
            
            
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
