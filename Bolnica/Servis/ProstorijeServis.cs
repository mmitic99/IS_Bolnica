using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;

namespace Servis
{
    public class ProstorijeServis
    {
        public void RenovirajProstoriju()
        {
            // TODO: implement
        }

        public void ZavrsiRenoviranje()
        {
            // TODO: implement
        }

        public void IzmeniKolicinuLeka(int idLeka, double kolicina, int izProstorije, int uProstoriju)
        {
            // TODO: implement
        }

        public void IzmeniKolicinuPotrosneOpreme(String tipOpreme, int kolicina, int izProstorije, int uProstoriju)
        {
            // TODO: implement
        }

        public void IzmeniKolicinuStacionarneOpreme(String tipOpreme, int kolicina, int izProstorije, int uProstoriju)
        {
            // TODO: implement
        }

        public int DobaviProstoriju(DateTime pocetakTermina, DateTime krajTermina, Model.Enum.VrstaProstorije vrstaProstorije)
        {
            // TODO: implement
            return 0;
        }

        public List<Termin> PrikaziTermine(int idProstorije)
        {
            // TODO: implement
            return null;
        }

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime pocetakTermina, DateTime krajTermina)
        {
            // TODO: implement
            return false;
        }

        public List<Prostorija> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Prostorija prostorija)
        {
            // TODO: implement
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            // TODO: implement
        }

        public SkladisteZaProstorije skladisteZaProstorije;
        public SkladisteZaTermine skladisteZaTermine;

    }
}