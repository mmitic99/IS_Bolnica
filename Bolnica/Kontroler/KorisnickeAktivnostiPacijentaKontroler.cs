
using Servis;
using System;

namespace Kontroler
{
   public class KorisnickeAktivnostiPacijentaKontroler
   {
        private KorisnickeAktivnostiPacijentaServis KorisnickeAktivnostiPacijentaServis;
        private static KorisnickeAktivnostiPacijentaKontroler instance = null;

        public static KorisnickeAktivnostiPacijentaKontroler GetInstance()
        {
            if (instance == null)
            {
                return new KorisnickeAktivnostiPacijentaKontroler();
            }
            else
                return instance;
        }

        public KorisnickeAktivnostiPacijentaKontroler()
        {
            instance = this;
            this.KorisnickeAktivnostiPacijentaServis = new KorisnickeAktivnostiPacijentaServis();
        }

      
      public int DobaviBrojOtkazivanjaUProteklihMesecDana(String jmbgKorisnika)
      {
         return KorisnickeAktivnostiPacijentaServis.DobaviBrojOtkazivanjaUProteklihMesecDana(jmbgKorisnika);
      }
      
      public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
      {
         return KorisnickeAktivnostiPacijentaServis.DobaviBrojZakazanihPregledaUBuducnosti(jmbgKorisnkika);
      }
      
      public bool DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
      {
         return KorisnickeAktivnostiPacijentaServis.DaLiJeMoguceZakazatiNoviTermin(jmbgKorisnika);
      }

        internal void OdblokirajKorisnike()
        {
            KorisnickeAktivnostiPacijentaServis.OdblokirajKorisnika();
        }

        public void DodajZakazivanje(string jmbg)
      {
            KorisnickeAktivnostiPacijentaServis.DodajZakazivanje(jmbg);
      }
      
      public void DodajOdlaganje(string jmbgPacijenta)
      {
            KorisnickeAktivnostiPacijentaServis.DodajOdlaganje(jmbgPacijenta);
      }
      
      public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
      {
         return KorisnickeAktivnostiPacijentaServis.DaLiJeMoguceOdlozitiZakazaniTermin(jmbgPacijenta);
      }

        internal string DobaviPorukuZabrane(string jmbgPacijenta)
        {
            return KorisnickeAktivnostiPacijentaServis.DobaviPorukuZabrane(jmbgPacijenta);
        }
    }
}