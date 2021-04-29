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