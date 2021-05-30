using System;

namespace Kontroler
{
   public interface KorisnikKontroler
   {
      Object PrijavljivanjeKorisnika(String korisnickoIme, String lozinka);
      bool IzmenaLozinke(string jmbg, string staraLozinka, string novaLozinka);
      bool IzmenaKorisnickogImena(String staroKorisnickoIme, String novoKorisnickoIme);
   }
}