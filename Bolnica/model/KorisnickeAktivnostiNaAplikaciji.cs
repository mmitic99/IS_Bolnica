/***********************************************************************
 * Module:  KorisnickeAktivnostiNaAplikaciji.cs
 * Author:  PC
 * Purpose: Definition of the Class Model.KorisnickeAktivnostiNaAplikaciji
 ***********************************************************************/

using System;
using Model.Enum;

namespace Model
{
   public class KorisnickeAktivnostiNaAplikaciji
   {
      public bool BlokirajKorisnika()
      {
         // TODO: implement
         return false;
      }
      
      public bool OdblokirajKorisnika()
      {
         // TODO: implement
         return false;
      }
      
      public bool OznaciDaJeZakazaoPrevisePregledaUnapred()
      {
         // TODO: implement
         return false;
      }
   
      public System.Collections.ArrayList korisnickaAktivnost;
      
      /// <pdGenerated>default getter</pdGenerated>
      public System.Collections.ArrayList GetKorisnickaAktivnost()
      {
         if (korisnickaAktivnost == null)
            korisnickaAktivnost = new System.Collections.ArrayList();
         return korisnickaAktivnost;
      }
      
      /// <pdGenerated>default setter</pdGenerated>
      public void SetKorisnickaAktivnost(System.Collections.ArrayList newKorisnickaAktivnost)
      {
         RemoveAllKorisnickaAktivnost();
         foreach (KorisnickaAktivnost oKorisnickaAktivnost in newKorisnickaAktivnost)
            AddKorisnickaAktivnost(oKorisnickaAktivnost);
      }
      
      /// <pdGenerated>default Add</pdGenerated>
      public void AddKorisnickaAktivnost(KorisnickaAktivnost newKorisnickaAktivnost)
      {
         if (newKorisnickaAktivnost == null)
            return;
         if (this.korisnickaAktivnost == null)
            this.korisnickaAktivnost = new System.Collections.ArrayList();
         if (!this.korisnickaAktivnost.Contains(newKorisnickaAktivnost))
            this.korisnickaAktivnost.Add(newKorisnickaAktivnost);
      }
      
      /// <pdGenerated>default Remove</pdGenerated>
      public void RemoveKorisnickaAktivnost(KorisnickaAktivnost oldKorisnickaAktivnost)
      {
         if (oldKorisnickaAktivnost == null)
            return;
         if (this.korisnickaAktivnost != null)
            if (this.korisnickaAktivnost.Contains(oldKorisnickaAktivnost))
               this.korisnickaAktivnost.Remove(oldKorisnickaAktivnost);
      }
      
      /// <pdGenerated>default removeAll</pdGenerated>
      public void RemoveAllKorisnickaAktivnost()
      {
         if (korisnickaAktivnost != null)
            korisnickaAktivnost.Clear();
      }
   
      private String JmbgKorisnika;
        private VrstaKorisnikaAplikacije TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Normalan;
   
   }
}