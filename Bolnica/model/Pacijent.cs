using System;

namespace Model
{
   public class Pacijent : Osoba
   {
        public bool Registrovan { get; set; }
        public String FullName
        {
            get
            {
                return this.Ime + " " + this.Prezime;
            }
            set
            {

            }
        }

        public Pacijent()
        {

        }

        public Pacijent(String jmbg)
        {
            this.Jmbg = jmbg;
            this.Ime = "Jovana";
            this.Prezime = "Jovanovic";
            this.Pol = Enum.Pol.Zenski;
            this.Registrovan = true;
            this.Adresa = "Jovana Mikica 23/A";
            this.DatumRodjenja = new DateTime(1999, 12, 3);
            Grad = new Grad();
            this.Grad.Naziv = "Beograd";
            this.Grad.PostanskiBroj = "2300";
            this.Grad.drzava.Naziv = "Srbija";
            this.Grad.drzava.Oznaka = "SRB";
        }

    }
}