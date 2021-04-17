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

        public bool DaLiJePacijentSlobodan(string jmbgPacijenta, DateTime datumVreme, int trajanjeMinute=30)
        {
            bool slobodan = true;
            List<Termin> terminiPacijenta = SkladisteZaTermine.getInstance().getByJmbg(jmbgPacijenta);
            foreach (Termin t in terminiPacijenta)
            {
                if (datumVreme > t.DatumIVremeTermina && datumVreme < (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) //da li pocetak upada
                    && (datumVreme.AddMinutes(trajanjeMinute)) > t.DatumIVremeTermina && (datumVreme.AddMinutes(trajanjeMinute)) < (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina))) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if (t.DatumIVremeTermina > datumVreme && (t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < (datumVreme.AddMinutes(trajanjeMinute))) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
            }
            return slobodan;
        }

        public List<Pacijent> GetAll()
        {
            return skladistePacijenta.GetAll();
        }

        public void Save(Model.Pacijent pacijent)
        {
            // TODO: implement
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            // TODO: implement
        }

        public SkladistePacijenta skladistePacijenta;

    }
}