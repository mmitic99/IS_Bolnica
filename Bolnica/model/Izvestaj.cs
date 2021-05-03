using System;

using System.Collections;
using System.Collections.Generic;

namespace Model
{
   public class Izvestaj
   {


        public List<Recept> recepti { get; set; }
        public DateTime datum { get; set;}
        public String dijagnoza { get; set; }
    

        public Izvestaj(List<Recept> recept)
        {
            this.recepti = recept;
        }

        public Izvestaj()
        {
        }

       
    }
}