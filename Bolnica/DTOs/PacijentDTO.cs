using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Enum;

namespace Bolnica.DTOs
{
    public class PacijentDTO
    {
        public string NazivGrada { get; set; }
        public KorisnikDTO Korisnik { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String Jmbg { get; set; }
        public String BrojTelefona { get; set; }
        public Pol Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public String Email { get; set; }
        public String Adresa { get; set; }
        public String BracnoStanje { get; set; }
        public String Zanimanje { get; set; }
        public ZdravstveniKartonDTO ZdravstveniKarton { get; set; }
        public bool Registrovan { get; set; }
    }
}
