using Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis.StatePattern
{
    class SpamStanje : IStanjeKorisnika
    {
        private KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis;

        public SpamStanje(KorisnickeAktivnostiPacijentaServis korisnickeAktivnostiPacijentaServis)
        {
            this.korisnickeAktivnostiPacijentaServis = korisnickeAktivnostiPacijentaServis;
        }

        public bool BlokirajKorisnika(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public bool DaLiMozeDaPomeri(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public bool DaLiMozeDaZakaze(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public int DobaviBrojOtkazivanjeUProteklihMesecDana(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public string DobaviPorukuZabrane(string JmbgKorisnik)
        {
            throw new NotImplementedException();
        }

        public bool DodajPomeranje(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public bool DodajZakazivanje(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }

        public bool OdblokirajKorisnika(string JmbgKorisnika)
        {
            throw new NotImplementedException();
        }
    }
}
