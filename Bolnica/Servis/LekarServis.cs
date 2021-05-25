using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Kontroler;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Bolnica.Repozitorijum.XmlSkladiste;
using static Bolnica.DTOs.ReceptDTO;

namespace Servis
{
    public class LekarServis : KorisnikServis
    {
        public PacijentServis PacijentServis;
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
            skladisteZaLekara = SkladisteZaLekaraXml.GetInstance();
            skladisteZaTermine = new SkladisteZaTermineXml();
            radnoVremeServis = new RadnoVremeServis();
            PacijentServis = new PacijentServis();
            instance = this;
        }

        public bool RegistrujLekara(Lekar lekar)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();

            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari.ElementAt(i).Jmbg.Equals(lekar.Jmbg))
                {
                    return false;
                }
            }
            skladisteZaLekara.Save(lekar);
            return true;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public void IzmeniLekara(string jmbg, Lekar lekar)
        {
            ObrisiLekara(jmbg);
            Save(lekar);
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            foreach (Lekar lekar1 in lekari)
            {
                if (lekar1.Korisnik.KorisnickoIme.Equals(korisnickoIme) && lekar1.Korisnik.Lozinka.Equals(lozinka))
                {
                    return lekar1;
                }
            }
            return null;
        }

        public bool DaLiJeLekarSlobodan(ParamsToCheckAvailabilityOfDoctorDTO parametri)
        {
            bool slobodan = DaLiLekarRadi(parametri);

            if (slobodan)
            {
                slobodan = ProveriTermineLekara(parametri);
            }

            return slobodan;
        }

        private bool ProveriTermineLekara(ParamsToCheckAvailabilityOfDoctorDTO parametri)
        {
            List<Termin> terminiLekara = skladisteZaTermine.GetByJmbgLekar(parametri.IDDoctor);
            foreach (Termin termin in terminiLekara)
            {
                if (NoviTerminUnutarStarogTermina(parametri, termin) || 
                    StariTerminUnutarNovogTermina(parametri, termin) ||
                    TerminiSePoklapaju(parametri, termin))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TerminiSePoklapaju(ParamsToCheckAvailabilityOfDoctorDTO parametri, Termin termin)
        {
            return DateTime.Compare(termin.DatumIVremeTermina, parametri.startTime) == 0;
        }

        private static bool StariTerminUnutarNovogTermina(ParamsToCheckAvailabilityOfDoctorDTO parametri, Termin termin)
        {
            return DateTime.Compare(parametri.startTime, termin.DatumIVremeTermina) < 0 &&
                   DateTime.Compare(parametri.startTime.AddMinutes(termin.TrajanjeTermina), termin.DatumIVremeTermina) >
                   0;
        }

        private static bool NoviTerminUnutarStarogTermina(ParamsToCheckAvailabilityOfDoctorDTO parametri, Termin termin)
        {
            return DateTime.Compare(parametri.startTime, termin.DatumIVremeTermina) > 0 && DateTime.Compare(parametri.startTime,
                termin.DatumIVremeTermina.AddMinutes(termin.TrajanjeTermina)) < 0;
        }

        private bool DaLiLekarRadi(ParamsToCheckAvailabilityOfDoctorDTO parametri)
        {
            foreach (RadnoVreme radnoVreme in radnoVremeServis.GetByJmbgAkoRadi(parametri.IDDoctor))
            {
                if (LekarRadi(parametri, radnoVreme))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool LekarRadi(ParamsToCheckAvailabilityOfDoctorDTO parametri, RadnoVreme radnoVreme)
        {
            return parametri.startTime.Date >= radnoVreme.DatumIVremePocetka.Date &&
                   parametri.startTime.AddMinutes(parametri.durationInMinutes).Date <= radnoVreme.DatumIVremeZavrsetka.Date &&
                   parametri.startTime.TimeOfDay >= radnoVreme.DatumIVremePocetka.TimeOfDay &&
                   parametri.startTime.AddMinutes(parametri.durationInMinutes).TimeOfDay <=  radnoVreme.DatumIVremeZavrsetka.TimeOfDay;
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
            List<Lekar> lekari = skladisteZaLekara.GetAll();
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
            return skladisteZaLekara.GetAll();
        }

        public void Save(Lekar lekar)
        {
            skladisteZaLekara.Save(lekar);
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }
        public Lekar GetByJmbg(string jmbg)
        {
            return skladisteZaLekara.getByJmbg(jmbg);
        }

        public ISkladisteZaLekara skladisteZaLekara;
        public ISkladisteZaTermine skladisteZaTermine;
        public void izdajRecept(ReceptiDTO parametri)
        {
            Recept Recept = new Recept(parametri.ImeLeka, parametri.SifraLeka, parametri.DodatneNapomene, parametri.DatumIzdavanja, parametri.BrojDana, parametri.Doza, parametri.terminiUzimanjaTokomDana, parametri.Dijagnoza, parametri.ImeDoktora);
            List<Recept> recepti = new List<Recept>();
            Recept.IdRecepta = PacijentServis.GetByJmbg(parametri.p.Jmbg).ZdravstveniKarton.Izvestaj.Count+1;
            recepti.Add(Recept);
            Izvestaj izvestaj = new Izvestaj(recepti);
            List<Izvestaj> izvestaji = new List<Izvestaj>();
            izvestaji.Add(izvestaj);
            parametri.p1.ZdravstveniKarton.Izvestaj.Add(izvestaj);
            PacijentServis.IzmeniPacijenta(parametri.p, parametri.p1);
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

        private RadnoVremeServis radnoVremeServis;
    }
}