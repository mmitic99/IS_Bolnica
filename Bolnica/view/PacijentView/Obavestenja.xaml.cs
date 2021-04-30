﻿using Bolnica.viewActions;
using Kontroler;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for Obavestenja.xaml
    /// </summary>
    public partial class Obavestenja : UserControl
    {
        public Pacijent pacijent;
        public static Obavestenja instance = null;
        public static Obavestenja getInstance()
        {
            return instance;
        }
        public Obavestenja()
        {
            this.pacijent = PacijentMainWindow.getInstance().pacijent;
            InitializeComponent();
            instance = this;
            obavestenjaPacijenta.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg(PacijentMainWindow.getInstance().pacijent.Jmbg);
            PodsetnikTerapija.ItemsSource = ObavestenjaKontroler.getInstance().DobaviPodsetnikeZaTerapiju(pacijent.Jmbg);
        }


        public void Execute(Action action, DateTime ExecutionTime)
        {
            Task WaitTask = Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            WaitTask.ContinueWith(_ => action);
            WaitTask.Start();
        }

        private void obavestenjaPacijenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (obavestenjaPacijenta.SelectedIndex != -1)
            {
                MainViewModel.getInstance().PrikazObavestenjaVM = new PrikazJednogObavestenjaPacijentaViewModel(obavestenjaPacijenta.SelectedItem);
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PrikazObavestenjaVM;
            }
        }
    }
}
