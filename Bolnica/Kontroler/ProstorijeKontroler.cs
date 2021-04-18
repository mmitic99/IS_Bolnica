using Model;
using Repozitorijum;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class ProstorijeKontroler
    {

        private static ProstorijeKontroler instance = null;

        public static ProstorijeKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new ProstorijeKontroler();
            }
            return instance;
        }

        public ProstorijeKontroler()
        {
            prostorijeServis = new ProstorijeServis();
        }

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

        public void IzmeniKolicinuPotrosneOpreme(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniKolicinuPotrosneOpreme(tipOpreme, kolicina);
        }

        public void IzmeniKolicinuStacionarneOpreme(int idProstorije, String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniKolicinuStacionarneOpreme(idProstorije, tipOpreme, kolicina);
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
            return prostorijeServis.GetAll();
        }

        public void Save(Prostorija prostorija)
        {
            // TODO: implement
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            // TODO: implement
        }

        public void DodajStacionarnuOpremu(int idProstorije, String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajStacionarnuOpremu(idProstorije, tipOpreme, kolicina);
        }

        public void DodajPotrosnuOpremu(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajPotrosnuOpremu(tipOpreme, kolicina);
        }

        public void IzbrisiStacionarnuOpremu(int idProstorije, int index)
        {
            ProstorijeServis.GetInstance().IzbrisiStacionarnuOpremu(idProstorije, index);
        }

        public void IzbrisiPotrosnuOpremu(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiPotrosnuOpremu(index);
        }

        public Prostorija GetMagacin()
        {
            return ProstorijeServis.GetInstance().GetMagacin();
        }

        public void DodajProstoriju(Prostorija p)
        {
            ProstorijeServis.GetInstance().DodajProstoriju(p);
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            ProstorijeServis.GetInstance().IzmeniProstoriju(index, p);
        }

        public void IzbrisiProstoriju(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiProstoriju(index);
        }

        public bool ProveriValidnostProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadaratura)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostProstorije(BrojProstorije, Sprat, IndexSelektovaneVrsteProstorije, Kvadaratura);
        }

        public bool ProveriValidnostIzmeneProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadaratura)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostIzmeneProstorije(BrojProstorije, Sprat, IndexSelektovaneVrsteProstorije, Kvadaratura);
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            return ProstorijeServis.GetInstance().GetVrstuProstorije(IndexSelektovaneVrsteProstorije);
        }

        public Servis.TerminServis terminServis;
        public Servis.ProstorijeServis prostorijeServis;

    }
}