using Bolnica.DTOs;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servis
{
    public class PacijentServis : KorisnikServis
    {
        public static PacijentServis instance = null;
        public static PacijentServis GetInstance()
        {
            if(instance == null)
            {
                return new PacijentServis();
            }
            else
            {
                return instance;
            }
        }
        public PacijentServis()
        {
            skladistePacijenta = SkladistePacijenta.GetInstance();
        }

        public bool RegistrujPacijenta(Pacijent pacijent)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();

            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    return false;
                }
            }
            skladistePacijenta.Save(pacijent);
            return true;
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
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();
            foreach (Pacijent pacijent in pacijenti)
            {
                if (pacijent.Korisnik.KorisnickoIme.Equals(korisnickoIme) && pacijent.Korisnik.Lozinka.Equals(lozinka))
                {
                    return pacijent;
                }
            }
            return null;
        }

        public List<Recept> DobaviReceptePacijenta(string jmbg)
        {
            List<Recept> receptiPacijenta = new List<Recept>();
            Pacijent pacijent = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
            foreach(Izvestaj i in pacijent.zdravstveniKarton.izvestaj)
            {
                foreach(Recept r in i.recepti)
                {
                    receptiPacijenta.Add(r);
                }
            }
            return receptiPacijenta;
        }

        public bool ObrisiPacijenta(string jmbg)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();
            bool uspesno = false;
            foreach (Pacijent pacijent in pacijenti)
            {
                if (pacijent.Jmbg.Equals(jmbg))
                {
                    uspesno = pacijenti.Remove(pacijent);
                    skladistePacijenta.SaveAll(pacijenti);
                    break;
                }
            }
            return uspesno;
        }

        public bool IzmeniPacijenta(Pacijent stari, Pacijent novi)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();
            bool uspesno = ObrisiPacijenta(stari.Jmbg);
            if (uspesno)
            {
                uspesno = RegistrujPacijenta(novi);
            }
            return uspesno;
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public bool DaLiJePacijentSlobodan(ParamsToCheckAvailabilityOfPatientDTO parametri)
        {
            List<Termin> terminiPacijenta = SkladisteZaTermine.getInstance().getByJmbg(parametri.Id);
            foreach (Termin termin in terminiPacijenta)
            {
                if (DaLiPocetakIliKrajTerminaUpadaju(parametri, termin)) return false;

                if (DaLiJeZakazaniTerminUnutarNovogTermina(parametri, termin)) return false;

                if (DaLiSuTerminiJednaki(parametri, termin)) return false;
            }
            return true;
        }

        private static bool DaLiPocetakIliKrajTerminaUpadaju(ParamsToCheckAvailabilityOfPatientDTO parametri, Termin termin)
        {
            if (parametri.startTime >= termin.DatumIVremeTermina && parametri.startTime <=(termin.DatumIVremeTermina.AddMinutes(termin.TrajanjeTermina)) //da li pocetak upada
                && (parametri.startTime.AddMinutes(parametri.durationInMinutes)) > termin.DatumIVremeTermina &&
                (parametri.startTime.AddMinutes(parametri.durationInMinutes)) < (termin.DatumIVremeTermina.AddMinutes(termin.TrajanjeTermina))) //da li kraj upada
            {
                return true;
            }

            return false;
        }

        private static bool DaLiJeZakazaniTerminUnutarNovogTermina(ParamsToCheckAvailabilityOfPatientDTO parametri,
            Termin termin)
        {
            if (termin.DatumIVremeTermina > parametri.startTime && (termin.DatumIVremeTermina.AddMinutes(termin.TrajanjeTermina)) <
                (parametri.startTime.AddMinutes(parametri.durationInMinutes))) 
            {
                return true;
            }

            return false;
        }

        private static bool DaLiSuTerminiJednaki(ParamsToCheckAvailabilityOfPatientDTO parametri, Termin termin)
        {
            if (parametri.startTime.Equals(termin.DatumIVremeTermina))
            {
                return true;
            }
            return false;
        }

        public Pacijent GetByJmbg(string jmbg)
        {
            return skladistePacijenta.getByJmbg(jmbg);
        }

        public List<Pacijent> GetAll()
        {
            return skladistePacijenta.GetAll();
        }

        public void Save(Model.Pacijent pacijent)
        {
            skladistePacijenta.Save(pacijent);
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            // TODO: implement
        }

        public SkladistePacijenta skladistePacijenta;

    }
}