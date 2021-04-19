using System;
using System.Collections.Generic;

namespace Model
{
   public class Recept
   {
      public Lek lek { get; set; }
   
      public DateTime DatumIzdavanja { get; set; }
      public TimeSpan vremenskiPeriodUzimanja { get; set; }
      
      public List<TimeSpan> terminiUzimanjaTokomDana { get; set;}
      private int BrojPutaPoDanu { get; set; }

      private DateTime DatumIVremePocetka;
   
   }
}