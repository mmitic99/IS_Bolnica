using Bolnica.Repozitorijum.XmlSkladiste;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Servis.StatePattern;

namespace Servis
{
    public class KorisnickeAktivnostiPacijentaServis
    {
        public const int MAX_BROJ_ZAKAZANIH_PACIjENTA = 5;
        public const int MAX_BROJ_OTKAZIVANJA = 3;
        public ISkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;
        public IStanjeKorisnika TrenutnoStanjeKorisnika;
        public KorisnickeAktivnostiNaAplikaciji korisnickeAktivnostiNaAplikaciji { get; set; }
        public TerminServis TerminServis { get;}

        public KorisnickeAktivnostiPacijentaServis(string JmbgKorisnika)
        {
            skladisteZaKorisnickeAktivnosti = new SkladisteZaKorisnickeAktivnostiXml();
            this.TerminServis = new TerminServis();
            this.korisnickeAktivnostiNaAplikaciji = GetByJmbg(JmbgKorisnika);
            TrenutnoStanjeKorisnika = postaviPocetnoStanjeKorisnika();
        }

        private IStanjeKorisnika postaviPocetnoStanjeKorisnika()
        {
            IStanjeKorisnika stanje = new NonSpamStanje(this);
            if (korisnickeAktivnostiNaAplikaciji.BlokiranDo != DateTime.MinValue) stanje = new SpamStanje(this);
            else if(TerminServis.DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(korisnickeAktivnostiNaAplikaciji.JmbgKorisnika) >= MAX_BROJ_ZAKAZANIH_PACIjENTA) stanje = new HalfSpamStanje(this);
            return stanje;
        }

        public bool DaLiJePredZabranuOtkazivanja()
        {
            return TrenutnoStanjeKorisnika.DaLiJePredZabranuOtkazivanja();
        }

        public bool DaLiJePredZabranuZakazivanja()
        {
            return TrenutnoStanjeKorisnika.DaLiJePredZabranuZakazivanja();
        }

        public KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
        {
            KorisnickeAktivnostiNaAplikaciji aktivnosti = skladisteZaKorisnickeAktivnosti.GetByJmbg(jmbgKorisnika);
            if (aktivnosti == null)
            {
                aktivnosti = NapraviNoveKorisnickeAktivnosti(jmbgKorisnika);
            }
            return aktivnosti;
        }

        private KorisnickeAktivnostiNaAplikaciji NapraviNoveKorisnickeAktivnosti(String jmbgKorisnika)
        {
            KorisnickeAktivnostiNaAplikaciji noveAktivnosti = new KorisnickeAktivnostiNaAplikaciji(jmbgKorisnika);
            skladisteZaKorisnickeAktivnosti.Save(noveAktivnosti);
            return noveAktivnosti;
        }

        public bool OdblokirajKorisnika()
        {
            return TrenutnoStanjeKorisnika.OdblokirajKorisnika();
        }

        public bool DaLiMozeDaZakaze()
        {
            return TrenutnoStanjeKorisnika.DaLiMozeDaZakaze();
        }

        public bool DaLiMozeDaPomeri()
        {
            return TrenutnoStanjeKorisnika.DaLiMozeDaPomeri();
        }

        public bool DodajPomeranje()
        {
            return TrenutnoStanjeKorisnika.DodajPomeranje();
        }

        public bool DodajZakazivanje()
        {
            return TrenutnoStanjeKorisnika.DodajZakazivanje();
        }

        public String DobaviPorukuZabrane()
        {
            return TrenutnoStanjeKorisnika.DobaviPorukuZabrane();
        }

        public bool SacuvajIzmenjenekorisnickeAktivnosti(string noviJmbg = null)
        {
            List<KorisnickeAktivnostiNaAplikaciji> aktivnostiSvihKorisnika = skladisteZaKorisnickeAktivnosti.GetAll();
            for (int i = 0; i < aktivnostiSvihKorisnika.Count; i++)
            {
                if (aktivnostiSvihKorisnika[i].JmbgKorisnika.Equals(korisnickeAktivnostiNaAplikaciji.JmbgKorisnika))
                {
                    if (noviJmbg != null)
                        korisnickeAktivnostiNaAplikaciji.JmbgKorisnika = noviJmbg;
                    aktivnostiSvihKorisnika[i] = korisnickeAktivnostiNaAplikaciji;
                    break;
                }
            }
            skladisteZaKorisnickeAktivnosti.SaveAll(aktivnostiSvihKorisnika);
            return true;
        }     
    }
}