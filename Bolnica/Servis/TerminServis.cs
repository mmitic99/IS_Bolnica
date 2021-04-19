using Bolnica.viewActions;
using Model;
using Repozitorijum;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Servis
{
    public class TerminServis
    {
        public SkladisteZaTermine skladisteZaTermine;
        public SkladisteZaObavestenja skladisteZaObavestenja;
        public static TerminServis instance =null;
        public const int MAX_BR_TERMINA_PRIKAZ =10;

        public static TerminServis getInstance()
        {
            if(instance == null)
            {
                return new TerminServis();
            }
            else
            {
                return instance;
            }
        }



        public TerminServis()
        {
            instance = this;
            skladisteZaObavestenja = SkladisteZaObavestenja.GetInstance();
            skladisteZaTermine = new SkladisteZaTermine();
        }

        public bool ZakaziTermin(Model.Termin termin)
        {
            skladisteZaTermine.Save(termin);

            return true;
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniTermin(Termin termin, string stariIdTermina = null)
        {
            Termin stariTermin;
            if(stariIdTermina!=null)
            {
                stariTermin = SkladisteZaTermine.getInstance().getById(stariIdTermina);
                skladisteZaTermine.RemoveByID(stariIdTermina);

            }
            else
            {
                stariTermin = skladisteZaTermine.getById(termin.IDTermina);
                skladisteZaTermine.RemoveByID(termin.IDTermina);
            }
            bool uspesno = true;
            
            termin.IDTermina = termin.generateRandId();

            Obavestenje obavestenjePacijent = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgPacijenta,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + stariTermin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now,
                Podsetnik = false
                
            };
            skladisteZaObavestenja.Save(obavestenjePacijent);

            Obavestenje obavestenjeLekar = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgLekara,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + stariTermin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            skladisteZaObavestenja.Save(obavestenjeLekar);

            skladisteZaTermine.Save(termin);

            return uspesno;
        }

        public List<Termin> NadjiTermineZaParametre(String jmbgLekara, String jmbgPacijenta, List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, int prioritet, String tegobe)
        {
            List<Termin> moguciTermini = new List<Termin>();
            if (prioritet == 0) //nema prioritet
            {
                moguciTermini = NadjiTermineIzBuducnosti(jmbgPacijenta, tegobe);
            }
            else if (prioritet == 1)
            {
                if (dani.Count == 0)
                {
                    moguciTermini = DobaviMoguceTerminePoLekaru(jmbgLekara,jmbgPacijenta, tegobe);
                } 
                else
                {
                    moguciTermini = DobaviTerminPoLekaruZaInterval(jmbgLekara, dani, pocetak, kraj, jmbgPacijenta, tegobe);
                    List<Termin> moguciTerminiBiloKadaZaLekara = DobaviMoguceTerminePoLekaru(jmbgLekara, jmbgPacijenta, tegobe);
                    int i = 0;
                    while(moguciTermini.Count < 10)
                    {
                        moguciTermini.Add(moguciTerminiBiloKadaZaLekara[i++]);
                    }
                }
            }
            else if (prioritet == 2)
            {
                if (dani.Count != 0)
                {
                    moguciTermini = DobaviTerminPoLekaruZaInterval(jmbgLekara, dani, pocetak, kraj, jmbgPacijenta, tegobe);
                    List<Termin> moguciTerminiUIntervalu = DobaviTerminZaInterval(dani, pocetak, kraj, jmbgPacijenta, tegobe);
                    int i = 0;
                    while (moguciTermini.Count < 10 && i<moguciTerminiUIntervalu.Count)
                    {
                        if (terminNijeUListi(moguciTermini, moguciTerminiUIntervalu[i]))
                        {
                            moguciTermini.Add(moguciTerminiUIntervalu[i]);

                        }
                        i++;
                    }
                }
                else
                {
                    moguciTermini = NadjiTermineIzBuducnosti(jmbgPacijenta, tegobe);
                }
            }
            return moguciTermini;
        }

        public List<Termin> NadjiTermineZaParametrePomeranje(Termin termin, string jmbgLekara, List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, int prioritet, string opisTegobe)
        {
            List<Termin> moguciTermini = new List<Termin>();
            List<DateTime> moguciDani = nabaviMoguceDanePomeranja(termin.DatumIVremeTermina);
            if (prioritet == 0) //nema prioritet
            {
                moguciTermini = DobaviTerminZaInterval(moguciDani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
            }
            else if (prioritet == 1)
            {
                if (dani.Count == 0)
                {
                    moguciTermini = DobaviTerminPoLekaruZaInterval(jmbgLekara, moguciDani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
                }
                else
                {
                    moguciTermini = DobaviTerminPoLekaruZaInterval(jmbgLekara, dani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
                }
            }
            else if (prioritet == 2)
            {
                if (dani.Count != 0)
                {
                    moguciTermini = DobaviTerminPoLekaruZaInterval(jmbgLekara, dani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
                    List<Termin> moguciTerminiUIntervalu = DobaviTerminZaInterval(dani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
                    int i = 0;
                    while (moguciTermini.Count < 10 && i<moguciTerminiUIntervalu.Count)
                    {
                        if (terminNijeUListi(moguciTermini, moguciTerminiUIntervalu[i]))
                        {
                            moguciTermini.Add(moguciTerminiUIntervalu[i]);

                        }
                        i++;
                    }
                }
                else
                {
                    moguciTermini = DobaviTerminZaInterval(moguciDani, pocetak, kraj, termin.JmbgPacijenta, opisTegobe);
                }
            }
            return moguciTermini;
        }

        private List<DateTime> nabaviMoguceDanePomeranja(DateTime datumIVremeTermina)
        {
            List<DateTime> moguciDaniPomeranja = new List<DateTime>();
            datumIVremeTermina = datumIVremeTermina.Date + new TimeSpan(0, 0, 0);
            DateTime prvi = datumIVremeTermina.AddDays(-3);
            DateTime poslednji = datumIVremeTermina.AddDays(3);
            while(prvi<poslednji || prvi.Equals(poslednji))
            {
                if (prvi > DateTime.Today) 
                {
                    moguciDaniPomeranja.Add(prvi);
                 }
                prvi = prvi.AddDays(1);
            }
            return moguciDaniPomeranja;
        }

        private bool terminNijeUListi(List<Termin> moguciTermini, Termin termin)
        {
            bool nijeUListi = true;
            foreach(Termin t in moguciTermini)
            {
                if(t.IDTermina.Equals(termin.IDTermina))
                {
                    nijeUListi = false;
                    break;
                }
            }
            return nijeUListi;
        }

        public List<Termin> NadjiTermineIzBuducnosti(String jmbgPacijenta, String tegobe)
        {
            List<Termin> moguciTermini = new List<Termin>();
            int brNadjenihTermina = 0;

            DateTime datum = dobaviSutrasnjiDan();

            while (brNadjenihTermina < MAX_BR_TERMINA_PRIKAZ 
                && datum < DateTime.Now.AddMonths(3))
            {
                List<Termin> terminiZaTacnoVreme = nadjiTermineZaTacnoVreme(datum, 30);
                if(terminiZaTacnoVreme.Count>0)
                {
                    foreach(Termin t in terminiZaTacnoVreme)
                    {
                        if(t.DatumIVremeTermina > DateTime.Today.AddDays(1) 
                            && PacijentServis.getInstance().DaLiJePacijentSlobodan(jmbgPacijenta, t.DatumIVremeTermina))
                        {
                            t.JmbgPacijenta = jmbgPacijenta;
                            t.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                            t.opisTegobe = tegobe;
                            t.IdProstorije = SkladisteZaProstorije.GetInstance().GetAll()[0].IdProstorije;
                            t.IDTermina = t.generateRandId();
                           
                            moguciTermini.Add(t);
                            if (++brNadjenihTermina == MAX_BR_TERMINA_PRIKAZ)
                            {
                                break;
                            }
                        }
                    }
                }
                datum = dobaviSledeciMoguciTajming(datum);               
            }
            return moguciTermini;
        }

        private DateTime dobaviSutrasnjiDan()
        {
            DateTime datum = new DateTime();
            TimeSpan vreme = new TimeSpan(6, 0, 0);
            datum = DateTime.Now;
            datum = datum.AddDays(1);
            datum = datum.Date + vreme;
            return datum;
        }

        public DateTime dobaviSledeciMoguciTajming(DateTime datumVreme)
        {
            if (datumVreme.Hour < 19 || datumVreme.Minute == 0)
            {
                datumVreme = datumVreme.AddMinutes(30);
            }
            else
            {
                datumVreme = datumVreme.AddDays(1);
                datumVreme = datumVreme.Date + new TimeSpan(6, 0, 0);
            }
            return datumVreme;
        }

        public List<Termin> nadjiTermineZaTacnoVreme(DateTime vreme, int trajanje, String jmbgLekara = null)
        {
            List<Termin> terminiZaTacnoVreme = new List<Termin>();
            if (jmbgLekara == null)
            {
                List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
                foreach (Lekar lekar in lekari)
                {
                    if(vreme > DateTime.Today.AddDays(1) 
                        && LekarServis.getInstance().DaLiJeLekarSlobodan(lekar.Jmbg, vreme, trajanje))
                    {
                        Termin t = new Termin()
                        {
                            JmbgLekara = lekar.Jmbg,
                            DatumIVremeTermina = vreme,
                            TrajanjeTermina = trajanje
                        };
                        terminiZaTacnoVreme.Add(t);
                    }
                }
            
            }
            else
            {
                if (vreme > DateTime.Today.AddDays(1) 
                    && LekarServis.getInstance().DaLiJeLekarSlobodan(jmbgLekara, vreme, trajanje))
                {
                    Termin t = new Termin()
                    {
                        JmbgLekara = jmbgLekara,
                        DatumIVremeTermina = vreme,
                        TrajanjeTermina = trajanje
                    };
                    terminiZaTacnoVreme.Add(t);
                }
            }
            return terminiZaTacnoVreme;     
        }

        public bool OdloziTermin(Termin termin)
        {
            return true;
        }   

        public void RemoveByID(string iDTermina)
        {
            skladisteZaTermine.RemoveByID(iDTermina);
        }

        public List<Termin> DobaviMoguceTerminePoLekaru(String jmbgLekara, String jmbgPacijenta=null, String tegobe = null)
        {
            // TODO: implement
            DateTime datum = dobaviSutrasnjiDan();
            List<Termin> moguciTerminiLekara = new List<Termin>();
            while(moguciTerminiLekara.Count<MAX_BR_TERMINA_PRIKAZ
                && datum<DateTime.Now.AddMonths(3))
            {
                List<Termin> terminiZaTacnoVremeLekara = nadjiTermineZaTacnoVreme(datum, 30, jmbgLekara);
                if (terminiZaTacnoVremeLekara.Count > 0)
                {
                    foreach (Termin t in terminiZaTacnoVremeLekara)
                    {
                        if (jmbgPacijenta!=null &&
                            PacijentServis.getInstance().DaLiJePacijentSlobodan(jmbgPacijenta, t.DatumIVremeTermina)
                            && t.DatumIVremeTermina > DateTime.Today.AddDays(1))
                        {
                            t.JmbgPacijenta = jmbgPacijenta;
                            t.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                            t.opisTegobe = tegobe;
                            t.IdProstorije = SkladisteZaProstorije.GetInstance().GetAll()[0].IdProstorije;
                            t.IDTermina = t.generateRandId();
                            moguciTerminiLekara.Add(t);

                        }
                        if (moguciTerminiLekara.Count == MAX_BR_TERMINA_PRIKAZ)
                        {
                            break;
                        }
                    }
                }
                datum = dobaviSledeciMoguciTajming(datum);
            }
            return moguciTerminiLekara;
        }

        public List<Termin> DobaviTerminZaInterval(List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, String jmbgPacijenta = null, String tegobe = null)
        {
            // TODO: implement
            List<Termin> moguciBuduciTermini = new List<Termin>();
            foreach (DateTime dan in dani)
            {
                DateTime danMinVreme = dan.Date + pocetak;
                DateTime danMaxVreme = dan.Date + kraj;
                while (danMinVreme.AddMinutes(30) < danMaxVreme
                    || danMinVreme.AddMinutes(30).Equals(danMaxVreme)) 
                {
                    List<Termin> terminiZaTacnoVreme = nadjiTermineZaTacnoVreme(danMinVreme, 30);
                    if (terminiZaTacnoVreme.Count > 0)
                    {
                        foreach (Termin t in terminiZaTacnoVreme)
                        {
                            if (jmbgPacijenta != null)
                            {
                                t.JmbgPacijenta = jmbgPacijenta;
                                t.opisTegobe = tegobe;
                                t.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                                t.IDTermina = t.generateRandId();
                            }
                            if (t.DatumIVremeTermina > DateTime.Today.AddDays(1))
                                moguciBuduciTermini.Add(t);
                        }
                    }
                    danMinVreme = dobaviSledeciMoguciTajming(danMinVreme);
                }
                if (moguciBuduciTermini.Count == MAX_BR_TERMINA_PRIKAZ)
                {
                    break;
                }
            }
            return moguciBuduciTermini;
        }

        public List<Termin> DobaviTerminPoLekaruZaInterval(String jmbgLekara,List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, String jmbgPacijenta = null, String tegobe = null)
        {
            List<Termin> moguciBuduciTermini = new List<Termin>();
            foreach(DateTime dan in dani)
             {
                DateTime danMinVreme = dan.Date + pocetak;
                DateTime danMaxVreme = dan.Date + kraj;
                while(danMinVreme.AddMinutes(30) < danMaxVreme
                    || danMinVreme.AddMinutes(30).Equals(danMaxVreme))
                {
                    List<Termin> terminiZaTacnoVreme = nadjiTermineZaTacnoVreme(danMinVreme, 30, jmbgLekara);
                    if(terminiZaTacnoVreme.Count>0)
                    {
                        if(jmbgPacijenta!= null)
                        {
                            terminiZaTacnoVreme[0].JmbgPacijenta = jmbgPacijenta;
                            terminiZaTacnoVreme[0].opisTegobe = tegobe;
                            terminiZaTacnoVreme[0].VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                            terminiZaTacnoVreme[0].IDTermina = terminiZaTacnoVreme[0].generateRandId();
                        }
                        moguciBuduciTermini.Add(terminiZaTacnoVreme[0]);
                    }
                    danMinVreme = dobaviSledeciMoguciTajming(danMinVreme);
                }
                if(moguciBuduciTermini.Count ==10)
                {
                    break;
                }
             }
            return moguciBuduciTermini;
        }

        public bool ProveriTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public List<Termin> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Termin termin)
        {
            // TODO: implement
        }

        public void SaveAll(List<Termin> termini)
        {
            // TODO: implement
        }



        internal IEnumerable GetBuduciTerminPacLekar()
        {
            return skladisteZaTermine.GetBuduciTerminPacLekar();
        }
    }
}