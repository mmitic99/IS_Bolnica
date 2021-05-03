using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class VerifikacijaLeka
    {
        public DateTime VremeSlanjaZahteva { get; set; }
        public String Naslov { get; set; }
        public String Sadrzaj { get; set; }
        public String JmbgLekara { get; set; }
        public String ImeLekara { get; set; }
        public String JmbgUpravnika { get; set; }
        public String ImeUpravnika { get; set; }
        public VerifikacijaLeka(DateTime vremeSlanja, String naslov, String sadrzaj, String jmbgLekara, String jmbgUpravnika)
        {
            VremeSlanjaZahteva = vremeSlanja;
            Naslov = naslov;
            Sadrzaj = sadrzaj;
            JmbgLekara = jmbgLekara;
            JmbgUpravnika = jmbgUpravnika;
        }
    }
}
