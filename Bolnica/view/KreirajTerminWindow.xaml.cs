﻿using Model;
using Model.Enum;
using Model.Skladista;
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

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for KreirajTerminWindow.xaml
    /// </summary>
   
    public partial class KreirajTerminWindow : Window
    {
       
        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        public PregledWindow pregledWindow;
        Termin termin;
        public KreirajTerminWindow(PregledWindow pw)
        {
            InitializeComponent();
            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox1.SelectedItem = null;
            ComboBox1.Text = "---izaberi--";
            //ComboBox2.ItemsSource = SkladisteZaProstorije.getAll();
            this.DataContext = this;
            pregledWindow = pw;
            
            

        }
       public void UcitajPodatke()
        {
            String vreme = txt1.Text;
            String trajanje = txt2.Text;
            Double trajanjeDou = Double.Parse(trajanje);
            VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
            Prostorija p = (Prostorija)ComboBox2.SelectedItem;
            
            var vremeDataTime = DateTime.Parse(vreme);
            termin = new Termin { DatumIVremeTermina = vremeDataTime, prostorija = p, TrajanjeTermina = trajanjeDou, VrstaTermina = pre };
            pregledWindow.Termini.Add(termin);
            pregledWindow.Pregledi_Table.Items.Refresh();
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke();
            skladiste.Save(termin);
            this.Close();

        }
    }
}
