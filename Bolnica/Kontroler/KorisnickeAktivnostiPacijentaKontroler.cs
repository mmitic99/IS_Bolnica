
using Servis;
using System;

namespace Kontroler
{
    public class KorisnickeAktivnostiPacijentaKontroler
    {
        private KorisnickeAktivnostiPacijentaServis KorisnickeAktivnostiPacijentaServis;
        private static KorisnickeAktivnostiPacijentaKontroler instance;

        public static KorisnickeAktivnostiPacijentaKontroler GetInstane()
        {
            if(instance==null)
            {
                instance = new KorisnickeAktivnostiPacijentaKontroler();
            }
            return instance;
        }

        public KorisnickeAktivnostiPacijentaKontroler()
        {
            this.KorisnickeAktivnostiPacijentaServis = new KorisnickeAktivnostiPacijentaServis();
        }

        public int DobaviBrojOtkazivanjaUProteklihMesecDana(String jmbgKorisnika)
        {
            return KorisnickeAktivnostiPacijentaServis.DobaviBrojOtkazivanjaUProteklihMesecDana(jmbgKorisnika);
        }

        public int DobaviBrojZakazanihPregledaUBuducnosti(String jmbgKorisnkika)
        {
            return KorisnickeAktivnostiPacijentaServis.DobaviBrojZakazanihPregledaUBuducnosti(jmbgKorisnkika);
        }

        public void OdblokirajKorisnike()
        {
            KorisnickeAktivnostiPacijentaServis.OdblokirajKorisnika();
        }

        public void DodajZakazivanje(string jmbg)
        {
            KorisnickeAktivnostiPacijentaServis.DodajZakazivanje(jmbg);
        }

        public void DodajOdlaganje(string jmbgPacijenta)
        {
            KorisnickeAktivnostiPacijentaServis.DodajOdlaganje(jmbgPacijenta);
        }

        public bool DaLiJeMoguceOdlozitiZakazaniTermin(String jmbgPacijenta)
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiJeMoguceOdlozitiZakazaniTermin(jmbgPacijenta);
        }

        public bool DaLiJeMoguceZakazatiNoviTermin(String jmbgKorisnika)
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiJeMoguceZakazatiNoviTermin(jmbgKorisnika);
        }

        public string DobaviPorukuZabrane(string jmbgPacijenta)
        {
            return KorisnickeAktivnostiPacijentaServis.DobaviPorukuZabrane(jmbgPacijenta);
        }
    }
}