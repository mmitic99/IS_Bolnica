﻿using Model;
using Model.Enum;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AzurirajTerminWindow.xaml
    /// </summary>
    public partial class AzurirajTerminWindow : Window
    {
        public Termin selektovani { get; set; }
        private PregledWindow pregledWindow;
        Termin t = null;
        SkladisteZaTermine skladiste = new SkladisteZaTermine();
        SkladisteZaProstorije skladprost = new SkladisteZaProstorije();
        public AzurirajTerminWindow(PregledWindow pr)
        {
            InitializeComponent();
           
            ComboBox1.ItemsSource = Enum.GetValues(typeof(VrstaPregleda));
            ComboBox2.ItemsSource = skladprost.GetAll();
            pregledWindow = pr;
            this.selektovani = (Termin)pr.Pregledi_Table.SelectedItem;
            List<Termin> termini = skladiste.GetAll();
            
            this.DataContext = this;
            if (pregledWindow.Pregledi_Table.SelectedIndex != -1)
            {
                t = (Termin)PregledWindow.getInstance().Pregledi_Table.SelectedItem;
                txt1.Text = t.DatumIVremeTermina.ToString();
                txt2.Text = t.TrajanjeTermina.ToString();
                ComboBox1.SelectedItem = t.VrstaTermina;
                
              
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

         
       

            List<Termin> termini = SkladisteZaTermine.getInstance().GetAll();
            foreach (Termin t in termini)
            {
                if (t.IDTermina != null)
                {
                    if (t.IDTermina.Equals(this.selektovani.IDTermina))
                    {
                        String vreme = txt1.Text;
                        String trajanje = txt2.Text;
                        Double trajanjeDou = Double.Parse(trajanje);
                        VrstaPregleda pre = (VrstaPregleda)ComboBox1.SelectedItem;
                        Prostorija p = (Prostorija)ComboBox2.SelectedItem;
                        
                        
                        var vremeDataTime = DateTime.Parse(vreme);
                        t.DatumIVremeTermina = vremeDataTime;
                        t.TrajanjeTermina = trajanjeDou;
                        t.VrstaTermina = pre;
                        t.prostorija = p;
                        
                        
                    }
                }
            }
            
            SkladisteZaTermine.getInstance().SaveAll(termini);

            PregledWindow.getInstance().Pregledi_Table.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbgLekar(t.lekar.Jmbg));



            this.Close();
            
        }
    }
}
