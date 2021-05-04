/***********************************************************************
 * Module:  KorisnickeAktivnostiPacijentaServis.cs
 * Author:  PC
 * Purpose: Definition of the Class Servis.KorisnickeAktivnostiPacijentaServis
 ***********************************************************************/

using System;
using System.Collections.Generic;
using Model;
using Model.Enum;
using Repozitorijum;

namespace Servis
{
   public class KorisnickeAktivnostiPacijentaServis
   {
        private static KorisnickeAktivnostiPacijentaServis instance = null;

        public static KorisnickeAktivnostiPacijentaServis GetInstance()
        {
            if (instance == null)
            {
                return new KorisnickeAktivnostiPacijentaServis();
            }
            else
                return instance;
        }

        public KorisnickeAktivnostiPacijentaServis()
        {
            instance = this;
        }

        public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
      {
            KorisnickeAktivnostiNaAplikaciji aktivnosti = SkladisteZaKorisnickeAktivnosti.GetInstance().GetByJmbg(jmbgKorisnika);
            if (aktivnosti == null)
            {
                aktivnosti = NapraviNoveKorisnickeAktivnosti(jmbgKorisnika);
               // SkladisteZaKorisnickeAktivnosti.GetInstance().Save(aktivnosti);
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

        private object NadjiSveTerminePacijentaIzBuducnosti(string jmbgKorisnkika)
        {
            throw new NotImplementedException();
        }

        internal void OdblokirajKorisnika()
        {
            List<KorisnickeAktivnostiNaAplikaciji> aktivnosti = SkladisteZaKorisnickeAktivnosti.GetInstance().GetAll();
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
            foreach(KorisnickaAktivnost akt in aktivnost.AktivnostiKorisnika)
            {
                if(akt.VrstaAktivnosti == VrstaKorisnickeAkcije.OdlaganjePregleda)
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
            if(tretiraSeKao == VrstaKorisnikaAplikacije.HalfSpam)
            {
                povratna+="Premašili ste dozvoljeni broj zakazanih termina. Pokušajte ponovo kada prethodno zakazani budu završeni ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n"+"Molimo Vas da smanjite bespotrebna zakazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
            }
            else if(tretiraSeKao == VrstaKorisnikaAplikacije.Spam)
            {
                povratna += "Premašili ste dozvoljeni broj odlaganja termina. Pokušajte ponovo uskoro ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n" + "Molimo Vas da smanjite bespotrebna zakazivanja i otkazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
            }
            return povratna;
        }

        public KorisnickeAktivnostiNaAplikaciji NapraviNoveKorisnickeAktivnosti(String jmbgKorisnika)
      {
            KorisnickeAktivnostiNaAplikaciji noveAktivnosti = new KorisnickeAktivnostiNaAplikaciji(jmbgKorisnika);
            SkladisteZaKorisnickeAktivnosti.GetInstance().Save(noveAktivnosti);
            return noveAktivnosti;
        }

        public Boolean DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
        {
            VrstaKorisnikaAplikacije rangKorisnika = GetRangKorisnika(jmbgKorisnika);
            if (rangKorisnika == VrstaKorisnikaAplikacije.HalfSpam || rangKorisnika == VrstaKorisnikaAplikacije.Spam) 
                return false;
            else 
                return true;
      }

        public Boolean DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
        {
            VrstaKorisnikaAplikacije rangKorisnika = GetRangKorisnika(jmbgPacijenta);
            if (rangKorisnika == VrstaKorisnikaAplikacije.Spam)
                return false;
            else
                return true;
        }

        public void DodajZakazivanje(String jmbgPacijenta)
      {
            KorisnickaAktivnost zakazivanje = new KorisnickaAktivnost(VrstaKorisnickeAkcije.ZakazivanjePregleda, DateTime.Now);
            KorisnickeAktivnostiNaAplikaciji sveAktivnostiKorisnika = SkladisteZaKorisnickeAktivnosti.GetInstance().GetByJmbg(jmbgPacijenta);
            sveAktivnostiKorisnika.AktivnostiKorisnika.Add(zakazivanje);
            IzmenaKorisnickeAktivnosti(sveAktivnostiKorisnika);
      }
      
      public void DodajOdlaganje(String jmbgPacijenta)
      {
            KorisnickaAktivnost odlaganje = new KorisnickaAktivnost(VrstaKorisnickeAkcije.OdlaganjePregleda, DateTime.Now);
            KorisnickeAktivnostiNaAplikaciji sveAktivnostiKorisnika = SkladisteZaKorisnickeAktivnosti.GetInstance().GetByJmbg(jmbgPacijenta);
            sveAktivnostiKorisnika.AktivnostiKorisnika.Add(odlaganje);
            IzmenaKorisnickeAktivnosti(sveAktivnostiKorisnika);
      }
      
      public bool IzmenaKorisnickeAktivnosti(Model.KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost)
      {
            AzurirajRang(korisnickaAktivnost);
            List<KorisnickeAktivnostiNaAplikaciji> aktivnostiSvihKorisnika =  SkladisteZaKorisnickeAktivnosti.GetInstance().GetAll();
            for(int i=0; i<aktivnostiSvihKorisnika.Count; i++)
            {
                if(aktivnostiSvihKorisnika[i].JmbgKorisnika.Equals(korisnickaAktivnost.JmbgKorisnika))
                {
                    aktivnostiSvihKorisnika[i] = korisnickaAktivnost;
                    break;
                }
            }
            SkladisteZaKorisnickeAktivnosti.GetInstance().SaveAll(aktivnostiSvihKorisnika);
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
      

   
      public Repozitorijum.SkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;
   
   }
}