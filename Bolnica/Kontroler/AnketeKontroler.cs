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
        private AnketeServis AnketeServis;
        public static AnketeKontroler instance;
        public static AnketeKontroler GetInstance()
        {
            if (instance == null) return new AnketeKontroler();
            else return instance;
        }
        public AnketeKontroler()
        {
            instance = this;
            this.AnketeServis = new AnketeServis();
        }

        internal bool DaLiJeKorisnikPopunioAnketu(Pacijent pacijent, model.KvartalnaAnketa anketa)
        {
            return AnketeServis.DaLiJeKorisnikPopunioKvartalnuAnketu(pacijent.Jmbg, anketa);
        }

        public bool SacuvajKvartalnuAnketu(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            if (uneteSveOcene(kvartalnaAnketa))
            {
                PopunjenaKvartalnaAnketaDTO popunjenaAnketa = KlasifikujParametrePopunjeneKvartalne(kvartalnaAnketa);
                return AnketeServis.SacuvajKvartalnuAnketu(popunjenaAnketa);
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

        public bool DaLiJeVremeZaKvartalnuAnketu()
        {
            return ((DateTime.Today.Date.Day.Equals(24) && DateTime.Today.Month.Equals(5)) //anketa prvog kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(6)) //anketa drugog kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(9)) //anketa treceg kvartala
                || (DateTime.Today.Date.Day.Equals(1) && DateTime.Today.Month.Equals(12))) //anketa cetvrtog kvartala
                && !DaLiJePoslataKvartalnaAnketa(DateTime.Today);
        }

        public bool SacuvajAnketuOLekaru(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            bool uspesnoSacuvana = false;
            if (uneteSveOceneLekarAnketa(popunjena))
            {
                PopunjenaAnketaPoslePregledaDTO popunjenaAnketa = KlasifikujParametrePopunjeneAnketeOLekaru(popunjena);
                uspesnoSacuvana = AnketeServis.SacuvajAnketuOLekaru(popunjenaAnketa);
            }
            return uspesnoSacuvana;
        }

        private bool uneteSveOceneLekarAnketa(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            bool isPopunjena = false;
            if ((String)popunjena.Ocena != "-1" && popunjena.Komentar != null)
            {
                isPopunjena = true;
            }
            return isPopunjena;
        }

        private PopunjenaAnketaPoslePregledaDTO KlasifikujParametrePopunjeneAnketeOLekaru(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            PopunjenaAnketaPoslePregledaDTO popunjenaAnketa = new PopunjenaAnketaPoslePregledaDTO()
            {
                IDAnkete = (String)popunjena.IDAnkete,
                JmbgLekara = (String)popunjena.JmbgLekara,
                Komentar = (String)popunjena.Komentar,
                Ocena = Double.Parse((String)popunjena.Ocena)
            };
            return popunjenaAnketa;
        }

        internal AnketaLekar GetAnketaOLekaruByJmbg(string jmbgLekara)
        {
            return AnketeServis.GetAnketaOLekaru(jmbgLekara);
        }

        internal KvartalnaAnketa GetByDate(DateTime kvartalnaAnketa)
        {
            return AnketeServis.GetKvartalnaAnketa(kvartalnaAnketa);
        }

        internal bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(KvartalnaAnketa anketa)
        {
            return AnketeServis.DaLiJeIstekloVremeZaPopunjavanjeAnkete(anketa.datum);
        }

        internal bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            return AnketeServis.DaLiJePoslataKvartalnaAnketa(today);
        }

        internal bool DaLiJeKorisnikPopunioAnketu(PrikacenaAnketaPoslePregledaDTO anketaOLekaru)
        {
            return AnketeServis.DaLiJeKorisnikPopunioAnketuOLekaru(anketaOLekaru);
        }

        private bool uneteSveOcene(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            bool UneteSveOcene = false;
            if ((String)kvartalnaAnketa.LjubaznostMedicinskogOsobolja != "-1" &&
                (String)kvartalnaAnketa.InformacijeOOdlozenomTerminu != "-1" &&
                (String)kvartalnaAnketa.IzgledNaseBolnice != "-1" &&
                (String)kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoAplikacije != "-1" &&
                (String)kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoTelefona != "-1" &&
                (String)kvartalnaAnketa.LjubaznostNemedicnskogOsoblja != "-1" &&
                (String)kvartalnaAnketa.OpremljenostBolnice != "-1" &&
                (String)kvartalnaAnketa.RezultatiTestovaDostupniURazumnoVreme!="-1"&&
                (String)kvartalnaAnketa.StrucnostMedicinskogOsobolja != "-1" &&
                (String)kvartalnaAnketa.CelokupniUtisak != "-1" &&
                (String)kvartalnaAnketa.DostupnostLekaraUTokuRadnihSatiLekara != "-1" &&
                (String)kvartalnaAnketa.DostupnostLekaraKadaJeBolnicaZatvorena!="-1" &&
                (String)kvartalnaAnketa.DostupnostTerminaURazumnomRoku != "-1")
            {
                UneteSveOcene = true;
            }
            return UneteSveOcene;
        }
    }
}
