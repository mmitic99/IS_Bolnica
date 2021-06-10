using Bolnica.DTOs;
using Bolnica.Servis;
using Bolnica.view;
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.model.Enum;
using System.Linq;
using Bolnica.Repozitorijum.ISkladista;

namespace Servis
{
    public class ObavestenjaServis
    {

        public static ObavestenjaServis instance;
        private ISkladisteZaObavestenja SkladisteZaObavestenja;
        private PacijentServis PacijentServis;
        private AnketeKvartalneServis AnketeKvartalneServis;
        private AnketeOLekaruServis AnketeOLekaruServis;
        private LekarServis LekarServis;

        public static ObavestenjaServis getInstance()
        {
            if (instance == null)
            {
                instance = new ObavestenjaServis();
            }
            return instance;
        }

        public ObavestenjaServis()
        {
            this.PacijentServis = new PacijentServis();
            this.SkladisteZaObavestenja = new SkladisteZaObavestenjaXml();
            this.AnketeKvartalneServis = new AnketeKvartalneServis();
            this.AnketeOLekaruServis = new AnketeOLekaruServis();
            this.LekarServis = new LekarServis();
        }

        public List<Obavestenje> GetAll()
        {
            return SkladisteZaObavestenja.GetAll();
        }

        public void Save(Model.Obavestenje obavestenje)
        {
            SkladisteZaObavestenja.Save(obavestenje);
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            SkladisteZaObavestenja.SaveAll(obavestenje);
        }

        public List<Obavestenje> GetObavestenjaByJmbg(String jmbg)
        {
            List<Obavestenje> obavestenja = SkladisteZaObavestenja.GetObavestenjaByJmbg(jmbg);
            foreach(Obavestenje obavestenje in SkladisteZaObavestenja.GetObavestenjaByJmbg("-1"))
            {
                if (!obavestenja.Contains(obavestenje))
                {
                    obavestenja.Add(obavestenje);
                }
            }
            List<Obavestenje> sortiranaObavestenja = obavestenja.OrderByDescending(x => x.VremeObavestenja).ToList();
            return obavestenja;
        }

        public void PosaljiAnketuOLekaru(string JmbgPacijenta, string JmbgLekara)
        {
            Pacijent pacijent = PacijentServis.GetByJmbg(JmbgPacijenta);
            Lekar lekar = LekarServis.GetByJmbg(JmbgLekara);
            AnketeOLekaruServis.GetAnketaOLekaru(JmbgLekara);
            PrikacenaAnketaPoslePregledaDTO anketaOLekaru = new PrikacenaAnketaPoslePregledaDTO()
            {
                IDAnkete = JmbgPacijenta + JmbgLekara + DateTime.Now.ToString(),
                JmbgLekara = JmbgLekara
            };
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = JmbgPacijenta,
                Naslov = "Anketa o nedavnom pregledu",
                Sadrzaj = "Poštovani/a " + pacijent.Ime + "\r\n" + "nedavno ste bili na pregledu kod " + lekar.FullName + ". Molimo Vas da popunite anketu o usluzi koja Vam je pružena i na taj način pomognete da poboljšamo komunikaciju i usluge koje naša bolnica pruža." +
                "Hvala Vam na izdvojenom vremenu." + "\r\n\n" + "Prijatan dan Vam želi ZDRAVO bolnica.",
                Vidjeno = false,
                anketaOLekaru = anketaOLekaru            
            };
            SkladisteZaObavestenja.Save(obavestenje);
        }

        internal void PosaljiKvartalnuAnketu()
        {
            AnketeKvartalneServis.GetKvartalnaAnketa(DateTime.Today);
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = "-1",
                Naslov = "Aktivna "+GetNazivMesec(DateTime.Now.Month)+" anketa",
                Sadrzaj = "Poštovani pacijenti," +"\r\n\n"+"obaveštavamo vas da je do "+DateTime.Now.Date.AddDays(15).ToString("d.M.yyyy")+" aktivna anketa o radu naše bolnice. Bili bismo Vam mnogo zahvalni ako izdvojite vreme i popunite anketu, na taj načit ćete nam pomoći da unapredimo poslovanje naše bolnice i time učiniti komunikaciju i interakciju sa našom bolnicom još lakšom i pristupačnijom. Mišljenje naših pacijenata nam je najznačajnije." +
                "\r\n\n"+"Sve najbolje Vam želi ZDRAVO bolnica.",
                kvartalnaAnketa = DateTime.Today,
                Vidjeno = false
            };
            SkladisteZaObavestenja.Save(obavestenje);
        }

        private string GetNazivMesec(int month)
        {
            string tekstualniZapisMeseca="";
            if (month == 1) tekstualniZapisMeseca = "januarska";
            else if (month == 2) tekstualniZapisMeseca = "februarska";
            else if (month == 3) tekstualniZapisMeseca = "martovska";
            else if (month == 4) tekstualniZapisMeseca = "aprilska";
            else if (month == 5) tekstualniZapisMeseca = "majska";
            else if (month == 6) tekstualniZapisMeseca = "junska";
            else if (month == 7) tekstualniZapisMeseca = "julska";
            else if (month == 8) tekstualniZapisMeseca = "avgustovska";
            else if (month == 9) tekstualniZapisMeseca = "septembarska";
            else if (month == 10) tekstualniZapisMeseca = "oktobarska";
            else if (month == 11) tekstualniZapisMeseca = "novembarska";
            else tekstualniZapisMeseca = "decembarska";
            return tekstualniZapisMeseca;
        }

        public bool IzmeniObavestenje(Obavestenje staroObavestenje, Obavestenje novoObavestenje)
        {
            ObrisiObavestenje(staroObavestenje);
            SkladisteZaObavestenja.Save(novoObavestenje);
            return true;
        }

        public bool ObrisiObavestenje(Obavestenje obavestenje)
        {
            bool retVal = false;
            List<Obavestenje> obavestenja = SkladisteZaObavestenja.GetAll();
            foreach (Obavestenje obavestenje1 in obavestenja)
            {
                if (obavestenje1.Equals(obavestenje))
                {
                    obavestenja.Remove(obavestenje);
                    SkladisteZaObavestenja.SaveAll(obavestenja);
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
    }
}