using Model;
using System;

namespace Kontroler
{
   public class LekarKontroler : KorisnikKontroler
   {
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

        public Servis.LekarServis lekarServis;
   
   }
}