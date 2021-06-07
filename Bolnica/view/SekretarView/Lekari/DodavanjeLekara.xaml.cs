using Bolnica.DTOs;
using Kontroler;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.ViewModel.SekretarViewModel.LekariViewModel;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for DodavanjeLekara.xaml
    /// </summary>
    public partial class DodavanjeLekara : Window
    {
        public DodavanjeLekara()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            DataContext = new DodavanjeLekaraViewModel();
        }
    }
}
