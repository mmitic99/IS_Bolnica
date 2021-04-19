using System;
using System.Collections.Generic;

namespace Model
{
   public class Izvestaj
   {
      public System.Collections.ArrayList recept;


        public List<Recept> recepti { get; set; }




        public System.Collections.ArrayList GetRecept()
      {
         if (recept == null)
            recept = new System.Collections.ArrayList();
         return recept;
      }
      
      
      public void SetRecept(System.Collections.ArrayList newRecept)
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
            this.recept = new System.Collections.ArrayList();
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
      }
   
      private String Anamneza;
   
   }
}