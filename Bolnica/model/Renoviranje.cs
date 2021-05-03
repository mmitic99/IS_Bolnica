using Kontroler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Bolnica.model
{
    public class Renoviranje
    {
        public int IdProstorije { get; set; }
        public String BrojProstorije {get; set;}
        public VrstaProstorije VrstaProstorije { get; set; }
        public int Sprat { get; set; }
        public DateTime DatumPocetkaRenoviranja { get; set; }
        public DateTime DatumZavrsetkaRenoviranja { get; set; }

        public Renoviranje() { }

        public Renoviranje(String Broj, DateTime DatumPocetka, DateTime DatumKraja)
        {
            IdProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(Broj);
            BrojProstorije = Broj;
            VrstaProstorije = ProstorijeKontroler.GetInstance().GetVrstaProstorijeByBrojProstorije(Broj);
            Sprat = ProstorijeKontroler.GetInstance().GetSpratProstorijeByBrojProstorije(Broj);
            DatumPocetkaRenoviranja = DatumPocetka;
            DatumZavrsetkaRenoviranja = DatumKraja;
        }

    }
}
