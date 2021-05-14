using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model.Enum;

namespace Bolnica.model
{
    public class RadnoVreme
    {
        public string IdRadnogVremena { get; set; }
        public string JmbgLekara { get; set; }
        public DateTime DatumIVremePocetka { get; set; }
        public DateTime DatumIVremeZavrsetka { get; set; }
        public StatusRadnogVremena StatusRadnogVremena { get; set; }

    public void GenerisiIdRadnogVremena()
        {
            IdRadnogVremena = StatusRadnogVremena + JmbgLekara + DatumIVremePocetka;
        }
    }
}
