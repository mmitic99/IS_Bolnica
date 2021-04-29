using System;
using System.Windows;

namespace Model
{
    public class Obavestenje
    {
        public DateTime VremeObavestenja { get; set; }
        public String Naslov { get; set; }
        public String Sadrzaj { get; set; }
        public String JmbgKorisnika { get; set; }
        public bool Podsetnik { get; set; }

        public Obavestenje()
        {
            Podsetnik = false;
        }

        public override bool Equals(object obj)
        {
            return obj is Obavestenje obavestenje &&
                   VremeObavestenja == obavestenje.VremeObavestenja &&
                   Naslov == obavestenje.Naslov &&
                   Sadrzaj == obavestenje.Sadrzaj &&
                   JmbgKorisnika == obavestenje.JmbgKorisnika &&
                   Podsetnik == obavestenje.Podsetnik;
        }
    }
}