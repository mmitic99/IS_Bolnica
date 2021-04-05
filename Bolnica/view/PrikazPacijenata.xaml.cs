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
using System.Windows.Shapes;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PrikazPacijenata.xaml
    /// </summary>
    public partial class PrikazPacijenata : Window
    {
        public PrikazPacijenata()
        {
            InitializeComponent();
            pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();

            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                var s = new IzmenaPacijenta(pacijentiPrikaz);
                s.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = SkladistePacijenta.GetInstance().GetAll();

            if (pacijentiPrikaz.SelectedIndex != -1)
            {
                pacijenti.RemoveAt(pacijentiPrikaz.SelectedIndex);
                SkladistePacijenta.GetInstance().SaveAll(pacijenti);
                pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            }
        }
    }
}
