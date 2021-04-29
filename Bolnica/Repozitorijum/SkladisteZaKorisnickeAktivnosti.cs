/***********************************************************************
 * Module:  SkladisteZaKorisnickeAktivnosti.cs
 * Author:  PC
 * Purpose: Definition of the Class Repozitorijum.SkladisteZaKorisnickeAktivnosti
 ***********************************************************************/

using Model;
using System;
using System.Collections.Generic;

namespace Repozitorijum
{
   public class SkladisteZaKorisnickeAktivnosti
   {
      public List<KorisnickeAktivnostiNaAplikaciji> GetAll()
      {
         // TODO: implement
         return null;
      }
      
      public void Save(Model.KorisnickeAktivnostiNaAplikaciji korisnickeAktivnosti)
      {
         // TODO: implement
      }
      
      public void SaveAll(List<KorisnickeAktivnostiNaAplikaciji> korisnickeAktivnosti)
      {
         // TODO: implement
      }
      
      public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
      {
         // TODO: implement
         return null;
      }
   
      private String Lokacija;
   
   }
}