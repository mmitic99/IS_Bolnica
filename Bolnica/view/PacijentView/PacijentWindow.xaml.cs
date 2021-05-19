using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using Model.Enum;
using System.Collections.ObjectModel;
using Bolnica.model;
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

            instance = this;

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

            List<Termin> termins = SkladisteZaTermine.getInstance().getByJmbg(pacijent.Jmbg);
            prikazTermina.ItemsSource = termins;
            this.JmbgPacijenta = pacijent.Jmbg;
            DataContext = pacijent;
            InitializeComponent();



        }




        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           // var s = new ZakazivanjeTermina(this.JmbgPacijenta);
           // s.Show();
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
               /* var s = new IzmenaTermina(((Termin)prikazTermina.SelectedItem).IDTermina);
                s.Show();*/
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }
    }
}
