using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class PacijentKontroler : KorisnikKontroler
    {
        public static PacijentKontroler instance = null;

        public static PacijentKontroler getInstance()
        {
            if (instance == null)
            {
                return new PacijentKontroler();
            }
            else
            {
                return instance;
            }
        }
        public PacijentKontroler()
        {
            pacijentServis = new PacijentServis();
            instance = this;
        }

        public bool RegistrujPacijenta(Pacijent pacijent)
        {
            return pacijentServis.RegistrujPacijenta(pacijent);
        }

        public bool DodajAlergen(String alergen)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public bool DodajIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return pacijentServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Pacijent> GetAll()
        {
            return pacijentServis.GetAll();
        }

        public void Save(Pacijent pacijent)
        {
            // TODO: implement
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            // TODO: implement
        }

        public bool izmeniPacijenta(Pacijent stari, Pacijent novi)
        {
            return pacijentServis.izmeniPacijenta(stari, novi);
        }

        public PacijentServis pacijentServis;

        internal bool obrisiPacijentaNaIndeksu(int selectedIndex)
        {
            return pacijentServis.obrisiPacijentaNaIndeksu(selectedIndex);
        }
    }
}