using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class IzvestajKontroler
    {
        private IzvestajServis IzvestajServis;

        public IzvestajKontroler()
        {
            this.IzvestajServis = new IzvestajServis();
        }
        public void KreirajIzvestajOPregledimaIOperacijama(DateTime pocetakIntervala, DateTime krajIntervala, string jmbgPacijenta)
        {
            IzvestajServis.KreirajIzvestajOPregledimaIOperacijama(pocetakIntervala, krajIntervala, jmbgPacijenta);
        }
    }
}
