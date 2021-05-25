using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class PotrosnaOpremaDTO
    {
        public String TipOpreme { get; set; }
        public int KolicinaOpreme { get; set; }

        public PotrosnaOpremaDTO() { }

        public PotrosnaOpremaDTO(String tip, int kolicina)
        {
            TipOpreme = tip;
            KolicinaOpreme = kolicina;
        }

    }
}
