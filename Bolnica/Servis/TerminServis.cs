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
            if(termin.IdProstorije==null)
            {
                termin.IdProstorije = SkladisteZaProstorije.GetInstance().GetAll()[0].IdProstorije;
                termin.IDTermina = termin.generateRandId();
            }
            skladisteZaTermine.Save(termin);

            return true;
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniTermin(Termin termin)
        {
            bool uspesno = true;

            skladisteZaTermine.RemoveByID(termin.IDTermina);
            termin.IDTermina = termin.generateRandId();

            Obavestenje obavestenjePacijent = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgPacijenta,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            skladisteZaObavestenja.Save(obavestenjePacijent);

            Obavestenje obavestenjeLekar = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgLekara,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.DatumIVremeTermina + "" +
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
            if(prioritet == 0) //nema prioritet
            {
                moguciTermini = NadjiDesetTerminaIzBuducnosti(jmbgPacijenta, tegobe);
            }
            return moguciTermini;
        }

        public List<Termin> NadjiDesetTerminaIzBuducnosti(String jmbgPacijenta, String tegobe)
        {
            List<Termin> moguciTermini = new List<Termin>();
            int brNadjenihTermina = 0;
            DateTime datum = new DateTime();
            TimeSpan vreme = new TimeSpan(6, 0, 0);
            datum = DateTime.Now;
            datum = datum.AddDays(1);
            datum = datum.Date + vreme;

            while (brNadjenihTermina <10)
            {
                List<Termin> terminiZaTacnoVreme = nadjiTermineZaTacnoVreme(datum, 30);
                if(terminiZaTacnoVreme.Count>0)
                {
                    foreach(Termin t in terminiZaTacnoVreme)
                    {
                        if(PacijentServis.getInstance().DaLiJePacijentSlobodan(jmbgPacijenta, t.DatumIVremeTermina))
                        {
                            t.JmbgPacijenta = jmbgPacijenta;
                            t.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                            t.opisTegobe = tegobe;
                            moguciTermini.Add(t);
                            brNadjenihTermina++;
                        }
                        if(brNadjenihTermina==10)
                        {
                            break;
                        }
                    }
                }

                if(datum.Hour<19 || datum.Minute==0)
                {
                    datum = datum.AddMinutes(30);
                }
                else
                {
                    datum = datum.AddDays(1);
                }

            }
            return moguciTermini;
        }

        public List<Termin> nadjiTermineZaTacnoVreme(DateTime vreme, int trajanje, Lekar l = null)
        {
            List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
            List<Termin> terminiZaTacnoVreme = new List<Termin>();
            if (l == null)
            {
                foreach (Lekar lekar in lekari)
                {
                    if(LekarServis.getInstance().DaLiJeLekarSlobodan(lekar.Jmbg, vreme, trajanje))
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
                if (LekarServis.getInstance().DaLiJeLekarSlobodan(l.Jmbg, vreme, trajanje))
                {
                    Termin t = new Termin()
                    {
                        JmbgLekara = l.Jmbg,
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

        public List<Termin> DobaviMoguceTerminePoLekaru(int idLekara)
        {
            // TODO: implement
            return null;
        }

        public List<Termin> DobaviTerminZaInterval(DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return null;
        }

        public List<Termin> DobaviTerminPoLekaruZaInterval(int idLekara, DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return null;
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