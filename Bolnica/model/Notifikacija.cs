using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class Notifikacija
    {
        public DateTime VremeObavestenja { get; set; }
        public String Naslov { get; set; }
        public String Sadrzaj { get; set; }
        public String JmbgKorisnika { get; set; }
        public bool Vidjeno { get; set; }

    }
}
