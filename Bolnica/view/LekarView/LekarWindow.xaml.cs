using Bolnica.view.LekarView;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PregledWindow.xaml
    /// </summary>
    public partial class LekarWindow : Window
    {
        private static LekarWindow instance = null;

        
        public static LekarWindow getInstance()
        {
            return instance;
        }
        public List<Termin> Termini
        {
            get;
            set;
        }
        public Lekar lekar1;
        

        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        public LekarWindow(Lekar lekar)
        {

            InitializeComponent();
            this.DataContext = this;                         
            Termini = skladiste.getByJmbgLekar(lekar.Jmbg);
            Frame1.Content = new TerminiPage(lekar);
            lekar1 = lekar;
            instance = this;
        }


       
    }   
}
