using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    class AnketeKontroler
    {
        public static AnketeKontroler instance;
        public static AnketeKontroler GetInstance()
        {
            if (instance == null) return new AnketeKontroler();
            else return instance;
        }
        public AnketeKontroler()
        {
            instance = this;
        }

        internal bool DaLiJeKorisnikPopunioAnketu(Pacijent pacijent, model.KvartalnaAnketa anketa)
        {
            return AnketeServis.GetInstance().DaLiJeKorisnikPopunioAnketu(pacijent.Jmbg, anketa);
        }

        internal bool SacuvajKvartalnuAnketu(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            if (uneteSveOcene(kvartalnaAnketa))
            {
              PopunjenaKvartalnaAnketaDTO popunjenaAnketa = KlasifikujParametrePopunjeneKvartalne(kvartalnaAnketa);
               return AnketeServis.GetInstance().SacuvajKvartalnuAnketu(popunjenaAnketa);
            }
            else
                return false;
        }

        private PopunjenaKvartalnaAnketaDTO KlasifikujParametrePopunjeneKvartalne(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            PopunjenaKvartalnaAnketaDTO popunjenaAnketa = new PopunjenaKvartalnaAnketaDTO()
            {
                CelokupniUtisak = Double.Parse((string)kvartalnaAnketa.CelokupniUtisak),
                datumAnkete = kvartalnaAnketa.datumAnkete,
                DostupnostLekaraKadaJeBolnicaZatvorena = Double.Parse((string)kvartalnaAnketa.DostupnostLekaraKadaJeBolnicaZatvorena),
                DostupnostLekaraUTokuRadnihSatiLekara = Double.Parse((string)kvartalnaAnketa.DostupnostLekaraUTokuRadnihSatiLekara),
                DostupnostTerminaURazumnomRoku = Double.Parse((string)kvartalnaAnketa.DostupnostTerminaURazumnomRoku),
                InformacijeOOdlozenomTerminu = Double.Parse((string)kvartalnaAnketa.InformacijeOOdlozenomTerminu),
                IzgledNaseBolnice = Double.Parse((string)kvartalnaAnketa.IzgledNaseBolnice),
                OpremljenostBolnice = Double.Parse((string)kvartalnaAnketa.OpremljenostBolnice),
                JednostavnostZakazivanjaTerminaPrekoAplikacije = Double.Parse((string)kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoAplikacije),
                JednostavnostZakazivanjaTerminaPrekoTelefona = Double.Parse((string)kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoTelefona),
                JmbgKorisnika = ((Pacijent)kvartalnaAnketa.JmbgKorisnika).Jmbg,
                KomentarKorisnika = (String)kvartalnaAnketa.KomentarKorisnika,
                LjubaznostMedicinskogOsobolja = Double.Parse((string)kvartalnaAnketa.LjubaznostMedicinskogOsobolja),
                LjubaznostNemedicnskogOsoblja = Double.Parse((string)kvartalnaAnketa.LjubaznostNemedicnskogOsoblja),
                StrucnostMedicinskogOsobolja = Double.Parse((string)kvartalnaAnketa.StrucnostMedicinskogOsobolja),
                RezultatiTestovaDostupniURazumnoVreme = Double.Parse((string)kvartalnaAnketa.RezultatiTestovaDostupniURazumnoVreme)
            };
            return popunjenaAnketa;
        }

        internal KvartalnaAnketa GetByDate(DateTime kvartalnaAnketa)
        {
            return AnketeServis.GetInstance().GetKvartalnaAnketa(kvartalnaAnketa);
        }

        internal bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(KvartalnaAnketa anketa)
        {
            return AnketeServis.GetInstance().DaLiJeIstekloVremeZaPopunjavanjeAnkete(anketa.datum);
        }

        internal bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            return AnketeServis.GetInstance().DaLiJePoslataKvartalnaAnketa(today);
        }

        private bool uneteSveOcene(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            return true;
        }
    }
}
