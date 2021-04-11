using Model;
using Servis;
using System;

namespace Kontroler
{
    public class UpravnikKontroler : KorisnikKontroler
    {
        public UpravnikKontroler()
        {
            upravnikServis = new UpravnikServis();
        }

        public bool RegistrujUpravnika(Upravnik upravnik)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return upravnikServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public Servis.UpravnikServis upravnikServis;

    }
}