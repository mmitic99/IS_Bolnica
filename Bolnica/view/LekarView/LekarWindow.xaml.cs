using Bolnica.view.LekarView;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.DTOs;
using Kontroler;
using Servis;
using Bolnica.Kontroler;

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
        public Lekar lekarTrenutni;
        

        SkladisteZaTermineXml skladiste = new SkladisteZaTermineXml();
        public LekarWindow(LekarDTO lekar)
        {

            InitializeComponent();
            this.DataContext = this;                        
            Frame1.Content = new TerminiPage(lekar);

            lekarTrenutni = LekarServis.getInstance().GetByJmbg(lekar.Jmbg);
            BolnickaLecenjaKontroler.GetInstance().OtpustanjePacijenata();
            instance = this;
        }


       
    }   
}
