using Model.Enum;
using System;

namespace Model
{
   public class Prostorija
   {
      public void RenovirajProstoriju()
      {
         // TODO: implement
      }
      
      public void ZavrsiRenoviranje()
      {
         // TODO: implement
      }
   
      public InventarZaProstorije inventarZaProstorije;
      public System.Collections.ArrayList termin;
      
      
      public System.Collections.ArrayList GetTermin()
      {
         if (termin == null)
            termin = new System.Collections.ArrayList();
         return termin;
      }
      
      
      public void SetTermin(System.Collections.ArrayList newTermin)
      {
         RemoveAllTermin();
         foreach (Termin oTermin in newTermin)
            AddTermin(oTermin);
      }
      
         
      public void AddTermin(Termin newTermin)
      {
         if (newTermin == null)
            return;
         if (this.termin == null)
            this.termin = new System.Collections.ArrayList();
         if (!this.termin.Contains(newTermin))
         {
            this.termin.Add(newTermin);
            newTermin.SetProstorija(this);      
         }
      }
      
         
      public void RemoveTermin(Termin oldTermin)
      {
         if (oldTermin == null)
            return;
         if (this.termin != null)
            if (this.termin.Contains(oldTermin))
            {
               this.termin.Remove(oldTermin);
               oldTermin.SetProstorija((Prostorija)null);
            }
      }
      
         
      public void RemoveAllTermin()
      {
         if (termin != null)
         {
            System.Collections.ArrayList tmpTermin = new System.Collections.ArrayList();
            foreach (Termin oldTermin in termin)
               tmpTermin.Add(oldTermin);
            termin.Clear();
            foreach (Termin oldTermin in tmpTermin)
               oldTermin.SetProstorija((Prostorija)null);
            tmpTermin.Clear();
         }
      }
   
      private Sprat Sprat;
      private String BrojSobe;
      private VrstaProstorije VrstaProstorije;
      private int IdProstorije;
      private bool RenoviraSe = false;
      private double Kvadratura;
   
   }
}