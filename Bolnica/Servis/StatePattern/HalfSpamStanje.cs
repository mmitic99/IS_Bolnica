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
    class HalfSpamStanje : IStanjeKorisnika
    {
        private KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis;
        public const int MAX_BROJ_ZAKAZANIH_PACIjENTA = 5;
        public const int MAX_BROJ_OTKAZIVANJA = 3;
        public HalfSpamStanje(KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis)
        {
            this.korisnickeAktivnostiPacijentaServis = korisnickeAktivnostiPacijentaServis;
        }
        public bool BlokirajKorisnika()
        {
            korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.BrojPutaBlokiranja++;
            korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.BlokiranDo = DateTime.Today.AddDays(korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.BrojPutaBlokiranja * 14);
            korisnickeAktivnostiPacijentaServis.SacuvajIzmenjenekorisnickeAktivnosti();
            korisnickeAktivnostiPacijentaServis.TrenutnoStanjeKorisnika = new SpamStanje(korisnickeAktivnostiPacijentaServis);
            return true;
        }

        public bool DaLiMozeDaPomeri()
        {
            return true;
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
            return "Premašili ste dozvoljeni broj zakazanih termina. Pokušajte ponovo kada prethodno zakazani budu završeni ili nas kontaktirajte putem telefona +381218381071781."
                    + "\r\n" + "Molimo Vas da smanjite bespotrebna zakazivanja kako bi zajedno doprineli boljem iskorišćenju radnog vremena naših zaposlenih.";
        }

        public bool DodajPomeranje()
        {
           korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika.Add(new KorisnickaAktivnost(VrstaKorisnickeAkcije.OdlaganjePregleda, DateTime.Now));
            korisnickeAktivnostiPacijentaServis.SacuvajIzmenjenekorisnickeAktivnosti();
            if (DobaviBrojOtkazivanjeUProteklihMesecDana() >= MAX_BROJ_OTKAZIVANJA)
            {
                BlokirajKorisnika();
            }
            return true;
        }

        public bool DodajZakazivanje()
        {
            return false;
        }

        public bool OdblokirajKorisnika()
        {
            return false;
        }

        public bool DaLiJePredZabranuOtkazivanja()
        {
            bool predZabranu = false;
            if (DobaviBrojOtkazivanjeUProteklihMesecDana() >= MAX_BROJ_OTKAZIVANJA - 1) predZabranu = true;
            return predZabranu;
        }

        public bool DaLiJePredZabranuZakazivanja()
        {
            return false;
        }
    }
}
