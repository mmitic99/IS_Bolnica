using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class IzmenaOpremeInfoDTO
    {
        public int IndexProstorije { get; set; }
        public int IndexOpreme { get; set; }
        public int KolicinaOpreme { get; set; }

        public IzmenaOpremeInfoDTO(int indexP, int indexO, int kolicina)
        {
            IndexProstorije = indexP;
            IndexOpreme = indexO;
            KolicinaOpreme = kolicina;
        }
    }
}
