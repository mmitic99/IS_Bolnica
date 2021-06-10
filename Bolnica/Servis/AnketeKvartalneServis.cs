using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    class AnketeKvartalneServis
    {
        public ISkladisteZaKvartalneAnkete skladisteZaKvartalneAnkete;
        public AnketeKvartalneServis()
        {
            skladisteZaKvartalneAnkete = new SkladisteZaKvartalneAnketeXml();
        }

        public bool DaLiJePoslataKvartalnaAnketa(DateTime today)
        {
            bool poslataAnketa = false;
            List<KvartalnaAnketa> sveAnkete = skladisteZaKvartalneAnkete.GetAll();
            foreach (KvartalnaAnketa anketa in sveAnkete)
            {
                if (anketa.datum.Date.Equals(today.Date))
                {
                    poslataAnketa = true;
                    break;
                }
            }
            return poslataAnketa;
        }

        public bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(DateTime datum)
        {
            bool istekloVreme = true;
            if (DateTime.Now < datum.Date.AddDays(15)) istekloVreme = false;
            return istekloVreme;
        }

        public bool DaLiJeKorisnikPopunioKvartalnuAnketu(string jmbg, DateTime DatumZakaceneKvartalne)
        {
            bool popunioAnketu = false;
            KvartalnaAnketa kvartalnaAnketa = GetKvartalnaAnketa(DatumZakaceneKvartalne);
            foreach (String jmbgPopunjenih in kvartalnaAnketa.anketuPopunili)
            {
                if (jmbgPopunjenih.Equals(jmbg))
                {
                    popunioAnketu = true;
                    break;
                }
            }
            return popunioAnketu;
        }

        public KvartalnaAnketa GetKvartalnaAnketa(DateTime datumAnkete)
        {
            KvartalnaAnketa kvartalnaAnketa = null;
            List<KvartalnaAnketa> sveAnkete = skladisteZaKvartalneAnkete.GetAll();
            foreach (KvartalnaAnketa anketa in sveAnkete)
            {
                if (anketa.datum.Equals(datumAnkete))
                {
                    kvartalnaAnketa = anketa;
                    break;
                }
            }
            if (kvartalnaAnketa == null)
            {
                kvartalnaAnketa = kreirajNovuKvartalnuAnketu(datumAnkete);
            }
            return kvartalnaAnketa;
        }

        public bool SacuvajKvartalnuAnketu(PopunjenaKvartalnaAnketaDTO popunjenaAnketa)
        {
            KvartalnaAnketa kvartalnaAnketa = GetKvartalnaAnketa(popunjenaAnketa.datumAnkete);
            DodajPopunjenuKvartalnuAnketu(popunjenaAnketa, kvartalnaAnketa);
            return SacuvajIzmenjenuKvartalnuAnketu(kvartalnaAnketa);
        }

        private KvartalnaAnketa kreirajNovuKvartalnuAnketu(DateTime datumAnkete)
        {
            KvartalnaAnketa kvartalnaAnketa = new KvartalnaAnketa(datumAnkete);
            skladisteZaKvartalneAnkete.Save(kvartalnaAnketa);
            return kvartalnaAnketa;
        }

        private void DodajPopunjenuKvartalnuAnketu(PopunjenaKvartalnaAnketaDTO anketa, KvartalnaAnketa kvartalnaAnketa)
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

        private double azuriranjeProsecneOcene(double novaOcena, double ProsecnaOcena, int BrojAnketiranih)
        {
            return (ProsecnaOcena * BrojAnketiranih + novaOcena) / (BrojAnketiranih + 1);
        }

        private bool SacuvajIzmenjenuKvartalnuAnketu(KvartalnaAnketa kvartalnaAnketa)
        {
            List<KvartalnaAnketa> ankete = skladisteZaKvartalneAnkete.GetAll();
            for (int i = 0; i < ankete.Count; i++)
            {
                if (ankete[i].datum.Equals(kvartalnaAnketa.datum))
                {
                    ankete[i] = kvartalnaAnketa;
                    break;
                }
            }
            skladisteZaKvartalneAnkete.SaveAll(ankete);
            return true;
        }

    }
}

