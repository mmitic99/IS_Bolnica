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
                SkladisteZaKorisnickeAktivnosti.GetInstance().Save(aktivnosti);
            }
            return aktivnosti;
        }
      
      public Model.Enum.VrstaKorisnikaAplikacije GetRangKorisnika(String jmbgKorisnika)
      {

            return GetByJmbg(jmbgKorisnika).TrenutnoSeTretiraKao;
      }
      
      public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
      {
            List<Termin> sviTerminiKorisnika = SkladisteZaTermine.getInstance().getByJmbg(jmbgKorisnkika); //vraca samo za pazijenta
            List<Termin> sviTerminiKorisnikaIzBuducnosti = new List<Termin>();
            foreach(Termin t in sviTerminiKorisnika)
            {
                if(t.DatumIVremeTermina>DateTime.Now)
                {
                    sviTerminiKorisnikaIzBuducnosti.Add(t);
                }
            }

         return sviTerminiKorisnikaIzBuducnosti.Count;
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
                    && aktivnost.VrstaAktivnosti == VrstaKorisnickeAkcije.OdlaganjePregleda)
                {
                    brojOdlaganja++;
                }
            }
            return brojOdlaganja;
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
            AzurirajRang(sveAktivnostiKorisnika);
            IzmenaKorisnickeAktivnosti(sveAktivnostiKorisnika);
      }
      
      public bool IzmenaKorisnickeAktivnosti(Model.KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost)
      {
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
                korisnickaAktivnost.TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Spam;
            else if (DobaviBrojZakazanihPregledaUBuducnosti(korisnickaAktivnost.JmbgKorisnika) > 4)
                korisnickaAktivnost.TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.HalfSpam;
            else
                korisnickaAktivnost.TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Normalan;
      }
      

   
      public Repozitorijum.SkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;
   
   }
}