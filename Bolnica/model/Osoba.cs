using Model.Enum;
using System;

namespace Model
{
    public class Osoba
    {
        public Grad Grad { get; set; }
        public Korisnik Korisnik { get; set; }

        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String Jmbg { get; set; }
        public String BrojTelefona { get; set; }
        public Pol Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public String Email { get; set; }
        public String Adresa { get; set; }

    }
}