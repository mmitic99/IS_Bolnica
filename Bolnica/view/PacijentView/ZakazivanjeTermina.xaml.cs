﻿using Model;
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
    /// Interaction logic for ZakazivanjeTermina.xaml
    /// </summary>
    public partial class ZakazivanjeTermina : Window
    {
        public Pacijent p;

        public ZakazivanjeTermina(String jmbg)
        {
            p = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
            InitializeComponent();
            List<Lekar> lekari = new List<Lekar>();
            /*lekari.Add(new Lekar("Milos", "Marinkovic", "6667"));
            lekari.Add(new Lekar("Miroslav", "Mi", "4536"));
            lekari.Add(new Lekar("Dunja", "Jovanovic", "609"));
            SkladisteZaLekara.GetInstance().SaveAll(lekari);*/
            lekari = SkladisteZaLekara.GetInstance().GetAll();
            izabraniLekar.ItemsSource = lekari;
            izabraniLekar.SelectedIndex = 0;
            sat.SelectedIndex = 0;
            minut.SelectedIndex = 0;
            datum.DisplayDateStart = DateTime.Today.AddDays(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Termin p = new Termin
            {
                JmbgPacijenta = this.p.Jmbg,
                JmbgLekara = ((Lekar)izabraniLekar.SelectedItem).Jmbg,
                TrajanjeTermina = 30,
                IdProstorije = 205,
                VrstaTermina = Model.Enum.VrstaPregleda.Pregled, //jer pacijent sebi moze da zakaze samo pregled, ali ne moze i operaciju
                opisTegobe = tegobe.Text


            };

            if (datum.SelectedDate != null)
            {
                DateTime dt = (DateTime)datum.SelectedDate;
                int hours = (int)sat.SelectedIndex+6;
                int minutes = 30;
                if (minut.SelectedIndex == 0)
                {
                    minutes = 0;
                }
                DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, 0);
                p.DatumIVremeTermina = dt1;
            }
            else
            {
                DateTime dt = DateTime.Now.AddDays(3);
                p.DatumIVremeTermina = new DateTime(dt.Year, dt.Month, dt.Day, 13, 0, 0);

            }

            p.IDTermina = p.generateRandId();
            SkladisteZaTermine.getInstance().Save(p);
            this.Close();

            PacijentZakazaniTermini.getInstance().prikazTermina1.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.JmbgPacijenta));


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            PacijentZakazaniTermini.getInstance().prikazTermina1.ItemsSource = new ObservableCollection<Termin>(SkladisteZaTermine.getInstance().getByJmbg(p.Jmbg));

        }

        private void sat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}