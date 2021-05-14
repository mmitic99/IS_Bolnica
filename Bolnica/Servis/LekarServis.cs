using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Kontroler;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using static Bolnica.DTO.ReceptDTO;

namespace Servis
{
    public class LekarServis : KorisnikServis
    {
        public static LekarServis instance = null;

        public static LekarServis getInstance()
        {
            if (instance == null)
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
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            bool uspesno = true;

            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari.ElementAt(i).Jmbg.Equals(lekar.Jmbg))
                {
                    uspesno = false;
                    return uspesno;
                }
            }

            if (uspesno)
            {
                skladisteZaLekara.Save(lekar);
            }

            return uspesno;
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

            bool slobodan = false;

            foreach (RadnoVreme radnoVreme in radnoVremeServis.GetByJmbgAkoRadi(parametri.IDDoctor))
            {
                if (parametri.startTime.Date >= radnoVreme.DatumIVremePocetka.Date &&
                    parametri.startTime.AddMinutes(parametri.durationInMinutes).Date <=
                    radnoVreme.DatumIVremeZavrsetka.Date &&
                    parametri.startTime.TimeOfDay >= radnoVreme.DatumIVremePocetka.TimeOfDay &&
                    parametri.startTime.AddMinutes(parametri.durationInMinutes).TimeOfDay <=
                    radnoVreme.DatumIVremeZavrsetka.TimeOfDay)
                {
                    slobodan = true;
                    break;
                }
            }

            if (slobodan)
            {

                List<Termin> terminiLekara = SkladisteZaTermine.getInstance().getByJmbgLekar(parametri.IDDoctor);
                foreach (Termin t in terminiLekara)
                {
                    if (DateTime.Compare(parametri.startTime, t.DatumIVremeTermina) > 0 && DateTime.Compare(parametri.startTime,
                        t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < 0)
                    {
                        slobodan = false;
                        break;
                    }

                    if (DateTime.Compare(parametri.startTime, t.DatumIVremeTermina) < 0 &&
                        DateTime.Compare(parametri.startTime.AddMinutes(t.TrajanjeTermina), t.DatumIVremeTermina) >
                        0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                    {
                        slobodan = false;
                        break;
                    }

                    if (DateTime.Compare(t.DatumIVremeTermina, parametri.startTime) == 0)
                    {
                        slobodan = false;
                        break;
                    }
                }
            }

            return slobodan;
        }

        public bool ObrisiLekara(string jmbg)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            bool uspesno = false;
            foreach (Lekar lekar in lekari)
            {
                if (lekar.Jmbg.Equals(jmbg))
                {
                    uspesno = lekari.Remove(lekar);
                    break;
                }
            }
            skladisteZaLekara.SaveAll(lekari);
            return uspesno;
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
        public void izdajRecept(ReceptiDTO parametri)
        {
            Recept Recept = new Recept(parametri.ImeLeka, parametri.SifraLeka, parametri.DodatneNapomene, parametri.DatumIzdavanja, parametri.BrojDana, parametri.Doza, parametri.terminiUzimanjaTokomDana, parametri.Dijagnoza, parametri.ImeDoktora);
            List<Recept> recepti = new List<Recept>();
            recepti.Add(Recept);
            Izvestaj izvestaj = new Izvestaj(recepti);
            List<Izvestaj> izvestaji = new List<Izvestaj>();
            izvestaji.Add(izvestaj);
            parametri.p1.zdravstveniKarton.izvestaj.Add(izvestaj);
            PacijentKontroler.getInstance().izmeniPacijenta(parametri.p, parametri.p1);
        }
        public List<int> dobijTerminePijenja(String terminiPijenja)
        {
            String[] termini = terminiPijenja.Split(',');
            List<int> terminiInt = new List<int>();
            for (int i = 0; i < termini.Length; i++)
            {
                String k = termini[i];
                terminiInt.Add(int.Parse(k));

            }
            return terminiInt;
        }

        private RadnoVremeServis radnoVremeServis = new RadnoVremeServis();
    }
}