using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;

namespace Servis
{
    public class LekarServis : KorisnikServis
    {
        public LekarServis()
        {
            skladisteZaLekara = SkladisteZaLekara.GetInstance();
        }

        public bool RegistrujLekara(Lekar lekar)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();

            Lekar lekar = new Lekar();

            foreach (Lekar lekar1 in lekari)
            {
                if (lekar1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    lekar = lekar1;
                    if (lekar1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return lekar;
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

        public List<Lekar> GetAll()
        {
            return skladisteZaLekara.GetAll();
        }

        public void Save(Lekar lekar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }

        public SkladisteZaLekara skladisteZaLekara;
    }
}