using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class StacionarnaOpremaDTO
    {
        public String TipStacionarneOpreme { get; set; }
        public int Kolicina { get; set; }

        public StacionarnaOpremaDTO() { }

        public StacionarnaOpremaDTO(String tip, int kolicina)
        {
            TipStacionarneOpreme = tip;
            Kolicina = kolicina;
        }
    }
}
