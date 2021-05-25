using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class PretragaInfoDTO
    {
        public String NazivOpreme { get; set; }
        public String KolicinaOpreme { get; set; }
        public int IndexComboBox { get; set; }

        public PretragaInfoDTO(String naziv, String kolicina, int index)
        {
            NazivOpreme = naziv;
            KolicinaOpreme = kolicina;
            IndexComboBox = index;
        }
    }
}
