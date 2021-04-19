using Model.Enum;
using Repozitorijum;
using System;

namespace Model
{
    public class Termin
    {
        public DateTime DatumIVremeTermina { get; set; }

        public VrstaPregleda VrstaTermina { get; set; }

        public int IdProstorije { get; set; }
        public String JmbgLekara { get; set; }
        public String JmbgPacijenta { get; set; }
        public String IDTermina { get; set; }
        public String lekar
        {
            get
            {
                Lekar l=SkladisteZaLekara.GetInstance().getByJmbg(JmbgLekara);
                return l.FullName;
            }
            set
            {

            }
        }
       public String pacijent
        {
           get
            {
                Pacijent p = SkladistePacijenta.GetInstance().getByJmbg(JmbgPacijenta);
                return p.FullName;
            }
            set
            {

            }
        }

        public double TrajanjeTermina { get; set; } //u satima
        public String opisTegobe { get; set; }
        public Termin()
        {

        }

        public Termin(Prostorija pros, String l, String p, DateTime dt, double tr, VrstaPregleda vp)
        {
            this.IdProstorije = pros.IdProstorije;
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
            this.IdProstorije = pros.IdProstorije;
            this.JmbgLekara = l;
            this.JmbgPacijenta = p;
            this.DatumIVremeTermina = dt;
            this.TrajanjeTermina = tr;
            this.VrstaTermina = vp;
            this.opisTegobe = opisTegobe;
            this.IDTermina = this.generateRandId();
        }

        public int GetProstorija()
        {
            return IdProstorije;
        }

        public void SetProstorija(Prostorija newProstorija)
        {
            // TODO: izmeni zakomentarisano ako je potrebno

            /*if (this.prostorija != newProstorija)
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
            }*/
            IdProstorije = newProstorija.IdProstorije;
        }

        public String generateRandId()
        {
            return JmbgPacijenta + JmbgLekara + DatumIVremeTermina.ToString();
        }




    }
}