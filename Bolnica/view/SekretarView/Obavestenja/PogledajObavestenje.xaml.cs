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
using System.Windows.Shapes;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for PogledajObavestenje.xaml
    /// </summary>
    public partial class PogledajObavestenje : Window
    {
        public PogledajObavestenje(Obavestenje obavestenje)
        {
            InitializeComponent();
            datumIVremeObavestenja.Content += " " + obavestenje.VremeObavestenja.ToString("dd.MM.yyyy HH:mm");
            naslov.Text = obavestenje.Naslov;
            sadrzaj.Text = obavestenje.Sadrzaj;
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
