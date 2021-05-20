using Bolnica.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class KvartalnaAnketa
    {
        public DateTime datum { get; set; }
        public int BrojAnketiranih { get; set; }
        //osoblje
        public double StrucnostMedicinskogOsoboljaProsecnaOcena { get; set; }
        public double LjubaznostMedicinskogOsoboljaProsecnaOcena { get; set; }
        public double LjubaznostNemedicnskogOsobljaProsecnaOcena { get; set; }

        //termini
        public double JednostavnostZakazivanjaTerminaPrekoTelefonaProsecnaOcena { get; set; }
        public double JednostavnostZakazivanjaTerminaPrekoAplikacijeProsecnaOcena { get; set; }
        public double DostupnostTerminaURazumnomRokuProsecnaOcena { get; set; }
        public double InformacijeOOdlozenomTerminuProsecnaOcena { get; set; }

        //komunikacija
        public double DostupnostLekaraKadaJeBolnicaZatvorenaProsecnaOcena { get; set; }
        public double DostupnostLekaraUTokuRadnihSatiLekaraProsecnaOcena { get; set; }
        public double RezultatiTestovaDostupniURazumnoVremeProsecnaOcena { get; set; }

        //bolnica
        public double IzgledNaseBolniceProsecnaOcena { get; set; }
        public double OpremljenostBolniceProsecnaOcena { get; set; }
        public double CelokupniUtisak { get; set; }
        public List<String> komentari { get; set; }
        public List<String> anketuPopunili { get; set; }

        public KvartalnaAnketa()
        {

        }

        public KvartalnaAnketa(DateTime datum)
        {
            this.datum = datum;
            StrucnostMedicinskogOsoboljaProsecnaOcena = 0.0;
            LjubaznostMedicinskogOsoboljaProsecnaOcena = 0.0;
            LjubaznostNemedicnskogOsobljaProsecnaOcena = 0.0;
            JednostavnostZakazivanjaTerminaPrekoAplikacijeProsecnaOcena = 0.0;
            JednostavnostZakazivanjaTerminaPrekoTelefonaProsecnaOcena = 0.0;
            DostupnostTerminaURazumnomRokuProsecnaOcena = 0.0;
            InformacijeOOdlozenomTerminuProsecnaOcena = 0.0;
            DostupnostLekaraUTokuRadnihSatiLekaraProsecnaOcena = 0.0;
            DostupnostLekaraKadaJeBolnicaZatvorenaProsecnaOcena = 0.0;
            RezultatiTestovaDostupniURazumnoVremeProsecnaOcena =0.0;
            IzgledNaseBolniceProsecnaOcena = 0.0;
            OpremljenostBolniceProsecnaOcena = 0.0;
            CelokupniUtisak = 0.0;
            komentari = new List<String>();
            anketuPopunili = new List<String>();
        }



    }
}
