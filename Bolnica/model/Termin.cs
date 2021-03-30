using System;

namespace Model
{
   public class Termin
   {
      public Prostorija prostorija;
      
      
      public Prostorija GetProstorija()
      {
         return prostorija;
      }
      
      
      public void SetProstorija(Prostorija newProstorija)
      {
         if (this.prostorija != newProstorija)
         {
            if (this.prostorija != null)
            {
               Prostorija oldProstorija = this.prostorija;
               this.prostorija = null;
               oldProstorija.RemoveTermin(this);
            }
            if (newProstorija != null)
            {
               this.prostorija = newProstorija;
               this.prostorija.AddTermin(this);
            }
         }
      }
      public Lekar lekar;
      public Pacijent pacijent;
   
      private DateTime DatumIVremeTermina;
      private double TrajanjeTermina;
      private int VrstaTermina;
   
   }
}