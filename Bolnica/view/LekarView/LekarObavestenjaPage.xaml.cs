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
    /// Interaction logic for LekarObavestenjaPage.xaml
    /// </summary>
    public partial class LekarObavestenjaPage : Page
    {
        public LekarObavestenjaPage()
        {
            InitializeComponent();
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
            LekarWindow.getInstance().Frame1.Content = new TerminiPage(LekarWindow.getInstance().lekar1);
        }

    }
}
    

