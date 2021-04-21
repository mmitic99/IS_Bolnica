using System;

namespace Model
{
   public class StacionarnaOprema
   {
      private String TipStacionarneOpreme { get; set; }
      private int Kolicina { get; set; }

        public StacionarnaOprema() { }

        public StacionarnaOprema(String tip, int kolicina)
        {
            TipStacionarneOpreme = tip;
            Kolicina = kolicina;
        }

        public String TipStacionarneOpreme_
        {
            get
            {
                return TipStacionarneOpreme;
            }
            set
            {
                if (value != TipStacionarneOpreme)
                {
                    TipStacionarneOpreme = value;
                }
            }
        }
        public int Kolicina_
        {
            get
            {
                return Kolicina;
            }
            set
            {
                if (value != Kolicina)
                {
                    Kolicina = value;
                }
            }
        }
    }
}