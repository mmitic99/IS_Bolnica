using Bolnica.DTOs;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;

namespace Servis
{
    public class LekarServis : KorisnikServis
    {
        public static LekarServis instance = null;

        public static LekarServis getInstance()
        {
            if(instance == null)
            {
                return new LekarServis();
            }
            else
            {
                return instance;
            }
        }

        public LekarServis()
        {
            skladisteZaLekara = SkladisteZaLekara.GetInstance();
            instance = this;
        }

        public bool RegistrujLekara(Lekar lekar)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();

            Lekar lekar = new Lekar();

            foreach (Lekar lekar1 in lekari)
            {
                if (lekar1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    lekar = lekar1;
                    if (lekar1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return lekar;
                    }
                }
            }
            return null;
        }

        public bool DaLiJeLekarSlobodan(ParamsToCheckAvailabilityOfDoctorDTO parametri)
        {
            bool slobodan = true;
            List<Termin> terminiLekara = SkladisteZaTermine.getInstance().getByJmbgLekar(parametri.IDDoctor);
            foreach(Termin t in terminiLekara)
            {
                if(parametri.startTime > t.DatumIVremeTermina && parametri.startTime<(t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) //da li pocetak upada
                    && (parametri.startTime.AddMinutes(parametri.durationInMinutes))>t.DatumIVremeTermina && (parametri.startTime.AddMinutes(parametri.durationInMinutes))<(t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina))) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if(t.DatumIVremeTermina> parametri.startTime && (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < (parametri.startTime.AddMinutes(parametri.durationInMinutes))) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
                if(t.DatumIVremeTermina.Equals(parametri.startTime))
                {
                    slobodan = false;
                    break;
                }
            }
            return slobodan;
        }

        internal int DobaviIndeksSelectovanogLekara(Termin termin)
        {
            List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari[i].Jmbg.Equals(termin.JmbgLekara))
                {
                    return i;
                }
            }
            return 0;        
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
            return SkladisteZaLekara.GetInstance().GetAll();
        }

        public void Save(Lekar lekar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }
        public Lekar GetByJmbg(string jmbg)
        {
            return skladisteZaLekara.getByJmbg(jmbg);
        }

        public SkladisteZaLekara skladisteZaLekara;
    }
}