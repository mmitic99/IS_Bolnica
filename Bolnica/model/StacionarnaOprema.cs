using System;

namespace Model
{
   public class StacionarnaOprema
   {
      public String TipStacionarneOpreme { get; set; }
      public int Kolicina { get; set; }

        public StacionarnaOprema() { }

        public StacionarnaOprema(String tip, int kolicina)
        {
            TipStacionarneOpreme = tip;
            Kolicina = kolicina;
        }
    }
}