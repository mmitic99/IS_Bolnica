using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class PraksaDTO
    {
        public String VrstaPrakse { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumKraja { get; set; }
        public String NazivUstanove { get; set; }
        public String BrojLjudi { get; set; }

        public PraksaDTO(string vrstaPrakse, DateTime datumPocetka, DateTime datumKraja, string nazivUstanove, string brojLjudi)
        {
            VrstaPrakse = vrstaPrakse;
            DatumPocetka = datumPocetka;
            DatumKraja = datumKraja;
            NazivUstanove = nazivUstanove;
            BrojLjudi = brojLjudi;
        }
    }
}
