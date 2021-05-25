using Bolnica.Kontroler;
using Bolnica.Repozitorijum.XmlSkladiste;
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
    /// Interaction logic for PrikazBolnickihLecenja.xaml
    /// </summary>
    public partial class PrikazBolnickihLecenja : Page
    {
        public PrikazBolnickihLecenja()
        {
            InitializeComponent();
            this.DataContext = this;
            Bolnicka_Lecenja_Table.ItemsSource = BolnickaLecenjaKontroler.GetInstance().GetAll();
        }
    }
}
