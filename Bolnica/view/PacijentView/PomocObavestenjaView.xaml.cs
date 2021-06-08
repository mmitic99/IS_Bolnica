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
    /// Interaction logic for PomocObavestenjaView.xaml
    /// </summary>
    public partial class PomocObavestenjaView : UserControl
    {
        public PomocObavestenjaView()
        {
            InitializeComponent();
            nazad.Command = MainViewModel.getInstance().VratiSeNazaCommand;
        }
    }
}
