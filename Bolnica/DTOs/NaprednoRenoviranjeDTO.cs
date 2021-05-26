using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class NaprednoRenoviranjeDTO
    {
        public int IdGlavneProstorije { get; set; }
        public int IdProstorije1 { get; set; }
        public int IdProstorije2 { get; set; }
        public String BrojGlavneProstorije { get; set; }
        public String BrojProstorije1 { get; set; }
        public String BrojProstorije2 { get; set; }
        public DateTime DatumPocetkaRenoviranja { get; set; }
        public DateTime DatumZavrsetkaRenoviranja { get; set; }
        public bool Spajanje { get; set; }
        public bool Podela { get; set; }

        public NaprednoRenoviranjeDTO() { }

        public NaprednoRenoviranjeDTO(int idGlavneProstorije, int idProstorije1, int idProstorije2,
                                   string brojGlavneProstorije, string brojProstorije1, string brojProstorije2,
                                   DateTime datumPocetkaRenoviranja, DateTime datumZavrsetkaRenoviranja, bool spajanje, bool podela)
        {
           // IdGlavneProstorije = idGlavneProstorije;
            IdProstorije1 = idProstorije1;
            IdProstorije2 = idProstorije2;
            BrojGlavneProstorije = brojGlavneProstorije;
            BrojProstorije1 = brojProstorije1;
            BrojProstorije2 = brojProstorije2;
            DatumPocetkaRenoviranja = datumPocetkaRenoviranja;
            DatumZavrsetkaRenoviranja = datumZavrsetkaRenoviranja;
            Spajanje = spajanje;
            Podela = podela;
        }
    }
}
