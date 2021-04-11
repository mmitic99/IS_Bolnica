using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;

namespace Servis
{
    public class SekretarServis : KorisnikServis
    {
        public SekretarServis()
        {
            skladisteSekretara = SkladisteSekretara.GetInstance();
        }

        public bool RegistrujSekretara(Sekretar sekretar)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Sekretar> sekretari = skladisteSekretara.GetAll();

            Sekretar sekretar = new Sekretar();

            foreach (Sekretar sekretar1 in sekretari)
            {
                if (sekretar1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    sekretar = sekretar1;
                    if (sekretar1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return sekretar;
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

        public List<Sekretar> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Model.Sekretar sekretar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Sekretar> sekretari)
        {
            // TODO: implement
        }

        public SkladisteSekretara skladisteSekretara;

    }
}