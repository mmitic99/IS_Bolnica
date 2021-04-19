﻿using Repozitorijum;
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

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for Obavestenja.xaml
    /// </summary>
    public partial class Obavestenja : UserControl
    {
        public Obavestenja()
        {
            InitializeComponent();
            obavestenjaPacijenta.ItemsSource = SkladisteZaObavestenja.GetInstance().GetByKorisnickoIme(PacijentMainWindow.getInstance().pacijent.Jmbg);
            
        }
    }
}
