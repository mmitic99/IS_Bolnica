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

        public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajPotrosnuOpremuUMagacin(tipOpreme, kolicina);
        }

        public void DodajStacionarnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajStacionarnuOpremuUMagacin(tipOpreme, kolicina);
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiStacionarnuOpremuIzMagacina(index);
        }
        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiPotrosnuOpremuIzMagacina(index);
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniStacionarnuOpremuUMagacinu(index, kolicina);
        }

        public void IzmeniDinamickuOpremuUMagacinu(int index, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniDinamickuOpremuUMagacinu(index, kolicina);
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

        public bool ProveriValidnostOpreme(String NazivOpreme, String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostOpreme(NazivOpreme, Kolicina);
        }

        public bool ProveriValidnostKolicineOpreme(String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpreme(Kolicina);
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpremePriPrebacivanju(Kolicina);
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            return ProstorijeServis.GetInstance().GetVrstuProstorije(IndexSelektovaneVrsteProstorije);
        }

        public List<StacionarnaOprema> GetStacionarnaOpremaProstorije(int index) 
        {
            return ProstorijeServis.GetInstance().GetStacionarnaOpremaProstorije(index);
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            ProstorijeServis.GetInstance().IzbrisiStacionarnuOpremuIzProstorije(indexProstorije, indexOpreme); 
        }

        public void IzmeniStacionarnuOpremuProstorije(int indexProstorije, int indexOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniStacionarnuOpremuProstorije(indexProstorije, indexOpreme, kolicina);
        }

        public bool ProveriValidnostPrebacivanjaOpreme(String kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpreme(kolicina);
        }
        public void PrebaciStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().PrebaciStacionarnuOpremuUProstoriju(indexIzKojeProstorije, indexUKojuProstoriju, nazivOpreme, kolicina);
        }

        public Servis.TerminServis terminServis;
        public Servis.ProstorijeServis prostorijeServis;

    }
}