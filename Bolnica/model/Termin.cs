using Model.Enum;
using System;

namespace Model
{
   public class Termin
   {
      public Prostorija prostorija;
      
      
      public Prostorija GetProstorija()
      {
         return prostorija;
      }
      
      
      public void SetProstorija(Prostorija newProstorija)
      {
         if (this.prostorija != newProstorija)
         {
            if (this.prostorija != null)
            {
               Prostorija oldProstorija = this.prostorija;
               this.prostorija = null;
               oldProstorija.RemoveTermin(this);
            }
            if (newProstorija != null)
            {
               this.prostorija = newProstorija;
               this.prostorija.AddTermin(this);
            }
         }
      }
        private Lekar lekar;
        private Pacijent pacijent;

        private DateTime datumIVremeTermina;
        private double trajanjeTermina;
        private VrstaPregleda vrstaTermina;

        public Lekar Lekar { get => lekar; set => lekar = value; }
        public Pacijent Pacijent { get => pacijent; set => pacijent = value; }
        public DateTime DatumIVremeTermina { get => datumIVremeTermina; set => datumIVremeTermina = value; }
        public double TrajanjeTermina { get => trajanjeTermina; set => trajanjeTermina = value; }
        public VrstaPregleda VrstaTermina { get => vrstaTermina; set => vrstaTermina = value; }


    }
}