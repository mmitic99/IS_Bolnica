using Model;
using Repozitorijum;
using System;

namespace Servis
{
   public class LekarServis : KorisnikServis
   {
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

        public SkladisteZaLekara skladisteZaLekara;
   
   }
}