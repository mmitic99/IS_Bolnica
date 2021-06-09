using Model;
using Model.Enum;
using Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis.StatePattern
{
    public class SpamStanje : IStanjeKorisnika
    {
        private KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis;
        public const int MAX_BROJ_ZAKAZANIH_PACIjENTA = 5;
        public const int MAX_BROJ_OTKAZIVANJA = 3;
        public SpamStanje(KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis)
        {
            this.korisnickeAktivnostiPacijentaServis = korisnickeAktivnostiPacijentaServis;
        }

        public bool BlokirajKorisnika()
        {
            return false;
        }

        public bool DaLiMozeDaPomeri()
        {
            return false;
        }

        public bool DaLiMozeDaZakaze()
        {
            return false;
        }

        public int DobaviBrojOtkazivanjeUProteklihMesecDana()
        {
            int brojOdlaganja = 0;
            foreach (KorisnickaAktivnost aktivnost in korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika)
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

        public string DobaviPorukuZabrane()
        {
            return "Premašili ste dozvoljeni broj odlaganja termina. Pokušajte ponovo uskoro ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n" + "Molimo Vas da smanjite bespotrebna zakazivanja i otkazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
        }

        public bool DodajPomeranje()
        {
            return false;
        }

        public bool DodajZakazivanje()
        {
            return false;
        }

        public bool OdblokirajKorisnika()
        {
            bool korisnikOdblokiran = false;
            if (korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.BlokiranDo <= DateTime.Now)
            {
                logickiObrisiOdlaganja();
                korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.BlokiranDo = DateTime.MinValue;
                korisnickeAktivnostiPacijentaServis.SacuvajIzmenjenekorisnickeAktivnosti();
                if (korisnickeAktivnostiPacijentaServis.TerminServis.DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.JmbgKorisnika) >= MAX_BROJ_ZAKAZANIH_PACIjENTA)
                    korisnickeAktivnostiPacijentaServis.TrenutnoStanjeKorisnika = korisnickeAktivnostiPacijentaServis.HalfSpam;
                else korisnickeAktivnostiPacijentaServis.TrenutnoStanjeKorisnika = korisnickeAktivnostiPacijentaServis.NonSpam;
                korisnikOdblokiran = true;
            }
            return korisnikOdblokiran;
        }

        private void logickiObrisiOdlaganja()
        {
            foreach (KorisnickaAktivnost akt in korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika)
            {
                if (akt.VrstaAktivnosti == VrstaKorisnickeAkcije.OdlaganjePregleda)
                {
                    akt.logickiObrisan = true;
                }
            }
        }

        public bool DaLiJePredZabranuOtkazivanja()
        {
            return false;
        }

        public bool DaLiJePredZabranuZakazivanja()
        {
            return false;
        }
    }
}
