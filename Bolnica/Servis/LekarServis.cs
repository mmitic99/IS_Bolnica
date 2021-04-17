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

        public bool DaLiJeLekarSlobodan(String JmbgLekara, DateTime datumVreme, int trajanjeMinute)
        {
            bool slobodan = true;
            List<Termin> terminiLekara = SkladisteZaTermine.getInstance().getByJmbgLekar(JmbgLekara);
            foreach(Termin t in terminiLekara)
            {
                if(datumVreme> t.DatumIVremeTermina && datumVreme<(t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) //da li pocetak upada
                    && (datumVreme.AddMinutes(trajanjeMinute))>t.DatumIVremeTermina && (datumVreme.AddMinutes(trajanjeMinute))<(t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina))) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if(t.DatumIVremeTermina> datumVreme && (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < (datumVreme.AddMinutes(trajanjeMinute))) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
            }
            return slobodan;
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

        public SkladisteZaLekara skladisteZaLekara;
    }
}