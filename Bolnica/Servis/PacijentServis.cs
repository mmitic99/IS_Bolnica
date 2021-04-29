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
        public static PacijentServis getInstance()
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
            bool uspesno = true;

            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    uspesno = false;
                    return uspesno;
                }
            }

            if (uspesno)
            {
                skladistePacijenta.Save(pacijent);
            }

            return uspesno;
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

            Pacijent pacijent = new Pacijent();

            foreach (Pacijent pacijent1 in pacijenti)
            {
                if (pacijent1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    pacijent = pacijent1;
                    if (pacijent1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return pacijent;
                    }
                }
            }
            return null;
        }

        public List<Recept> dobaviReceptePacijenta(string jmbg)
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

        internal bool izmeniPacijenta(Pacijent stari, Pacijent novi)
        {
            bool uspesno = true;
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();

            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(stari.Jmbg))
                {
                    pacijenti.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(novi.Jmbg))
                {
                    uspesno = false;
                    return uspesno;
                }
            }

            if (uspesno)
            {
                pacijenti.Add(novi);
                skladistePacijenta.SaveAll(pacijenti);
            }

            return uspesno;
        }

        public bool obrisiPacijentaNaIndeksu(int selectedIndex)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();
            pacijenti.RemoveAt(selectedIndex);
            skladistePacijenta.SaveAll(pacijenti);
            return true;
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
            bool slobodan = true;
            List<Termin> terminiPacijenta = SkladisteZaTermine.getInstance().getByJmbg(parametri.Id);
            foreach (Termin t in terminiPacijenta)
            {
                if (parametri.startTime >= t.DatumIVremeTermina && parametri.startTime <= (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) //da li pocetak upada
                    && (parametri.startTime.AddMinutes(parametri.durationInMinutes)) > t.DatumIVremeTermina && (parametri.startTime.AddMinutes(parametri.durationInMinutes)) < (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina))) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if (t.DatumIVremeTermina > parametri.startTime && (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < (parametri.startTime.AddMinutes(parametri.durationInMinutes))) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
                if (parametri.startTime.Equals(t.DatumIVremeTermina))
                {
                    slobodan = false;
                    break;
                }
            }
            return slobodan;
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