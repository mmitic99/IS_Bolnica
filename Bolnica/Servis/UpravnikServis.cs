using Model;
using Repozitorijum;
using System;

namespace Servis
{
   public class UpravnikServis : KorisnikServis
   {
      public bool RegistrujUpravnika(Upravnik upravnik)
      {
         // TODO: implement
         return false;
      }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public SkladisteUpravnik skladisteUpravnik;
   
   }
}