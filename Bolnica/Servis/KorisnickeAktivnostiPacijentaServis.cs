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
        public ISkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;
        public IStanjeKorisnika TrenutnoStanjeKorisnika;
        public IStanjeKorisnika Spam { get; set; }
        public IStanjeKorisnika HalfSpam { get; set; }
        public IStanjeKorisnika NonSpam { get; set; }

        public KorisnickeAktivnostiPacijentaServis(string JmbgPacijenta)
        {
            skladisteZaKorisnickeAktivnosti = new SkladisteZaKorisnickeAktivnostiXml();
            this.Spam = new SpamStanje(this);
            this.HalfSpam = new HalfSpamStanje(this);
            this.NonSpam = new NonSpamStanje(this);
        }

        void postaviTrenutnoStanje(IStanjeKorisnika novoStanje)
        {
            TrenutnoStanjeKorisnika = novoStanje;
        }

        public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
        {
            KorisnickeAktivnostiNaAplikaciji aktivnosti = skladisteZaKorisnickeAktivnosti.GetByJmbg(jmbgKorisnika);
            if (aktivnosti == null)
            {
                aktivnosti = NapraviNoveKorisnickeAktivnosti(jmbgKorisnika);
            }
            return aktivnosti;
        }

        public Model.Enum.VrstaKorisnikaAplikacije GetRangKorisnika(String jmbgKorisnika)
        {
            return GetByJmbg(jmbgKorisnika).TrenutnoSeTretiraKao;
        }

        public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
        {
            return TerminServis.getInstance().NadjiSveTerminePacijentaIzBuducnosti(jmbgKorisnkika).Count;
        }

        internal void OdblokirajKorisnika()
        {
            List<KorisnickeAktivnostiNaAplikaciji> aktivnosti = skladisteZaKorisnickeAktivnosti.GetAll();
            foreach (KorisnickeAktivnostiNaAplikaciji aktivnost in aktivnosti)
            {
                if (aktivnost.TrenutnoSeTretiraKao == VrstaKorisnikaAplikacije.Spam &&
                    (aktivnost.BlokiranDo.Equals(DateTime.Now) || aktivnost.BlokiranDo < DateTime.Now))
                {
                    LogickiObrisiOdlaganja(aktivnost);
                    aktivnost.OdblokirajKorisnika();
                }
            }
        }

        private void LogickiObrisiOdlaganja(KorisnickeAktivnostiNaAplikaciji aktivnost)
        {
            foreach (KorisnickaAktivnost akt in aktivnost.AktivnostiKorisnika)
            {
                if (akt.VrstaAktivnosti == VrstaKorisnickeAkcije.OdlaganjePregleda)
                {
                    akt.logickiObrisan = true;
                }
            }
            IzmenaKorisnickeAktivnosti(aktivnost);
        }

        public int DobaviBrojOtkazivanjaUProteklihMesecDana(String jmbgKorisnika)
        {
            List<KorisnickaAktivnost> aktivnostiKorisnika = GetByJmbg(jmbgKorisnika).AktivnostiKorisnika;
            return IzracunajBrojOdlaganja(aktivnostiKorisnika);
        }

        private int IzracunajBrojOdlaganja(List<KorisnickaAktivnost> aktivnostiKorisnika)
        {
            int brojOdlaganja = 0;
            foreach (KorisnickaAktivnost aktivnost in aktivnostiKorisnika)
            {
                if (DateTime.Today.AddMonths(-1) < aktivnost.DatumIVreme
                    && aktivnost.VrstaAktivnosti == VrstaKorisnickeAkcije.OdlaganjePregleda
                    && !aktivnost.logickiObrisan)
                {
                    brojOdlaganja++;
                }
            }
            return brojOdlaganja;
        }

        internal string DobaviPorukuZabrane(string jmbgPacijenta)
        {
            VrstaKorisnikaAplikacije tretiraSeKao = GetRangKorisnika(jmbgPacijenta);
            String povratna = "";
            if (tretiraSeKao == VrstaKorisnikaAplikacije.HalfSpam)
            {
                povratna += "Premašili ste dozvoljeni broj zakazanih termina. Pokušajte ponovo kada prethodno zakazani budu završeni ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n" + "Molimo Vas da smanjite bespotrebna zakazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
            }
            else if (tretiraSeKao == VrstaKorisnikaAplikacije.Spam)
            {
                povratna += "Premašili ste dozvoljeni broj odlaganja termina. Pokušajte ponovo uskoro ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n" + "Molimo Vas da smanjite bespotrebna zakazivanja i otkazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
            }
            return povratna;
        }

        public KorisnickeAktivnostiNaAplikaciji NapraviNoveKorisnickeAktivnosti(String jmbgKorisnika)
        {
            KorisnickeAktivnostiNaAplikaciji noveAktivnosti = new KorisnickeAktivnostiNaAplikaciji(jmbgKorisnika);
            skladisteZaKorisnickeAktivnosti.Save(noveAktivnosti);
            return noveAktivnosti;
        }

        public Boolean DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
        {
            bool moguceZakazati = true;
            VrstaKorisnikaAplikacije rangKorisnika = GetRangKorisnika(jmbgKorisnika);
            if (rangKorisnika == VrstaKorisnikaAplikacije.HalfSpam || rangKorisnika == VrstaKorisnikaAplikacije.Spam)
                moguceZakazati = false;        
            return moguceZakazati;
        }

        public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
        {
            bool moguceOdloziti = true;
            VrstaKorisnikaAplikacije rangKorisnika = GetRangKorisnika(jmbgPacijenta);
            if (rangKorisnika == VrstaKorisnikaAplikacije.Spam)
                moguceOdloziti = false;
            return moguceOdloziti;
        }

        public void DodajZakazivanje(String jmbgPacijenta)
        {
            KorisnickaAktivnost zakazivanje = new KorisnickaAktivnost(VrstaKorisnickeAkcije.ZakazivanjePregleda, DateTime.Now);
            KorisnickeAktivnostiNaAplikaciji sveAktivnostiKorisnika = skladisteZaKorisnickeAktivnosti.GetByJmbg(jmbgPacijenta);
            sveAktivnostiKorisnika.AktivnostiKorisnika.Add(zakazivanje);
            IzmenaKorisnickeAktivnosti(sveAktivnostiKorisnika);
        }

        public void DodajOdlaganje(String jmbgPacijenta)
        {
            KorisnickaAktivnost odlaganje = new KorisnickaAktivnost(VrstaKorisnickeAkcije.OdlaganjePregleda, DateTime.Now);
            KorisnickeAktivnostiNaAplikaciji sveAktivnostiKorisnika = skladisteZaKorisnickeAktivnosti.GetByJmbg(jmbgPacijenta);
            sveAktivnostiKorisnika.AktivnostiKorisnika.Add(odlaganje);
            IzmenaKorisnickeAktivnosti(sveAktivnostiKorisnika);
        }

        public bool IzmenaKorisnickeAktivnosti(Model.KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost, string noviJmbg = null)
        {
            AzurirajRang(korisnickaAktivnost);
            List<KorisnickeAktivnostiNaAplikaciji> aktivnostiSvihKorisnika = skladisteZaKorisnickeAktivnosti.GetAll();
            for (int i = 0; i < aktivnostiSvihKorisnika.Count; i++)
            {
                if (aktivnostiSvihKorisnika[i].JmbgKorisnika.Equals(korisnickaAktivnost.JmbgKorisnika))
                {
                    if (noviJmbg != null)
                        korisnickaAktivnost.JmbgKorisnika = noviJmbg;
                    aktivnostiSvihKorisnika[i] = korisnickaAktivnost;
                    break;
                }
            }
            skladisteZaKorisnickeAktivnosti.SaveAll(aktivnostiSvihKorisnika);
            return true;
        }

        public void AzurirajRang(KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost)
        {
            if (IzracunajBrojOdlaganja(korisnickaAktivnost.AktivnostiKorisnika) > 2)
            {
                korisnickaAktivnost.BlokirajKorisnika();
            }
            else if (DobaviBrojZakazanihPregledaUBuducnosti(korisnickaAktivnost.JmbgKorisnika) > 4)
                korisnickaAktivnost.TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.HalfSpam;
            else
                korisnickaAktivnost.TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Normalan;
        }

    }
}