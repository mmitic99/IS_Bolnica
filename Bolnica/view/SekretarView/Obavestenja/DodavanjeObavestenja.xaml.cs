using Bolnica.DTOs;
using Kontroler;
using System;
using System.Windows;
using System.Windows.Controls;
using Bolnica.ViewModel.SekretarViewModel;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for DodavanjeObavestenja.xaml
    /// </summary>
    public partial class DodavanjeObavestenja : Window
    {
        public DodavanjeObavestenja()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.DataContext = new DodavanjeObavestenjaViewModel();
        }
    }
}
