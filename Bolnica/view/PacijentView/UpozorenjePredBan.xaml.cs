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
using Bolnica.viewActions;
using Model.Enum;

namespace Bolnica.view.PacijentView
{
    /// <summary>
    /// Interaction logic for UpozorenjePredBan.xaml
    /// </summary>
    public partial class UpozorenjePredBan : Window
    {
        public UpozorenjePredBan()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PacijentTerminiVM;
        }
    }
}
