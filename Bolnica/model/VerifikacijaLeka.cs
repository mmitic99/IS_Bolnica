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
        public String JmbgPrimaoca { get; set; }
        public String JmbgPosiljaoca { get; set; }
        public String Napomena { get; set; }
        public String IdVerifikacijeLeka { get; set; }
        public VerifikacijaLeka(DateTime vremeSlanja, String naslov, String sadrzaj, String jmbgPosiljaoca, String jmbgPrimaoca, String napomena)
        {
            IdVerifikacijeLeka = vremeSlanja.ToString() + jmbgPosiljaoca + jmbgPrimaoca;
            VremeSlanjaZahteva = vremeSlanja;
            Naslov = naslov;
            Sadrzaj = sadrzaj;
            JmbgPrimaoca = jmbgPrimaoca;
            JmbgPosiljaoca = jmbgPosiljaoca;
            Napomena = napomena;
        }
    }
}
