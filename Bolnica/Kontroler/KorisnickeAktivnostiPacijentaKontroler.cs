/***********************************************************************
 * Module:  KorisnickeAktivnostiPacijentaKontroler.cs
 * Author:  PC
 * Purpose: Definition of the Class Kontroler.KorisnickeAktivnostiPacijentaKontroler
 ***********************************************************************/

using Servis;
using System;

namespace Kontroler
{
   public class KorisnickeAktivnostiPacijentaKontroler
   {
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
        }

      public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
      {
         // TODO: implement
         return null;
      }
      
      public Model.Enum.VrstaKorisnikaAplikacije GetRangKorisnika(String jmbgKorisnika)
      {
         // TODO: implement
         return Model.Enum.VrstaKorisnikaAplikacije.Normalan;
      }
      
      public int DobaviBrojOtkazivanjaUProteklihMesecDana(String jmbgKorisnika)
      {
         // TODO: implement
         return KorisnickeAktivnostiPacijentaServis.GetInstance().DobaviBrojOtkazivanjaUProteklihMesecDana(jmbgKorisnika);
      }
      
      public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
      {
         // TODO: implement
         return KorisnickeAktivnostiPacijentaServis.GetInstance().DobaviBrojZakazanihPregledaUBuducnosti(jmbgKorisnkika);
      }
      
      public bool DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
      {
         // TODO: implement
         return KorisnickeAktivnostiPacijentaServis.GetInstance().DaLiJeMoguceZakazatiNoviTermin(jmbgKorisnika);
      }
      
      public void DodajZakazivanje(string jmbg)
      {
            KorisnickeAktivnostiPacijentaServis.GetInstance().DodajZakazivanje(jmbg);
      }
      
      public void DodajOdlaganje(string jmbgPacijenta)
      {
            KorisnickeAktivnostiPacijentaServis.GetInstance().DodajOdlaganje(jmbgPacijenta);
      }
      
      public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
      {
         // TODO: implement
         return KorisnickeAktivnostiPacijentaServis.GetInstance().DaLiJeMoguceOdlozitiZakazaniTermin(jmbgPacijenta);
      }

        internal string DobaviPorukuZabrane(string jmbgPacijenta)
        {
            return KorisnickeAktivnostiPacijentaServis.GetInstance().DobaviPorukuZabrane(jmbgPacijenta);
        }

        /// <pdGenerated>default getter</pdGenerated>

    }
}