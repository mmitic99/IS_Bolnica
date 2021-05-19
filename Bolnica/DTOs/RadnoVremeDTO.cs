using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model.Enum;

namespace Bolnica.DTOs
{
    public class RadnoVremeDTO
    {
        public string IdRadnogVremena { get; set; }
        public string JmbgLekara { get; set; }
        public DateTime DatumIVremePocetka { get; set; }
        public DateTime DatumIVremeZavrsetka { get; set; }
        public StatusRadnogVremena StatusRadnogVremena { get; set; }
    }
}
