/***********************************************************************
 * Module:  KorisnickeAktivnostiPacijentaKontroler.cs
 * Author:  PC
 * Purpose: Definition of the Class Kontroler.KorisnickeAktivnostiPacijentaKontroler
 ***********************************************************************/

using System;

namespace Kontroler
{
   public class KorisnickeAktivnostiPacijentaKontroler
   {
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
         return 0;
      }
      
      public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
      {
         // TODO: implement
         return 0;
      }
      
      public bool DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
      {
         // TODO: implement
         return true;
      }
      
      public void DodajZakazivanje()
      {
         // TODO: implement
      }
      
      public void DodajOdlaganje()
      {
         // TODO: implement
      }
      
      public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
      {
         // TODO: implement
         return true;
      }
   
      public System.Collections.ArrayList korisnickeAktivnostiPacijentaServis;
      
      /// <pdGenerated>default getter</pdGenerated>
      public System.Collections.ArrayList GetKorisnickeAktivnostiPacijentaServis()
      {
         if (korisnickeAktivnostiPacijentaServis == null)
            korisnickeAktivnostiPacijentaServis = new System.Collections.ArrayList();
         return korisnickeAktivnostiPacijentaServis;
      }
      
      /// <pdGenerated>default setter</pdGenerated>
      public void SetKorisnickeAktivnostiPacijentaServis(System.Collections.ArrayList newKorisnickeAktivnostiPacijentaServis)
      {
         RemoveAllKorisnickeAktivnostiPacijentaServis();
         foreach (Servis.KorisnickeAktivnostiPacijentaServis oKorisnickeAktivnostiPacijentaServis in newKorisnickeAktivnostiPacijentaServis)
            AddKorisnickeAktivnostiPacijentaServis(oKorisnickeAktivnostiPacijentaServis);
      }
      
      /// <pdGenerated>default Add</pdGenerated>
      public void AddKorisnickeAktivnostiPacijentaServis(Servis.KorisnickeAktivnostiPacijentaServis newKorisnickeAktivnostiPacijentaServis)
      {
         if (newKorisnickeAktivnostiPacijentaServis == null)
            return;
         if (this.korisnickeAktivnostiPacijentaServis == null)
            this.korisnickeAktivnostiPacijentaServis = new System.Collections.ArrayList();
         if (!this.korisnickeAktivnostiPacijentaServis.Contains(newKorisnickeAktivnostiPacijentaServis))
            this.korisnickeAktivnostiPacijentaServis.Add(newKorisnickeAktivnostiPacijentaServis);
      }
      
      /// <pdGenerated>default Remove</pdGenerated>
      public void RemoveKorisnickeAktivnostiPacijentaServis(Servis.KorisnickeAktivnostiPacijentaServis oldKorisnickeAktivnostiPacijentaServis)
      {
         if (oldKorisnickeAktivnostiPacijentaServis == null)
            return;
         if (this.korisnickeAktivnostiPacijentaServis != null)
            if (this.korisnickeAktivnostiPacijentaServis.Contains(oldKorisnickeAktivnostiPacijentaServis))
               this.korisnickeAktivnostiPacijentaServis.Remove(oldKorisnickeAktivnostiPacijentaServis);
      }
      
      /// <pdGenerated>default removeAll</pdGenerated>
      public void RemoveAllKorisnickeAktivnostiPacijentaServis()
      {
         if (korisnickeAktivnostiPacijentaServis != null)
            korisnickeAktivnostiPacijentaServis.Clear();
      }
   
   }
}