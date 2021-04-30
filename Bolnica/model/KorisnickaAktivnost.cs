using Model.Enum;
using System;

namespace Model
{
   public class KorisnickaAktivnost
   {
      public Model.Enum.VrstaKorisnickeAkcije VrstaAktivnosti { get; set; }
      public DateTime DatumIVreme { get; set; }
        public KorisnickaAktivnost()
        {

        }
        public KorisnickaAktivnost(VrstaKorisnickeAkcije vrsta, DateTime datum)
        {
            this.VrstaAktivnosti = vrsta;
            this.DatumIVreme = datum;
        }
   
   }
}