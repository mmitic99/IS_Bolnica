using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class PrebacivanjeOpremeInfoDTO
    {
        public int IndexIzKojeProstorije { get; set; }
        public int IndexUKojuProstoriju { get; set; }
        public String NazivOpreme { get; set; }
        public int KolicinaOpreme { get; set; }

        public PrebacivanjeOpremeInfoDTO(int indexIz, int indexU, String naziv, int kolicina)
        {
            IndexIzKojeProstorije = indexIz;
            IndexUKojuProstoriju = indexU;
            NazivOpreme = naziv;
            KolicinaOpreme = kolicina;
        }
    }
}
