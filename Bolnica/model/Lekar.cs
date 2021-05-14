using System;

namespace Model
{
    public class Lekar : Radnik
    {
        public Specijalizacija Specijalizacija { get; set; }
        public System.Collections.ArrayList obavestenje;
        

        public System.Collections.ArrayList GetObavestenje()
        {
            if (obavestenje == null)
                obavestenje = new System.Collections.ArrayList();
            return obavestenje;
        }


        public void SetObavestenje(System.Collections.ArrayList newObavestenje)
        {
            RemoveAllObavestenje();
            foreach (Obavestenje oObavestenje in newObavestenje)
                AddObavestenje(oObavestenje);
        }


        public void AddObavestenje(Obavestenje newObavestenje)
        {
            if (newObavestenje == null)
                return;
            if (this.obavestenje == null)
                this.obavestenje = new System.Collections.ArrayList();
            if (!this.obavestenje.Contains(newObavestenje))
                this.obavestenje.Add(newObavestenje);
        }


        public void RemoveObavestenje(Obavestenje oldObavestenje)
        {
            if (oldObavestenje == null)
                return;
            if (this.obavestenje != null)
                if (this.obavestenje.Contains(oldObavestenje))
                    this.obavestenje.Remove(oldObavestenje);
        }


        public void RemoveAllObavestenje()
        {
            if (obavestenje != null)
                obavestenje.Clear();
        }

        public String FullName
        {
            get
            {
                return "dr " + this.Ime + " " + this.Prezime;
            }
            set
            {

            }
        }
        public String ImeiSpecijalizacija
        {
            get 
            {
                if (Specijalizacija == null)
                {
                    return this.FullName + "-  Lekar opšte medicine";
                }
                return this.FullName + "- " + this.Specijalizacija.VrstaSpecijalizacije;            
            }
            set
            {

            }
        }
        public int IdOrdinacija { get; set; }

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
            this.IdOrdinacija =1 ;


        }

        public String getFullName()
        {
            return this.Ime + " " + this.Prezime;
        }
        public String getImeiSpecijalizacija()
        {
            return this.FullName + " " + this.Specijalizacija;
        }
    }
}