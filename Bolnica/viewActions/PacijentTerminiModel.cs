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

        public List<Termin> ZakazaniTerminiPacijenta
        {
            get
            {
                return (List<Termin>)TerminKontroler.GetByJmbgPacijenta(JmbgPacijenta);
            }
            set
            {

            }
        }
        public String JmbgPacijenta;

        public PacijentTerminiViewModel(Pacijent pacijent)
        {
            this.TerminKontroler = new TerminKontroler();
            this.IzvestajKontroler = new IzvestajKontroler();
            this.JmbgPacijenta = pacijent.Jmbg;
            this.KreirajIzvestajCommand = new MyICommand(KreirajIzvestaj);
        }

        public void KreirajIzvestaj()
        {
            IzvestajKontroler.KreirajIzvestajOPregledimaIOperacijama(JmbgPacijenta);
        }
           
    }
}
