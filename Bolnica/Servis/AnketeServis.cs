﻿using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis
{
    class AnketeServis
    {
        public static AnketeServis instance;
        public static AnketeServis GetInstance()
        {
            if (instance == null) return new AnketeServis();
            else return instance;
        }

        public AnketeServis()
        {
            instance = this;
        }

        public bool DaLiJeKorisnikPopunioAnketu(string jmbg, KvartalnaAnketa anketa)
        {
            KvartalnaAnketa kvartalnaAnketa = GetKvartalnaAnketa(anketa.datum);
            foreach (String jmbgPopunjenih in kvartalnaAnketa.anketuPopunili)
            {
                if (jmbgPopunjenih.Equals(jmbg))
                    return true;
            }
            return false;
        }

        public bool SacuvajKvartalnuAnketu(PopunjenaKvartalnaAnketaDTO popunjenaAnketa)
        {
            KvartalnaAnketa kvartalnaAnketa = GetKvartalnaAnketa(popunjenaAnketa.datumAnkete);
            kvartalnaAnketa.dodajPopunjenuAnketu(popunjenaAnketa);
            return SacuvajIzmenjenuKvartalnuAnketu(kvartalnaAnketa);
        }

        private bool SacuvajIzmenjenuKvartalnuAnketu(KvartalnaAnketa kvartalnaAnketa)
        {
            List<KvartalnaAnketa> ankete = SkladisteZaKvartalneAnkete.GetInstance().GetAll();
            for(int i=0; i<ankete.Count; i++)
            {
                if(ankete[i].datum.Equals(kvartalnaAnketa.datum))
                {
                    ankete[i] = kvartalnaAnketa;
                    break;
                }
            }
            SkladisteZaKvartalneAnkete.GetInstance().SaveAll(ankete);
            return true;
        }

        public KvartalnaAnketa GetKvartalnaAnketa(DateTime datumAnkete)
        {
            List<KvartalnaAnketa> sveAnkete = SkladisteZaKvartalneAnkete.GetInstance().GetAll();
            foreach (KvartalnaAnketa anketa in sveAnkete)
            {
                if (anketa.datum.Equals(datumAnkete))
                    return anketa;
            }
            return kreirajNovuKvartalnuAnketu(datumAnkete);
        }

        private KvartalnaAnketa kreirajNovuKvartalnuAnketu(DateTime datumAnkete)
        {
            KvartalnaAnketa kvartalnaAnketa = new KvartalnaAnketa(datumAnkete);
            SkladisteZaKvartalneAnkete.GetInstance().Save(kvartalnaAnketa);
            return kvartalnaAnketa;
        }

        internal bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            List<KvartalnaAnketa> sveAnkete = SkladisteZaKvartalneAnkete.GetInstance().GetAll();
            foreach (KvartalnaAnketa anketa in sveAnkete)
            {
                if (anketa.datum.Date.Equals(today.Date))
                {
                    return true;
                }
            }
            return false;
        }

        internal AnketaLekar GetAnketaOLekaru(string jmbgLekara)
        {
            List<AnketaLekar> sveAnkete = SkladisteZaAnketeOLekaru.GetInstance().GetAll();
            foreach(AnketaLekar anketa in sveAnkete)
            {
                if (anketa.JmbgLekara.Equals(jmbgLekara))
                    return anketa;
            }
            return kreirajNovuAnketuOLekaru(jmbgLekara);
        }

        private AnketaLekar kreirajNovuAnketuOLekaru(string jmbgLekara)
        {
            AnketaLekar anketaOLekaru = new AnketaLekar(jmbgLekara);
            SkladisteZaAnketeOLekaru.GetInstance().Save(anketaOLekaru);
            return anketaOLekaru;
        }

        internal bool SacuvajAnketuOLekaru(PopunjenaAnketaPoslePregledaDTO popunjenaAnketa)
        {
            AnketaLekar anketaOLekaru = GetAnketaOLekaru(popunjenaAnketa.JmbgLekara);
            anketaOLekaru.DodajPopunjenuAnketu(popunjenaAnketa);
            return SacuvajIzmenjenuAnketuOLekaru(anketaOLekaru);
        }

        private bool SacuvajIzmenjenuAnketuOLekaru(AnketaLekar anketaOLekaru)
        {
            List<AnketaLekar> ankete = SkladisteZaAnketeOLekaru.GetInstance().GetAll();
            for (int i = 0; i < ankete.Count; i++)
            {
                if (ankete[i].JmbgLekara.Equals(anketaOLekaru.JmbgLekara))
                {
                    ankete[i] = anketaOLekaru;
                    break;
                }
            }
            SkladisteZaAnketeOLekaru.GetInstance().SaveAll(ankete);
            return true;
        }

        internal bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(DateTime datum)
        {
            if (DateTime.Now < datum.Date.AddDays(15)) return false;
            else return true;
        }

        internal bool DaLiJeKorisnikPopunioAnketu(PrikacenaAnketaPoslePregledaDTO anketaOLekaru)
        {
            AnketaLekar anketa = GetAnketaOLekaru(anketaOLekaru.JmbgLekara);
            foreach (String ID in anketa.ispunjeneAnkete)
                if (ID.Equals(anketaOLekaru.IDAnkete))
                    return true;
            return false;
        }
    }
}