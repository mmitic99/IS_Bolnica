using Model;
using Servis;
using System;

namespace Kontroler
{
    public class SekretarKontroler : KorisnikKontroler
    {
        public SekretarKontroler()
        {
            sekretarServis = new SekretarServis();
        }

        public bool RegistrujSekretara(Sekretar sekretar)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return sekretarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public Servis.SekretarServis sekretarServis;

    }
}