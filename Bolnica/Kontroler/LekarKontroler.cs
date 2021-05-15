using Bolnica.DTO;
using Bolnica.view;
using Bolnica.view.PacijentView;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using static Bolnica.DTO.ReceptDTO;

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
            return lekarServis.RegistrujLekara(lekar);
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
        public void izdajRecept(ReceptiDTO parametri)
        {
            LekarServis.getInstance().izdajRecept(parametri);
        }
        public Lekar GetByJmbg(string jmbg)
        {
            return lekarServis.GetByJmbg(jmbg);
        }
        public List<int> dobijTerminePijenja(String terminiPijenja)
        {
            return LekarServis.getInstance().dobijTerminePijenja(terminiPijenja);
        }

        public bool ObrisiLekara(string jmbg)
        {
            return lekarServis.ObrisiLekara(jmbg);
        }

        public bool IzmeniLekara(string jmbgLekara, Lekar lekar)
        {
            lekarServis.IzmeniLekara(jmbgLekara, lekar);
            return true;
        }
    }
}