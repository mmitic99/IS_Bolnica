using Model.Enum;
using System;

namespace Model
{
   public class Lek
   {
      public VrstaLeka VrstaLeka { get; set; }
      private double KolicinaLeka { get; set; }
      public String NazivLeka { get; set; }
      private KlasaLeka KlasaLeka { get; set; }
      private int IdLeka { get; set; }
   
   }
}