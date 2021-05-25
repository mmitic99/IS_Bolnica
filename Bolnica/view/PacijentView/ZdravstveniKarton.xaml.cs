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
    public partial class ZdravstveniKarton : UserControl
    {
        private MainViewModel MainViewModel;
        private ZdravstveniKartonViewModel ViewModel;
        public ZdravstveniKarton()
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.ViewModel = MainViewModel.ZdravstveniKartonVM;
            SetStartView();
        }

        private void SetStartView()
        {
            DataContext = ViewModel;
        }
    }

}
