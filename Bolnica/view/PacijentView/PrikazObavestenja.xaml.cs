using Bolnica.viewActions;
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

namespace Bolnica.view.PacijentView
{
    /// <summary>
    /// Interaction logic for PrikazObavestenja.xaml
    /// </summary>
    public partial class PrikazObavestenja : UserControl
    {
        public PrikazObavestenja()
        {
            InitializeComponent();
            NaslovObavestenja.Text = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.Naslov;
            TekstObavestenja.Text = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.Sadrzaj;
        }
    }
}
