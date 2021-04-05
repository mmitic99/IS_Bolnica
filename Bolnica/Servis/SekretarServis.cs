using Model;
using Repozitorijum;
using System;

namespace Servis
{
   public class SekretarServis : KorisnikServis
   {
      public bool RegistrujSekretara(Sekretar sekretar)
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

        public SkladisteSekretara skladisteSekretara;
   
   }
}