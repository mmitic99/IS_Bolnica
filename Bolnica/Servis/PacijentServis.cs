using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;

namespace Servis
{
    public class PacijentServis : KorisnikServis
    {
        public PacijentServis()
        {
            skladistePacijenta = SkladistePacijenta.GetInstance();
        }

        public bool RegistrujPacijenta(Pacijent pacijent)
        {
            // TODO: implement
            return false;
        }

        public bool DodajAlergen(String alergen)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public bool DodajIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();

            Pacijent pacijent = new Pacijent();

            foreach (Pacijent pacijent1 in pacijenti)
            {
                if (pacijent1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    pacijent = pacijent1;
                    if (pacijent1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return pacijent;
                    }
                }
            }
            return null;
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public SkladistePacijenta skladistePacijenta;

    }
}