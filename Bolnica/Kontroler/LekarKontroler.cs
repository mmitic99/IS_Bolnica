using Bolnica.view;
using Bolnica.view.PacijentView;
using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class LekarKontroler : KorisnikKontroler
    {
        public static LekarKontroler instance = null;

        public static LekarKontroler getInstance()
        {
            if(instance==null)
            {
                return new LekarKontroler();
            }
            else
            {
                return instance;
            }
        }
        public LekarKontroler()
        {
            instance = this;
            lekarServis = new LekarServis();
        }

        public bool RegistrujLekara(Lekar lekar)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return lekarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Lekar> GetAll()
        {
            return LekarServis.getInstance().GetAll();
        }

        public void Save(Model.Lekar lekar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }

        public Servis.LekarServis lekarServis;

        internal int DobaviIndeksSelektovanogLekara(Object termin)
        {
            return LekarServis.getInstance().DobaviIndeksSelectovanogLekara((Termin)termin);
        }
        public void izdajRecept(string imeLeka, string sifraLeka, string dodatneNapomene, DateTime datumIzdavanja, int brojDana, int doza, List<int> terminiUzimanjaLeka, string dijagnoza, string imeDoktora, Pacijent p, Pacijent p1)
        {
            Recept recept = new Recept(imeLeka, sifraLeka, dodatneNapomene, datumIzdavanja, brojDana, doza, terminiUzimanjaLeka, dijagnoza
                  , LekarWindow.getInstance().lekar1.FullName);
            recept.DatumIzdavanja = DateTime.Today;
            List<Recept> recepti = new List<Recept>();
            recepti.Add(recept);
            Izvestaj izvestaj = new Izvestaj(recepti);
            List<Izvestaj> izvestaji = new List<Izvestaj>();
            izvestaji.Add(izvestaj);
            p1.zdravstveniKarton.izvestaj.Add(izvestaj);
            PacijentKontroler.getInstance().izmeniPacijenta(p, p1);
        }
    }
}