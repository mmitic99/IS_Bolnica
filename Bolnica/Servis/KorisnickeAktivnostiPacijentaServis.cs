/***********************************************************************
 * Module:  KorisnickeAktivnostiPacijentaServis.cs
 * Author:  PC
 * Purpose: Definition of the Class Servis.KorisnickeAktivnostiPacijentaServis
 ***********************************************************************/

using System;
using Model.Enum;

namespace Servis
{
   public class KorisnickeAktivnostiPacijentaServis
   {
      public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
      {
         // TODO: implement
         return null;
      }
      
      public Model.Enum.VrstaKorisnikaAplikacije GetRangKorisnika(String jmbgKorisnika)
      {
         // TODO: implement
         return VrstaKorisnikaAplikacije.Normalan;
      }
      
      public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
      {
         // TODO: implement
         return 0;
      }
      
      public int DobaviBrojOtkazivanjaUProteklihMesecDana(String jmbgKorisnika)
      {
         // TODO: implement
         return 0;
      }
      
      public void NapraviNoveKorisnickeAktivnosti(String jmbgKorisnika)
      {
         // TODO: implement
      }
      
      public Boolean DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
      {
         // TODO: implement
         return true;
      }
      
      public void DodajZakazivanje(String jmbgPacijenta)
      {
         // TODO: implement
      }
      
      public void DodajOdlaganje(String jmbgPacijenta)
      {
         // TODO: implement
      }
      
      public bool IzmenaKorisnickeAktivnosti(Model.KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost)
      {
         // TODO: implement
         return false;
      }
      
      public void AzurirajRang(Model.KorisnickaAktivnost korisnickaAktivnost)
      {
         // TODO: implement
      }
      
      public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
      {
         // TODO: implement
         return true;
      }
   
      public Repozitorijum.SkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;
   
   }
}