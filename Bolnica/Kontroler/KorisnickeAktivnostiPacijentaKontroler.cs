
using Servis;
using System;

namespace Kontroler
{
    public class KorisnickeAktivnostiPacijentaKontroler
    {
        private KorisnickeAktivnostiPacijentaServis KorisnickeAktivnostiPacijentaServis;
        public KorisnickeAktivnostiPacijentaKontroler(String JmbgKorisnika)
        {
            this.KorisnickeAktivnostiPacijentaServis = new KorisnickeAktivnostiPacijentaServis(JmbgKorisnika);
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