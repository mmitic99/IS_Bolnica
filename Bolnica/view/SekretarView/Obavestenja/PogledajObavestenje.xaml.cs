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
using Bolnica.DTOs;
using Bolnica.ViewModel.SekretarViewModel.ObavestenjaViewModel;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for PogledajObavestenje.xaml
    /// </summary>
    public partial class PogledajObavestenje : Window
    {
        public PogledajObavestenje(ObavestenjeDTO obavestenje)
        {
            InitializeComponent();
            this.DataContext = new PogledajObavestenjaViewModel(obavestenje);
        }
    }
}
