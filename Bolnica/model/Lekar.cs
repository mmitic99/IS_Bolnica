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
            this.grad = new Grad();
            this.grad.Naziv = "Beograd";
            this.grad.PostanskiBroj = "2300";
            this.grad.drzava.Naziv = "Srbija";
            this.grad.drzava.Oznaka = "SRB";



        }

        public String getFullName()
        {
            return this.Ime + " " + this.Prezime;
        }
    }
}