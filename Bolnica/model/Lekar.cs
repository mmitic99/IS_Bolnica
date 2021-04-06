using System;

namespace Model
{
   public class Lekar : Radnik
   {
      public Specijalizacija specijalizacija;
        public String FullName
        {
            get
            {
                return "dr "+this.Ime + " " + this.Prezime;
            }
            set
            {

            }
        }

        public Lekar()
        {

        }

        public Lekar(String im, String prezim, String jmbg)
        {
            this.Ime = im;
            this.Prezime = prezim;
            this.Jmbg = jmbg;
            this.Pol = Enum.Pol.Zenski;
            this.Adresa = "Jovana Vojvodica 23/A";
            this.DatumRodjenja = new DateTime(1967, 12, 25);
            this.Grad = new Grad();
            this.Grad.Naziv = "Beograd";
            this.Grad.PostanskiBroj = "2300";
            this.Grad.drzava.Naziv = "Srbija";
            this.Grad.drzava.Oznaka = "SRB";
            this.Korisnik = new Korisnik();
            this.Korisnik.KorisnickoIme = jmbg;
            this.Korisnik.Lozinka = im;



        }

        public String getFullName()
        {
            return this.Ime + " " + this.Prezime;
        }
    }
}