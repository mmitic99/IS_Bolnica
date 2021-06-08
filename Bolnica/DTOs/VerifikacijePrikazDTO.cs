using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class VerifikacijePrikazDTO
    {
        public DateTime VremeSlanjaZahteva { get; set; }
        public String Naslov { get; set; }
        public string ImeLekara { get; set; }
        public String Napomena { get; set; }
        public String ID { get; set; }
        public String Sadrzaj { get; set; }

        public VerifikacijePrikazDTO(DateTime vremeSlanjaZahteva, string naslov, string imeLekara, string napomena, string id, string sadrzaj)
        {
            VremeSlanjaZahteva = vremeSlanjaZahteva;
            Naslov = naslov;
            ImeLekara = imeLekara;
            Napomena = napomena;
            ID = id;
            Sadrzaj = sadrzaj;
        }

        public VerifikacijePrikazDTO()
        {
        }
    }
}
