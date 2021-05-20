using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    class AnketeServis
    {
        public static AnketeServis instance;
        public ISkladisteZaKvartalneAnkete skladisteZaKvartalneAnkete;
        public static AnketeServis GetInstance()
        {
            if (instance == null) return new AnketeServis();
            else return instance;
        }

        public AnketeServis()
        {
            instance = this;
            skladisteZaKvartalneAnkete = new SkladisteZaKvartalneAnketeXml();
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
            dodajPopunjenuAnketu(popunjenaAnketa, kvartalnaAnketa);
            return SacuvajIzmenjenuKvartalnuAnketu(kvartalnaAnketa);
        }

        public void dodajPopunjenuAnketu(PopunjenaKvartalnaAnketaDTO anketa, KvartalnaAnketa kvartalnaAnketa)
        {
            kvartalnaAnketa.StrucnostMedicinskogOsoboljaProsecnaOcena = azuriranjeProsecneOcene(anketa.StrucnostMedicinskogOsobolja, kvartalnaAnketa.StrucnostMedicinskogOsoboljaProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.LjubaznostMedicinskogOsoboljaProsecnaOcena = azuriranjeProsecneOcene(anketa.LjubaznostMedicinskogOsobolja, kvartalnaAnketa.LjubaznostMedicinskogOsoboljaProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.LjubaznostNemedicnskogOsobljaProsecnaOcena = azuriranjeProsecneOcene(anketa.LjubaznostNemedicnskogOsoblja, kvartalnaAnketa.LjubaznostNemedicnskogOsobljaProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoTelefonaProsecnaOcena = azuriranjeProsecneOcene(anketa.JednostavnostZakazivanjaTerminaPrekoTelefona, kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoTelefonaProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoAplikacijeProsecnaOcena = azuriranjeProsecneOcene(anketa.JednostavnostZakazivanjaTerminaPrekoAplikacije, kvartalnaAnketa.JednostavnostZakazivanjaTerminaPrekoAplikacijeProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.DostupnostTerminaURazumnomRokuProsecnaOcena = azuriranjeProsecneOcene(anketa.DostupnostTerminaURazumnomRoku, kvartalnaAnketa.DostupnostTerminaURazumnomRokuProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.InformacijeOOdlozenomTerminuProsecnaOcena = azuriranjeProsecneOcene(anketa.InformacijeOOdlozenomTerminu, kvartalnaAnketa.InformacijeOOdlozenomTerminuProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.DostupnostLekaraKadaJeBolnicaZatvorenaProsecnaOcena = azuriranjeProsecneOcene(anketa.DostupnostLekaraKadaJeBolnicaZatvorena, kvartalnaAnketa.DostupnostLekaraKadaJeBolnicaZatvorenaProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.DostupnostLekaraUTokuRadnihSatiLekaraProsecnaOcena = azuriranjeProsecneOcene(anketa.DostupnostLekaraUTokuRadnihSatiLekara, kvartalnaAnketa.DostupnostLekaraUTokuRadnihSatiLekaraProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.RezultatiTestovaDostupniURazumnoVremeProsecnaOcena = azuriranjeProsecneOcene(anketa.RezultatiTestovaDostupniURazumnoVreme, kvartalnaAnketa.RezultatiTestovaDostupniURazumnoVremeProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.IzgledNaseBolniceProsecnaOcena = azuriranjeProsecneOcene(anketa.IzgledNaseBolnice, kvartalnaAnketa.IzgledNaseBolniceProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.OpremljenostBolniceProsecnaOcena = azuriranjeProsecneOcene(anketa.OpremljenostBolnice, kvartalnaAnketa.OpremljenostBolniceProsecnaOcena, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.CelokupniUtisak = azuriranjeProsecneOcene(anketa.CelokupniUtisak, kvartalnaAnketa.CelokupniUtisak, kvartalnaAnketa.BrojAnketiranih);
            kvartalnaAnketa.komentari.Add(anketa.KomentarKorisnika);
            kvartalnaAnketa.anketuPopunili.Add(anketa.JmbgKorisnika);
            ++kvartalnaAnketa.BrojAnketiranih;
        }

        public double azuriranjeProsecneOcene(double novaOcena, double ProsecnaOcena, int BrojAnketiranih)
        {
            return (ProsecnaOcena * BrojAnketiranih + novaOcena) / (BrojAnketiranih + 1);
        }

        private bool SacuvajIzmenjenuKvartalnuAnketu(KvartalnaAnketa kvartalnaAnketa)
        {
            List<KvartalnaAnketa> ankete = skladisteZaKvartalneAnkete.GetAll();
            for(int i=0; i<ankete.Count; i++)
            {
                if(ankete[i].datum.Equals(kvartalnaAnketa.datum))
                {
                    ankete[i] = kvartalnaAnketa;
                    break;
                }
            }
            skladisteZaKvartalneAnkete.SaveAll(ankete);
            return true;
        }

        public KvartalnaAnketa GetKvartalnaAnketa(DateTime datumAnkete)
        {
            List<KvartalnaAnketa> sveAnkete = skladisteZaKvartalneAnkete.GetAll();
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
            skladisteZaKvartalneAnkete.Save(kvartalnaAnketa);
            return kvartalnaAnketa;
        }

        internal bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            List<KvartalnaAnketa> sveAnkete = skladisteZaKvartalneAnkete.GetAll();
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
            List<AnketaLekar> sveAnkete = SkladisteZaAnketeOLekaruXml.GetInstance().GetAll();
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
            SkladisteZaAnketeOLekaruXml.GetInstance().Save(anketaOLekaru);
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
            List<AnketaLekar> ankete = SkladisteZaAnketeOLekaruXml.GetInstance().GetAll();
            for (int i = 0; i < ankete.Count; i++)
            {
                if (ankete[i].JmbgLekara.Equals(anketaOLekaru.JmbgLekara))
                {
                    ankete[i] = anketaOLekaru;
                    break;
                }
            }
            SkladisteZaAnketeOLekaruXml.GetInstance().SaveAll(ankete);
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
