using System;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model;
using Model.Enum;
using Repozitorijum;

namespace Bolnica.model
{
    public class Termin
    {
        public DateTime DatumIVremeTermina { get; set; }

        public VrstaPregleda VrstaTermina { get; set; }

        public int IdProstorije { get; set; }
        public String JmbgLekara { get; set; }
        public String JmbgPacijenta { get; set; }
        public String IDTermina { get; set; }

        public String brojSobe
        {
            get
            {
                return SkladisteZaProstorijeXml.GetInstance().GetById(this.IdProstorije).BrojSobe;
            }
            set
            {

            }
        }

        public int sprat
        {
            get
            {
                return SkladisteZaProstorijeXml.GetInstance().GetById(this.IdProstorije).Sprat;
            }
            set
            {

            }
        }
        public DateTime poslednjiDatumOtkazivanja
        {
            get
            {
                return DatumIVremeTermina.Date.AddSeconds(-1);
            }
            
        }
        public String lekar
        {
            get
            {
                Lekar l=SkladisteZaLekaraXml.GetInstance().getByJmbg(JmbgLekara);
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
                Pacijent p = SkladistePacijentaXml.GetInstance().GetByJmbg(JmbgPacijenta);
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


        public void SetProstorija(Prostorija newProstorija)
        {
            IdProstorije = newProstorija.IdProstorije;
        }

        public String generateRandId()
        {
            return JmbgPacijenta + JmbgLekara + DatumIVremeTermina.ToString();
        }




    }
}