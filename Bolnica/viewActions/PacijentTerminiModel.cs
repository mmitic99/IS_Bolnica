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
    public class PacijentTerminiViewModel
    {
        private TerminKontroler TerminKontroler;
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
            this.JmbgPacijenta = pacijent.Jmbg;
        }
    }
}
