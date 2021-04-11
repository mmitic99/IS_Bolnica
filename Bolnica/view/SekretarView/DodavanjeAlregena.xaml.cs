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
using System.Windows.Shapes;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for DodavanjeAlregena.xaml
    /// </summary>
    public partial class DodavanjeAlregena : Window
    {
        ObservableCollection<string> alergeni;
        public DodavanjeAlregena(ObservableCollection<string> alergeni)
        {
            InitializeComponent();
            this.alergeni = alergeni;
            alergen.Focus();
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if(!alergen.Text.Equals("") && !alergeni.Contains(alergen.Text))
            {
                alergeni.Add(alergen.Text);
                this.Close();
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
