using Bolnica.DTOs;
using Kontroler;
using System;
using System.Windows;
using System.Windows.Controls;
using Bolnica.ViewModel.SekretarViewModel;

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for IzmenaObavestenja.xaml
    /// </summary>
    public partial class IzmenaObavestenja : Window
    {
        public IzmenaObavestenja(ObavestenjeDTO selectedObavestenje)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.DataContext = new IzmenaObavestenjaViewModel(selectedObavestenje);
        }
    }
}
