using Kontroler;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class RenoviranjeDTO
    {
        public int IdProstorije { get; set; }
        public String BrojProstorije { get; set; }
        public VrstaProstorije VrstaProstorije { get; set; }
        public int Sprat { get; set; }
        public DateTime DatumPocetkaRenoviranja { get; set; }
        public DateTime DatumZavrsetkaRenoviranja { get; set; }

        public RenoviranjeDTO() { }

        public RenoviranjeDTO(String Broj, DateTime DatumPocetka, DateTime DatumKraja)
        {
            BrojProstorije = Broj;
            IdProstorije = ProstorijeKontroler.GetInstance().GetIdProstorijeByBrojProstorije(Broj);
            VrstaProstorije = ProstorijeKontroler.GetInstance().GetVrstaProstorijeByBrojProstorije(Broj);
            Sprat = ProstorijeKontroler.GetInstance().GetSpratProstorijeByBrojProstorije(Broj);
            DatumPocetkaRenoviranja = DatumPocetka;
            DatumZavrsetkaRenoviranja = DatumKraja;
        }

    }
}
