using System;

namespace Model
{
   public class PotrosnaOprema
   {
      public String TipOpreme { get; set; }
      public int KolicinaOpreme { get; set; }

        public PotrosnaOprema() {}

        public PotrosnaOprema(String tip, int kolicina)
        {
            TipOpreme = tip;
            KolicinaOpreme = kolicina;
        }

    }
}