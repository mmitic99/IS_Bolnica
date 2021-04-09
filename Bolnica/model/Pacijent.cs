using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
    public class Pacijent : Osoba
    {
        public ZdravstveniKarton zdravstveniKarton;
        public List<Obavestenje> obavestenje;


        public List<Obavestenje> GetObavestenje()
        {
            if (obavestenje == null)
                obavestenje = new List<Obavestenje>();
            return obavestenje;
        }


        public void SetObavestenje(List<Obavestenje> newObavestenje)
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
                this.obavestenje = new List<Obavestenje>();
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