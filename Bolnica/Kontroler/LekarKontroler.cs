using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class LekarKontroler : KorisnikKontroler
    {
        public LekarKontroler()
        {
            lekarServis = new LekarServis();
        }

        public bool RegistrujLekara(Lekar lekar)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return lekarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Lekar> GetAll()
        {
            return lekarServis.GetAll();
        }

        public void Save(Model.Lekar lekar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }

        public Servis.LekarServis lekarServis;

    }
}