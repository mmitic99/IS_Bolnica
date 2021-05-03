using Bolnica.DTOs;
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

        internal bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(DateTime datum)
        {
            if (DateTime.Now < datum.Date.AddDays(15)) return false;
            else return true;
        }
    }
}
