using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using static Bolnica.DTOs.ReceptDTO;
using Bolnica.view;

namespace Servis
{
    public class LekarServis : KorisnikServis
    {
        public PacijentServis PacijentServis;
        public static LekarServis instance = null;
        public ISkladisteZaLekara skladisteZaLekara;
        public ISkladisteZaTermine skladisteZaTermine;
        private RadnoVremeServis radnoVremeServis;

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
            bool sacuvaj = true;
            List<Lekar> lekari = skladisteZaLekara.GetAll();

            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari.ElementAt(i).Jmbg.Equals(lekar.Jmbg))
                {
                    sacuvaj = false;
                }
            }
            if(sacuvaj)
                skladisteZaLekara.Save(lekar);
            return sacuvaj;
        }

        public void IzmeniLekara(string jmbg, Lekar lekar)
        {
            ObrisiLekaraZaIzmenu(jmbg);
            Save(lekar);
        }

        private void ObrisiLekaraZaIzmenu(string jmbg)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.Jmbg.Equals(jmbg))
                {
                    lekari.Remove(lekar);
                    skladisteZaLekara.SaveAll(lekari);
                    break;
                }
            }
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
            bool retVal = true;
            List<Termin> terminiLekara = skladisteZaTermine.GetByJmbgLekar(parametri.IDDoctor);
            foreach (Termin termin in terminiLekara)
            {
                if (NoviTerminUnutarStarogTermina(parametri, termin) || 
                    StariTerminUnutarNovogTermina(parametri, termin) ||
                    TerminiSePoklapaju(parametri, termin))
                {
                    retVal = false;
                }
            }

            return retVal;
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
            bool retVal = false;
            foreach (RadnoVreme radnoVreme in radnoVremeServis.GetByJmbgAkoRadi(parametri.IDDoctor))
            {
                if (LekarRadi(parametri, radnoVreme))
                {
                    retVal = true;
                }
            }

            return retVal;
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
                if (lekar.Jmbg.Equals(jmbg) && LekarNemaTermine(jmbg))
                {
                    uspesno = lekari.Remove(lekar);
                    skladisteZaLekara.SaveAll(lekari);
                    break;
                }
            }
            return uspesno;
        }

        private bool LekarNemaTermine(string jmbg)
        {
            return !(TerminServis.getInstance().GetByJmbgLekar(jmbg).Count > 0);
        }

        internal int DobaviIndeksSelectovanogLekara(Termin termin)
        {
            int indeksSelektovanog = 0;
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari[i].Jmbg.Equals(termin.JmbgLekara))
                {
                    indeksSelektovanog = i;
                    break;
                }
            }
            return indeksSelektovanog;
        }

        public bool IzmenaLozinke(string jmbg, string novaLozinka)
        {
            Lekar l =skladisteZaLekara.getByJmbg(jmbg);
            l.Korisnik.Lozinka = novaLozinka;
            skladisteZaLekara.Save(l);
            return true;
        }

        public bool IzmenaKorisnickogImena(string jmbg, string novoKorisnickoIme)
        {
            Lekar l = skladisteZaLekara.getByJmbg(jmbg);
            l.Korisnik.KorisnickoIme = novoKorisnickoIme;
            skladisteZaLekara.Save(l);
            return true;
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
            skladisteZaLekara.SaveAll(lekari);
        }
        public Lekar GetByJmbg(string jmbg)
        {
            return skladisteZaLekara.getByJmbg(jmbg);
        }

        public void izdajRecept(ReceptiDTO parametri)
        {
            Recept Recept = new Recept(parametri.ImeLeka, parametri.SifraLeka, parametri.DodatneNapomene, parametri.DatumIzdavanja, parametri.BrojDana, parametri.Doza, parametri.terminiUzimanjaTokomDana, parametri.Dijagnoza, parametri.ImeDoktora);
            List<Recept> recepti = new List<Recept>();
            Recept.IdRecepta = PacijentServis.GetByJmbg(parametri.p.Jmbg).ZdravstveniKarton.Izvestaj.Count+1;
            recepti.Add(Recept);
            Izvestaj izvestaj = new Izvestaj(recepti);
            List<Izvestaj> izvestaji = new List<Izvestaj>();
            izvestaji.Add(izvestaj);
            Pacijent pacijent = PacijentServis.GetInstance().GetByJmbg(parametri.p1.Jmbg);
            Pacijent pacijent1 = pacijent;
            pacijent1.ZdravstveniKarton.Izvestaj.Add(izvestaj);
            PacijentServis.GetInstance().IzmeniPacijenta(pacijent, pacijent1);
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

        public Lekar trenutnoUlogovaniLekar()
        {
            return LekarWindow.getInstance().lekarTrenutni;
        }

        public Lekar getByJmbg(string jmbgLekara)
        {
            return skladisteZaLekara.getByJmbg(jmbgLekara);
        }
    }
   
}