using System;

namespace Model
{
   public class PotrosnaOprema
   {
      private String TipOpreme { get; set; }
      private int KolicinaOpreme { get; set; }

        public PotrosnaOprema() {}

        public PotrosnaOprema(String tip, int kolicina)
        {
            TipOpreme = tip;
            KolicinaOpreme = kolicina;
        }

        public String TipOpreme_
        {
            get
            {
                return TipOpreme;
            }
            set
            {
                if (value != TipOpreme)
                {
                    TipOpreme = value;
                }
            }
        }
        public int KolicinaOpreme_
        {
            get
            {
                return KolicinaOpreme;
            }
            set
            {
                if (value != KolicinaOpreme)
                {
                    KolicinaOpreme = value;
                }
            }
        }

    }
}