
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

        public void OdblokirajKorisnika()
        {
            KorisnickeAktivnostiPacijentaServis.OdblokirajKorisnika();
        }

        public void DodajZakazivanje()
        {
            KorisnickeAktivnostiPacijentaServis.DodajZakazivanje();
        }

        public void DodajOdlaganje()
        {
            KorisnickeAktivnostiPacijentaServis.DodajPomeranje();
        }

        public bool DaLiJeMoguceOdlozitiZakazaniTermin()
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiMozeDaPomeri();
        }

        public bool DaLiJeMoguceZakazatiNoviTermin()
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiMozeDaZakaze();
        }

        public string DobaviPorukuZabrane()
        {
            return KorisnickeAktivnostiPacijentaServis.DobaviPorukuZabrane();
        }

        public bool DaLiJePredZabranuZakazivanja()
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiJePredZabranuZakazivanja();
        }

        public bool PredZabranuOtkazivanja()
        {
            return KorisnickeAktivnostiPacijentaServis.DaLiJePredZabranuOtkazivanja();
        }
    }
}