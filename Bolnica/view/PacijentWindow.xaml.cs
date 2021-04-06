﻿using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Model.Enum;
using System.Collections.ObjectModel;
using Repozitorijum;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PacijentWindow.xaml
    /// </summary>
    public partial class PacijentWindow : Window
    {
        Pacijent pacijent;
        public String JmbgPacijenta { get; set; }

        private static PacijentWindow instance = null;

        public static PacijentWindow getInstance()
        {
            return instance;
        }
        public PacijentWindow(Pacijent pacijent)
        {

            InitializeComponent();

            this.pacijent = pacijent;
            
          // Lekar l = new Lekar("Dragana", "Dusanovic", "2366");
           // SkladistePacijenta.GetInstance().Save(p);
           /* Prostorija pr = new Prostorija(Sprat.Cetvrti, "407B");
            Termin t = new Termin(pr, l, p, new DateTime(2021, 6, 27), 0.5, VrstaPregleda.Operacija);
            List<Termin> termins = new List<Termin>();
            termins.Add(t);
           SkladisteZaTermine.getInstance().SaveAll(termins);
            */
            DataContext = SkladistePacijenta.GetInstance().getByJmbg(pacijent.Jmbg);

            prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(pacijent.Jmbg));
            this.JmbgPacijenta = pacijent.Jmbg;
            instance = this;


        }


        private void prikazTermina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTermina(this.JmbgPacijenta);
            s.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (prikazTermina.SelectedIndex != -1)
            {
                List<Termin> termini = SkladisteZaTermine.getInstance().GetAll();
                termini.Remove((Termin)prikazTermina.SelectedItem);
                SkladisteZaTermine.getInstance().SaveAll(termini);
                SkladisteZaTermine.getInstance().RemoveByID(((Termin)prikazTermina.SelectedItem).IDTermina);
                prikazTermina.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(JmbgPacijenta));
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (prikazTermina.SelectedIndex != -1 && ((Termin)prikazTermina.SelectedItem).VrstaTermina!=VrstaPregleda.Operacija)
            {
                var s = new IzmenaTermina(((Termin)prikazTermina.SelectedItem).IDTermina);
                s.Show();
            }
        }
    }
}
