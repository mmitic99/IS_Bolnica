using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class ObavestenjeDTO
    {
        public DateTime VremeObavestenja { get; set; }
        public String Naslov { get; set; }
        public String Sadrzaj { get; set; }
        public String JmbgKorisnika { get; set; }
        public bool Podsetnik { get; set; }
        public DateTime kvartalnaAnketa { get; set; }
        public PrikacenaAnketaPoslePregledaDTO anketaOLekaru { get; set; }
        public bool Vidjeno { get; set; }
    }
}
