using Model;
using System;

namespace Kontroler
{
   public class PacijentKontroler : KorisnikKontroler
   {
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

        public Servis.PacijentServis pacijentServis;
   
   }
}