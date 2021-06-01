using Bolnica.DTOs;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Servis
{
    public class PacijentServis : KorisnikServis
    {
        private KorisnickeAktivnostiPacijentaServis KorisnickeAktivnostiPacijentaServis;
        public static PacijentServis instance = null;

        public static PacijentServis GetInstance()
        {
            if(instance == null)
            {
                instance = new PacijentServis();
            }
            return instance;
        }
        public PacijentServis()
        {
            skladistePacijenta = SkladistePacijentaXml.GetInstance();
            skladisteZaTermine = new SkladisteZaTermineXml();
            skladisteZaObavestenja = new SkladisteZaObavestenjaXml();
            skladisteZaKorisnickeAktivnosti = new SkladisteZaKorisnickeAktivnostiXml();
            KorisnickeAktivnostiPacijentaServis = new KorisnickeAktivnostiPacijentaServis();
        }


        public bool RegistrujPacijenta(Pacijent pacijent)
        {
            bool retVal = true;
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();

            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti.ElementAt(i).Jmbg.Equals(pacijent.Jmbg))
                {
                    retVal =  false;
                }
            }
            if(retVal)
                skladistePacijenta.Save(pacijent);
            return retVal;
        }

        public Object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
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
            Pacijent pacijent = skladistePacijenta.GetByJmbg(jmbg);
            foreach(Izvestaj i in pacijent.ZdravstveniKarton.Izvestaj)
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

        public void SacuvajKomentarNaDijagnozu(Recept izabraniRecept, Pacijent pacijent)
        {
            for(int i=0; i<pacijent.ZdravstveniKarton.Izvestaj.Count; i++)
            {
                for(int j=0; j<pacijent.ZdravstveniKarton.Izvestaj[i].recepti.Count; j++)
                    if(pacijent.ZdravstveniKarton.Izvestaj[i].recepti[j].IdRecepta == izabraniRecept.IdRecepta)
                    {
                            pacijent.ZdravstveniKarton.Izvestaj[i].recepti[j] = izabraniRecept;
                    }
            }
            IzmeniPacijenta(pacijent, pacijent);
        }

        public bool IzmeniPacijenta(Pacijent stari, Pacijent novi)
        {
            List<Pacijent> pacijenti = skladistePacijenta.GetAll();
            bool uspesno = ObrisiPacijenta(stari.Jmbg);
            if (uspesno)
            {
                uspesno = RegistrujPacijenta(novi);
                if(!stari.Jmbg.Equals(novi.Jmbg))
                {
                    IzmeniJmbgPacijentaUTerminima(stari.Jmbg, novi.Jmbg);
                    IzmeniJmbgPacijentaUObavestenjima(stari.Jmbg, novi.Jmbg);
                    IzmeniJmbgPacijentaUAktivnostima(stari.Jmbg, novi.Jmbg);
                }

            }
            return uspesno;
        }

        private void IzmeniJmbgPacijentaUAktivnostima(string stariJmbg, string noviJmbg)
        {
            foreach (KorisnickeAktivnostiNaAplikaciji korisnickaAktivnost in skladisteZaKorisnickeAktivnosti.GetAll())
            {
                if (korisnickaAktivnost.JmbgKorisnika.Equals(stariJmbg))
                {
                    korisnickaAktivnost.JmbgKorisnika = noviJmbg;
                    KorisnickeAktivnostiPacijentaServis.IzmenaKorisnickeAktivnosti(korisnickaAktivnost, noviJmbg);
                }
            }
        }

        private void IzmeniJmbgPacijentaUObavestenjima(string stariJmbg, string noviJmbg)
        {
            foreach (Obavestenje obavestenje in skladisteZaObavestenja.GetAll())
            {
                if (obavestenje.JmbgKorisnika.Equals(stariJmbg))
                {
                    ObavestenjaServis.getInstance().IzmeniObavestenje(obavestenje, new Obavestenje()
                    {
                        JmbgKorisnika = noviJmbg, Naslov = obavestenje.Naslov,
                        Sadrzaj = obavestenje.Sadrzaj, Vidjeno = obavestenje.Vidjeno,
                        VremeObavestenja = obavestenje.VremeObavestenja, anketaOLekaru = obavestenje.anketaOLekaru,
                        kvartalnaAnketa = obavestenje.kvartalnaAnketa
                    });
                }
            }
        }

        private void IzmeniJmbgPacijentaUTerminima(string stariJmbg, string noviJmbg)
        {
            foreach (Termin termin in skladisteZaTermine.GetAll())
            {
                if (termin.JmbgPacijenta.Equals(stariJmbg))
                {
                    termin.JmbgPacijenta = noviJmbg;
                    TerminServis.getInstance().IzmeniTermin(termin);
                }
            }
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
            List<Termin> terminiPacijenta = skladisteZaTermine.GetByJmbgPacijenta(parametri.Id);
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
            return skladistePacijenta.GetByJmbg(jmbg);
        }

        public List<Pacijent> GetAll()
        {
            return skladistePacijenta.GetAll();
        }

        public void Save(Model.Pacijent pacijent)
        {
            skladistePacijenta.Save(pacijent);
        }


        public ISkladistePacijenta skladistePacijenta;
        public ISkladisteZaTermine skladisteZaTermine;
        public ISkladisteZaObavestenja skladisteZaObavestenja;
        public ISkladisteZaKorisnickeAktivnosti skladisteZaKorisnickeAktivnosti;

    }
}