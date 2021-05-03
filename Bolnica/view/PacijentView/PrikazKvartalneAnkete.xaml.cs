using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.viewActions;
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

namespace Bolnica.view.PacijentView
{
    /// <summary>
    /// Interaction logic for PrikazKvartalneAnkete.xaml
    /// </summary>
    public partial class PrikazKvartalneAnkete : UserControl
    {
        public PrikazKvartalneAnkete()
        {
            InitializeComponent();
            Nazad.Command = MainViewModel.getInstance().PrikazObavestenjaCommand;
        }

        private void SacuvajAnketu_Click(object sender, RoutedEventArgs e)
        {
            KvartalnaAnketaDTO kvartalnaAnketa = new KvartalnaAnketaDTO()
            {
                JmbgKorisnika = PacijentMainWindow.getInstance().pacijent,
                KomentarKorisnika = komentar.Text,
                StrucnostMedicinskogOsobolja = vratiOcenu(strucnostOsoblja),
                LjubaznostMedicinskogOsobolja = vratiOcenu(ljubaznostMedicinskog),
                LjubaznostNemedicnskogOsoblja = vratiOcenu(ljubaznostNemedicinskog),
                RezultatiTestovaDostupniURazumnoVreme = vratiOcenu(rezultatiAnaliza),
                JednostavnostZakazivanjaTerminaPrekoTelefona = vratiOcenu(zakazivanjeTelefon),
                JednostavnostZakazivanjaTerminaPrekoAplikacije = vratiOcenu(zakazivanjeAplikacija),
                InformacijeOOdlozenomTerminu = vratiOcenu(obavestenjaOtkazivanje),
                DostupnostLekaraUTokuRadnihSatiLekara = vratiOcenu(dostupnostRadno),
                DostupnostLekaraKadaJeBolnicaZatvorena = vratiOcenu(dostupnostNeradno),
                DostupnostTerminaURazumnomRoku = vratiOcenu(dostupnostTermina),
                IzgledNaseBolnice = vratiOcenu(izgledBolnice),
                OpremljenostBolnice = vratiOcenu(opremljenostBolnice),
                CelokupniUtisak = vratiOcenu(opstiUtisak),
                datumAnkete = MainViewModel.getInstance().PrikazKvartalneAnketeVM.anketa.datum
                
            };

            if (AnketeKontroler.GetInstance().SacuvajKvartalnuAnketu(kvartalnaAnketa))
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PrikazObavestenjaVM;
            }
            else
            {
                var s = new Upozorenje("Morate uneti sve ocene!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
               
        }

        private object vratiOcenu(StackPanel strucnostOsoblja)
        {
            foreach(RadioButton RB in strucnostOsoblja.Children)
            {
                if((bool)RB.IsChecked)
                {
                    return RB.Content;
                }
            }
            return "-1";
            
        }
    }
}
