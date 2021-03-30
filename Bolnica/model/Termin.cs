using Model.Enum;
using System;

namespace Model
{
    public class Termin
    {
        public DateTime DatumIVremeTermina { get; set; }

        public VrstaPregleda VrstaTermina { get; set; }

        public Prostorija prostorija { get; set; }
        public Lekar lekar { get; set; }
        public Pacijent pacijent { get; set; }
        public String IDTermina { get; set; }

        public double TrajanjeTermina { get; set; } //u satima
        public String opisTegobe { get; set; }
        public Termin()
        {

        }

        public Termin(Prostorija pros, Lekar l, Pacijent p, DateTime dt, double tr, VrstaPregleda vp, String opisTegobe)
        {
            this.prostorija = pros;
            this.lekar = l;
            this.pacijent = p;
            this.DatumIVremeTermina = dt;
            this.TrajanjeTermina = tr;
            this.VrstaTermina = vp;
            this.opisTegobe = opisTegobe;
            this.IDTermina = this.generateRandId();
        }

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

        public String generateRandId()
        {
            return pacijent.Jmbg + lekar.Jmbg + DatumIVremeTermina.ToString();
        }




    }
}