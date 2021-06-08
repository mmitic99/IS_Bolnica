using Bolnica.Kontroler;
using Bolnica.model;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class PacijentTerminiViewModel : BindableBase
    {
        private TerminKontroler TerminKontroler;
        private IzvestajKontroler IzvestajKontroler;
        public MyICommand KreirajIzvestajCommand { get; set; }
        public ObservableCollection<Termin> ZakazaniTerminiPacijenta
        {
            get
            {
                return new ObservableCollection<Termin>((List<Termin>)TerminKontroler.GetByJmbgPacijentaVremenskiPeriod(pocetakIntervala, krajIntervala, JmbgPacijenta));
            }
            set
            {

            }
        }
        public DateTime pocetakIntervala { get; set; }
        public DateTime krajIntervala { get; set; }
        public String JmbgPacijenta;

        public PacijentTerminiViewModel(Pacijent pacijent)
        {
            this.TerminKontroler = new TerminKontroler();
            this.IzvestajKontroler = new IzvestajKontroler();
            this.JmbgPacijenta = pacijent.Jmbg;
            this.KreirajIzvestajCommand = new MyICommand(KreirajIzvestaj);
            this.pocetakIntervala = DateTime.Today;
            this.krajIntervala = DateTime.Today.AddMonths(1).AddDays(1).AddSeconds(-1);
            this.ZakazaniTerminiPacijenta = new ObservableCollection<Termin>((List<Termin>)TerminKontroler.GetByJmbgPacijentaVremenskiPeriod(pocetakIntervala, krajIntervala, JmbgPacijenta));
        }

        public void KreirajIzvestaj()
        {
            IzvestajKontroler.KreirajIzvestajOPregledimaIOperacijama(pocetakIntervala, krajIntervala, JmbgPacijenta);
        }

        public void daLiJeIspravanKrajnjiDatum()
        {
            if(pocetakIntervala > krajIntervala)
            {
                krajIntervala = pocetakIntervala.Date.AddDays(1).AddSeconds(-1);
            }
            AzurirajTabelu();
        }

        public void DaLiJeIspravanPocetniDatum()
        {
            krajIntervala = krajIntervala.AddDays(1).AddSeconds(-1);
            if(pocetakIntervala>krajIntervala)
            {
                pocetakIntervala = krajIntervala.Date;
            }
            AzurirajTabelu();
        }

        public void AzurirajTabelu()
        {
            ZakazaniTerminiPacijenta = new ObservableCollection<Termin>((List<Termin>)TerminKontroler.GetByJmbgPacijentaVremenskiPeriod(pocetakIntervala, krajIntervala, JmbgPacijenta));
        }
    }
}
