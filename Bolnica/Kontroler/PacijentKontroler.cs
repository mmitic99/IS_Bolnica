using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class PacijentKontroler : KorisnikKontroler
    {
        public PacijentKontroler()
        {
            pacijentServis = new PacijentServis();
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
            return pacijentServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Pacijent> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Pacijent pacijent)
        {
            // TODO: implement
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            // TODO: implement
        }

        public Servis.PacijentServis pacijentServis;

    }
}