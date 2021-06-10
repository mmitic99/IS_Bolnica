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
    public class NonSpamStanje : IStanjeKorisnika
    {
        public const int MAX_BROJ_ZAKAZANIH_PACIjENTA = 5;
        public const int MAX_BROJ_OTKAZIVANJA = 3;
        private KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis;
        public NonSpamStanje(KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis)
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
            return true;
        }

        public int DobaviBrojOtkazivanjeUProteklihMesecDana()
        {
            int brojOdlaganja = 0;
            foreach(KorisnickaAktivnost aktivnost in korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika)
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
            return "Trenutno nemate zabrane na aplikaciji!";
        }

        public bool DodajPomeranje()
        {
            korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika.Add(new KorisnickaAktivnost(VrstaKorisnickeAkcije.OdlaganjePregleda, DateTime.Now));
            korisnickeAktivnostiPacijentaServis.SacuvajIzmenjenekorisnickeAktivnosti();
            if(DobaviBrojOtkazivanjeUProteklihMesecDana() >= MAX_BROJ_OTKAZIVANJA)
            {
                BlokirajKorisnika();
            }
            return true;
        }

        public bool DodajZakazivanje()
        {
            korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.AktivnostiKorisnika.Add(new KorisnickaAktivnost(VrstaKorisnickeAkcije.ZakazivanjePregleda, DateTime.Now));
            korisnickeAktivnostiPacijentaServis.SacuvajIzmenjenekorisnickeAktivnosti();
            if(korisnickeAktivnostiPacijentaServis.TerminServis.DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.JmbgKorisnika) >= MAX_BROJ_ZAKAZANIH_PACIjENTA)
            {
                korisnickeAktivnostiPacijentaServis.TrenutnoStanjeKorisnika = new HalfSpamStanje(korisnickeAktivnostiPacijentaServis);
            }
            return true;
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
            bool predZabranu = false;
            if (korisnickeAktivnostiPacijentaServis.TerminServis.DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(korisnickeAktivnostiPacijentaServis.korisnickeAktivnostiNaAplikaciji.JmbgKorisnika) >= MAX_BROJ_ZAKAZANIH_PACIjENTA - 1) predZabranu = true;
            return predZabranu;
        }
    }
}
