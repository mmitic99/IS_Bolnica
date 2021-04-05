using System;

namespace Servis
{
   public interface KorisnikServis
   {
      Object PrijavljivanjeKorisnika(String korisnickoIme, String lozinka);
      bool IzmenaLozinke(String staraLozinka, String novaLozinka);
      bool IzmenaKorisnickogImena(String staroKorisnickoIme, String novoKorisnickoIme);
   }
}