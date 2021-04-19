using Model.Enum;
using System;

namespace Model
{
   public class Lek
   {
        public Lek()
        {

        }
      public VrstaLeka VrstaLeka { get; set; }
      private double KolicinaLeka { get; set; }
      public String NazivLeka { get; set; }
      public KlasaLeka KlasaLeka { get; set; }
      public int IdLeka { get; set; }
   
   }
}