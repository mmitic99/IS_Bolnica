using System;

namespace Model
{
   public class InventarZaProstorije
   {
      public void IzmeniKolicinuLeka(int idLeka, double kolicina)
      {
         // TODO: implement
      }
      
      public void IzmeniKolicinuPotrosneOpreme(String tipOpreme, int kolicina)
      {
         // TODO: implement
      }
      
      public void IzmeniKolicinuStacionarneOpreme(String tipOpreme, int kolicina)
      {
         // TODO: implement
      }
   
      public System.Collections.ArrayList lek;
      
      
      public System.Collections.ArrayList GetLek()
      {
         if (lek == null)
            lek = new System.Collections.ArrayList();
         return lek;
      }
      
      
      public void SetLek(System.Collections.ArrayList newLek)
      {
         RemoveAllLek();
         foreach (Lek oLek in newLek)
            AddLek(oLek);
      }
      
         
      public void AddLek(Lek newLek)
      {
         if (newLek == null)
            return;
         if (this.lek == null)
            this.lek = new System.Collections.ArrayList();
         if (!this.lek.Contains(newLek))
            this.lek.Add(newLek);
      }
      
         
      public void RemoveLek(Lek oldLek)
      {
         if (oldLek == null)
            return;
         if (this.lek != null)
            if (this.lek.Contains(oldLek))
               this.lek.Remove(oldLek);
      }
      
         
      public void RemoveAllLek()
      {
         if (lek != null)
            lek.Clear();
      }
      public System.Collections.ArrayList potrosnaOprema;
      
      
      public System.Collections.ArrayList GetPotrosnaOprema()
      {
         if (potrosnaOprema == null)
            potrosnaOprema = new System.Collections.ArrayList();
         return potrosnaOprema;
      }
      
      
      public void SetPotrosnaOprema(System.Collections.ArrayList newPotrosnaOprema)
      {
         RemoveAllPotrosnaOprema();
         foreach (PotrosnaOprema oPotrosnaOprema in newPotrosnaOprema)
            AddPotrosnaOprema(oPotrosnaOprema);
      }
      
         
      public void AddPotrosnaOprema(PotrosnaOprema newPotrosnaOprema)
      {
         if (newPotrosnaOprema == null)
            return;
         if (this.potrosnaOprema == null)
            this.potrosnaOprema = new System.Collections.ArrayList();
         if (!this.potrosnaOprema.Contains(newPotrosnaOprema))
            this.potrosnaOprema.Add(newPotrosnaOprema);
      }
      
         
      public void RemovePotrosnaOprema(PotrosnaOprema oldPotrosnaOprema)
      {
         if (oldPotrosnaOprema == null)
            return;
         if (this.potrosnaOprema != null)
            if (this.potrosnaOprema.Contains(oldPotrosnaOprema))
               this.potrosnaOprema.Remove(oldPotrosnaOprema);
      }
      
         
      public void RemoveAllPotrosnaOprema()
      {
         if (potrosnaOprema != null)
            potrosnaOprema.Clear();
      }
      public System.Collections.ArrayList stacionarnaOprema;
      
      
      public System.Collections.ArrayList GetStacionarnaOprema()
      {
         if (stacionarnaOprema == null)
            stacionarnaOprema = new System.Collections.ArrayList();
         return stacionarnaOprema;
      }
      
      
      public void SetStacionarnaOprema(System.Collections.ArrayList newStacionarnaOprema)
      {
         RemoveAllStacionarnaOprema();
         foreach (StacionarnaOprema oStacionarnaOprema in newStacionarnaOprema)
            AddStacionarnaOprema(oStacionarnaOprema);
      }
      
         
      public void AddStacionarnaOprema(StacionarnaOprema newStacionarnaOprema)
      {
         if (newStacionarnaOprema == null)
            return;
         if (this.stacionarnaOprema == null)
            this.stacionarnaOprema = new System.Collections.ArrayList();
         if (!this.stacionarnaOprema.Contains(newStacionarnaOprema))
            this.stacionarnaOprema.Add(newStacionarnaOprema);
      }
      
         
      public void RemoveStacionarnaOprema(StacionarnaOprema oldStacionarnaOprema)
      {
         if (oldStacionarnaOprema == null)
            return;
         if (this.stacionarnaOprema != null)
            if (this.stacionarnaOprema.Contains(oldStacionarnaOprema))
               this.stacionarnaOprema.Remove(oldStacionarnaOprema);
      }
      
         
      public void RemoveAllStacionarnaOprema()
      {
         if (stacionarnaOprema != null)
            stacionarnaOprema.Clear();
      }
   
   }
}