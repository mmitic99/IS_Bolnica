using Bolnica.DTOs;
using Bolnica.Servis;
using Bolnica.view;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Servis
{
    public class ObavestenjaServis
    {

        public static ObavestenjaServis instance;

        public static ObavestenjaServis getInstance()
        {
            if (instance == null)
            {
                return new ObavestenjaServis();
            }
            else
            {
                return instance;
            }
        }

        public ObavestenjaServis()
        {
            skladisteZaObavestenja = SkladisteZaObavestenja.GetInstance();
        }

        public List<Obavestenje> GetAll()
        {
            // TODO: implement
            return skladisteZaObavestenja.GetAll();
        }

        public bool Save(Model.Obavestenje obavestenje)
        {
            return skladisteZaObavestenja.Save(obavestenje);
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            skladisteZaObavestenja.SaveAll(obavestenje);
        }

        public List<Obavestenje> GetByJmbg(String jmbg)
        {
            List<Obavestenje> obavestenja = skladisteZaObavestenja.GetObavestenjaByJmbg(jmbg);

            foreach(Obavestenje obavestenje in skladisteZaObavestenja.GetObavestenjaByJmbg("-1"))
            {
                if (!obavestenja.Contains(obavestenje))
                {
                    obavestenja.Add(obavestenje);
                }
            }

            return obavestenja;
        }

        public List<Obavestenje> GetPodsetnici(String jmbg)
        {
            return skladisteZaObavestenja.GetPodsetniciByJmbg(jmbg);
        }

        public bool napraviPodsetnik(string jmbgPacijenta, Recept r, int hours)
        {
            Pacijent pacijent = PacijentServis.getInstance().GetByJmbg(jmbgPacijenta);
            //ObavestenjaServis.getInstance().napraviPodsetnik(jmbgPacijenta, r, hours);
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = jmbgPacijenta,
                Podsetnik = true,
                Naslov = "Podsetnik o uzimanju leka -" + r.ImeLeka,
                Sadrzaj = "Poštovani/a " + pacijent.Ime + " podsećamo vas da danas u " + (DateTime.Today.AddHours(hours)).ToString("HH:mm") + 
                " treba da uzmete Vaš lek. Prijatan dan Vam želi ,,Zdravo bolnica"
            };
            return SkladisteZaObavestenja.GetInstance().Save(obavestenje);
        }

        internal void PosaljiAnketuOLekaru(string JmbgPacijenta, string JmbgLekara)
        {
            Pacijent pacijent = PacijentServis.getInstance().GetByJmbg(JmbgPacijenta);
            Lekar lekar = LekarServis.getInstance().GetByJmbg(JmbgLekara);
            AnketeServis.GetInstance().GetAnketaOLekaru(JmbgLekara);
            PrikacenaAnketaPoslePregledaDTO anketaOLekaru = new PrikacenaAnketaPoslePregledaDTO()
            {
                IDAnkete = JmbgPacijenta + JmbgLekara + DateTime.Now.ToString(),
                JmbgLekara = JmbgLekara
            };
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = JmbgPacijenta,
                Podsetnik = false,
                Naslov = "Anketa o nedavnom pregledu",
                Sadrzaj = "Poštovani/a " + pacijent.Ime + "\r\n" + "nedavno ste bili na pregledu kod " + lekar.FullName + ". Molimo Vas da popunite anketu o usluzi koja Vam je pružena i na taj način pomognete da poboljšamo komunikaciju i usluge koje naša bolnica pruža." +
                "Hvala Vam na izdvojenom vremenu." + "\r\n\n" + "Prijatan dan Vam želi ZDRAVO bolnica.",
                Vidjeno = false,
                anketaOLekaru = anketaOLekaru
                
            };
            SkladisteZaObavestenja.GetInstance().Save(obavestenje);
        }

        internal void PosaljiKvartalnuAnketu()
        {
            AnketeServis.GetInstance().GetKvartalnaAnketa(DateTime.Today);
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = "-1",
                Podsetnik=false,
                Naslov = "Aktivna "+GetNazivMesec(DateTime.Now.Month)+" anketa",
                Sadrzaj = "Poštovani pacijenti," +"\r\n\n"+"obaveštavamo vas da je do "+DateTime.Now.Date.AddDays(15).ToString("d.M.yyyy")+" aktivna anketa o radu naše bolnice. Bili bismo Vam mnogo zahvalni ako izdvojite vreme i popunite anketu, na taj načit ćete nam pomoći da unapredimo poslovanje naše bolnice i time učiniti komunikaciju i interakciju sa našom bolnicom još lakšom i pristupačnijom. Mišljenje naših pacijenata nam je najznačajnije." +
                "\r\n\n"+"Sve najbolje Vam želi ZDRAVO bolnica.",
                kvartalnaAnketa = DateTime.Today,
                Vidjeno = false
            };
            SkladisteZaObavestenja.GetInstance().Save(obavestenje);
        }

        private string GetNazivMesec(int month)
        {
            if (month == 1) return "januarska";
            else if (month == 2) return "februarska";
            else if (month == 3) return "martovska";
            else if (month == 4) return "aprilska";
            else if (month == 5) return "majska";
            else if (month == 6) return "junska";
            else if (month == 7) return "julska";
            else if (month == 8) return "avgustovska";
            else if (month == 9) return "septemvarska";
            else if (month == 10) return "oktobarska";
            else if (month == 11) return "novembarska";
            else return "decembarska";
        }

        public bool IzmeniObavestenje(Obavestenje staroObavestenje, Obavestenje novoObavestenje)
        {
            obrisiObavestenje(staroObavestenje);
            skladisteZaObavestenja.Save(novoObavestenje);
            return true;
        }

        public bool obrisiObavestenje(Obavestenje obavestenje)
        {
            List<Obavestenje> obavestenja = skladisteZaObavestenja.GetAll();
            foreach (Obavestenje obavestenje1 in obavestenja)
            {
                if (obavestenje1.Equals(obavestenje))
                {
                    obavestenja.Remove(obavestenje);
                    skladisteZaObavestenja.SaveAll(obavestenja);
                    return true;
                }
            }
            return false;
        }

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            /*Pacijent p = SkladistePacijenta.GetInstance().getByJmbg(jmbgPacijenta);
             p.zdravstveniKarton.izvestaj = new List<Izvestaj>();
             Izvestaj i = new Izvestaj();
             i.recepti = new List<Recept>();
             TimeSpan ts1 = new TimeSpan(8, 0, 0);
             TimeSpan ts2 = new TimeSpan(10, 0, 0);
             TimeSpan ts3 = new TimeSpan(12, 0, 0);
             List<int> terminiUzimanja = new List<int>();
             terminiUzimanja.Add(7);
             terminiUzimanja.Add(10);
             terminiUzimanja.Add(13);
             Recept r = new Recept();
             r.lek = new Lek();
             r.lek.NazivLeka = "Brufen";
             r.terminiUzimanjaTokomDana = terminiUzimanja;
             i.recepti.Add(r);
             p.zdravstveniKarton.izvestaj.Add(i);
             PacijentServis.getInstance().izmeniPacijenta(p, p);*/
            return skladisteZaObavestenja.GetPodsetniciByJmbg(jmbgPacijenta);
        }





        public Repozitorijum.SkladisteZaObavestenja skladisteZaObavestenja;

    }
}