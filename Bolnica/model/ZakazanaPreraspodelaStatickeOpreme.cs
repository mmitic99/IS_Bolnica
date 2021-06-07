using Model;
using Repozitorijum;
using Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class ZakazanaPreraspodelaStatickeOpreme
    {
        public DateTime DatumIVremePreraspodele { get; set; }

        public String NazivOpreme { get; set; }

        public int KolicinaOpreme { get; set; }

        public String BrojProstorijeIzKojeSePrenosiOprema { get; set; }

        public String BrojProstorijeUKojuSePrenosiOprema { get; set; }

        public int IdProstorijeIzKojeSePrenosiOprema { get; set; }

        public int IdProstorijeUKojUSePrenosiOprema { get; set; }

        public double TrajanjePreraspodele { get; set; }

        public ZakazanaPreraspodelaStatickeOpreme() {}

        public ZakazanaPreraspodelaStatickeOpreme(String brojSobeIz, String brojSobeU, DateTime datumIVreme, double trajanje, String nazivOpreme, int kolicina)
        {
            BrojProstorijeIzKojeSePrenosiOprema = brojSobeIz;
            BrojProstorijeUKojuSePrenosiOprema = brojSobeU;
            DatumIVremePreraspodele = datumIVreme;
            TrajanjePreraspodele = trajanje;
            NazivOpreme = nazivOpreme;
            KolicinaOpreme = kolicina;
            IdProstorijeIzKojeSePrenosiOprema = ProstorijeServis.GetInstance().GetIdProstorijeByBrojProstorije(brojSobeIz);
            IdProstorijeUKojUSePrenosiOprema = ProstorijeServis.GetInstance().GetIdProstorijeByBrojProstorije(brojSobeU);
        }
    }
}
