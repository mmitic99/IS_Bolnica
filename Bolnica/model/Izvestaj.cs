using System;

using System.Collections;
using System.Collections.Generic;

namespace Model
{
   public class Izvestaj
   {


        public List<Recept> recepti { get; set; }

      
      
     /* public List<Recept> GetRecept()
      {
         if (recept == null)
            recept = new List<Recept>();
         return recept;
      }
      
      
      public void SetRecept(List<Recept> newRecept)
      {
         RemoveAllRecept();
         foreach (Recept oRecept in newRecept)
            AddRecept(oRecept);
      }
      
         
      public void AddRecept(Recept newRecept)
      {
         if (newRecept == null)
            return;
         if (this.recept == null)
            this.recept = new List<Recept>();
         if (!this.recept.Contains(newRecept))
            this.recept.Add(newRecept);
      }
      
         
      public void RemoveRecept(Recept oldRecept)
      {
         if (oldRecept == null)
            return;
         if (this.recept != null)
            if (this.recept.Contains(oldRecept))
               this.recept.Remove(oldRecept);
      }
      
         
      public void RemoveAllRecept()
      {
         if (recept != null)
            recept.Clear();
      }*/
   
    

        public Izvestaj(List<Recept> recept)
        {
            this.recepti = recept;
        }

        public Izvestaj()
        {
        }

       
    }
}