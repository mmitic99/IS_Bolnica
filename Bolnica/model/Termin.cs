using Model.Enum;
using System;

namespace Model
{
    public class Termin
    {
        public DateTime DatumIVremeTermina { get; set; }

        public VrstaPregleda VrstaTermina { get; set; }

        public Prostorija prostorija { get; set; }
        public String JmbgLekara { get; set; }
        public String JmbgPacijenta { get; set; }
        public String IDTermina { get; set; }

        public double TrajanjeTermina { get; set; } //u satima
        public String opisTegobe { get; set; }
        public Termin()
        {

        }

        public Termin(Prostorija pros, String l, String p, DateTime dt, double tr, VrstaPregleda vp)
        {
            this.prostorija = pros;
            this.JmbgLekara = l;
            this.JmbgPacijenta = p;
            this.DatumIVremeTermina = dt;
            this.TrajanjeTermina = tr;
            this.VrstaTermina = vp;
            this.opisTegobe = opisTegobe;
            this.IDTermina = this.generateRandId();
        }

        public Termin(Prostorija pros, String l, String p, DateTime dt, double tr, VrstaPregleda vp, String opisTegobe)
        {
            this.prostorija = pros;
            this.JmbgLekara = l;
            this.JmbgPacijenta = p;
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
            return JmbgPacijenta + JmbgLekara + DatumIVremeTermina.ToString();
        }




    }
}