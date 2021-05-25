using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class ProstorijaInfoDTO
    {
        public PretragaInfoDTO pretragaInfo { get; set; }
        public List<StacionarnaOprema> OpremaProstorije { get; set; } 
        public Prostorija Prostorija { get; set; }

        public ProstorijaInfoDTO(PretragaInfoDTO pretragaInfo, List<StacionarnaOprema> opremaProstorije, Prostorija prostorija)
        {
            this.pretragaInfo = pretragaInfo;
            OpremaProstorije = opremaProstorije;
            Prostorija = prostorija;
        }

        public ProstorijaInfoDTO()
        {
        }
    }
}
