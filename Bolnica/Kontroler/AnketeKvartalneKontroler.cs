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
    class AnketeKvartalneKontroler
    {
        private AnketeKvartalneServis AnketeKvartalneServis;

        public AnketeKvartalneKontroler()
        {
            this.AnketeKvartalneServis = new AnketeKvartalneServis();
        }

        public bool SacuvajKvartalnuAnketu(KvartalnaAnketaDTO kvartalnaAnketa)
        {
            bool uspesnoSacuvana = false;
            if (uneteSveOcene(kvartalnaAnketa))
            {
                PopunjenaKvartalnaAnketaDTO popunjenaAnketa = KlasifikujParametrePopunjeneKvartalne(kvartalnaAnketa);
                uspesnoSacuvana = AnketeKvartalneServis.SacuvajKvartalnuAnketu(popunjenaAnketa);
            }
            return uspesnoSacuvana;
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

        public KvartalnaAnketa GetByDate(DateTime kvartalnaAnketa)
        {
            return AnketeKvartalneServis.GetKvartalnaAnketa(kvartalnaAnketa);
        }

        internal bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(DateTime DatumZakaceneKvartalne)
        {
            return AnketeKvartalneServis.DaLiJeIstekloVremeZaPopunjavanjeAnkete(DatumZakaceneKvartalne);
        }

        internal bool DaLiJeKorisnikPopunioAnketu(String JmbgPacijenta, DateTime datumZakaceneKvartalne)
        {
            return AnketeKvartalneServis.DaLiJeKorisnikPopunioKvartalnuAnketu(JmbgPacijenta, datumZakaceneKvartalne);
        }

        internal bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            return AnketeKvartalneServis.DaLiJePoslataKvartalnaAnketa(today);
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
                (String)kvartalnaAnketa.RezultatiTestovaDostupniURazumnoVreme != "-1" &&
                (String)kvartalnaAnketa.StrucnostMedicinskogOsobolja != "-1" &&
                (String)kvartalnaAnketa.CelokupniUtisak != "-1" &&
                (String)kvartalnaAnketa.DostupnostLekaraUTokuRadnihSatiLekara != "-1" &&
                (String)kvartalnaAnketa.DostupnostLekaraKadaJeBolnicaZatvorena != "-1" &&
                (String)kvartalnaAnketa.DostupnostTerminaURazumnomRoku != "-1")
            {
                UneteSveOcene = true;
            }
            return UneteSveOcene;
        }
    }
}
