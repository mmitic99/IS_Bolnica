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
        private ISkladisteZaPodsetnike SkladisteZaPodsetnike;
        private PacijentServis PacijentServis;
        private AnketeServis AnketeServis;
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
            this.SkladisteZaPodsetnike = new SkladisteZaPodsetnikeXml();
            this.AnketeServis = new AnketeServis();
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

        public List<Podsetnik> GetPodsetnici(String jmbg)
        {
            return SkladisteZaPodsetnike.GetPodsetniciByJmbg(jmbg);
        }

        public bool NapraviPodsetnikZaUzimanjeLeka(string jmbgPacijenta, Recept r, int hours)
        {
            Pacijent pacijent = PacijentServis.GetByJmbg(jmbgPacijenta);
            Podsetnik podsetnik = new Podsetnik()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = jmbgPacijenta,
                Ubaceno = true,
                VrstaPodsetnika = VrstaPodsetnika.UzimanjeLeka,
                Naslov = "Podsetnik o uzimanju leka -" + r.ImeLeka,
                Sadrzaj = "Poštovani/a " + pacijent.Ime + " podsećamo vas da danas u " + (DateTime.Today.AddHours(hours)).ToString("HH:mm") + 
                " treba da uzmete Vaš lek. Prijatan dan Vam želi ,,Zdravo bolnica"
            };
            return PosaljiPodsetnik(podsetnik);
         }

        internal bool NapravikorisnickePodsetnike(KorisnickiPodsetnikKlasifikovnoDTO podsetnikKlasifikovano)
        {
            foreach(DateTime datum in podsetnikKlasifikovano.Datumi)
            {
                NapraviKorisnickiPodsetnik(podsetnikKlasifikovano, datum);
            }
            return true;
        }

        private void NapraviKorisnickiPodsetnik(KorisnickiPodsetnikKlasifikovnoDTO podsetnikKlasifikovano, DateTime datum)
        {
            Podsetnik podsetnik = new Podsetnik()
            {
                VremeObavestenja = datum,
                JmbgKorisnika = podsetnikKlasifikovano.JmbgKorisnika,
                Ubaceno = false,
                VrstaPodsetnika = VrstaPodsetnika.Korisnicki,
                Naslov = podsetnikKlasifikovano.Naslov,
                Sadrzaj = podsetnikKlasifikovano.Sadrzaj,
                Vidjeno = false
            };
            PosaljiPodsetnik(podsetnik);
        }

        private bool PosaljiPodsetnik(Podsetnik podsetnik)
        {
            bool uspesnoPoslat = false;
            if (!DaLiJePrethodnoPodsetnikPoslat(podsetnik))
            {
                SkladisteZaPodsetnike.Save(podsetnik);
                uspesnoPoslat = true;
            }
            return uspesnoPoslat;    
        }

        private bool DaLiJePrethodnoPodsetnikPoslat(Podsetnik podsetnik)
        {
            bool poslatPodsetnik = false;
            List<Podsetnik> podsetnici = SkladisteZaPodsetnike.GetAll();
            foreach(Podsetnik p in podsetnici)
            {
                if (DaLiJeIstiPodsetnik(p, podsetnik))
                {
                    poslatPodsetnik = true;
                    break;
                }
            }
            return poslatPodsetnik;
        }

        private bool DaLiJeIstiPodsetnik(Podsetnik p, Podsetnik podsetnik)
        {
            return  p.Naslov.Equals(podsetnik.Naslov) 
                    && p.Sadrzaj.Equals(podsetnik.Sadrzaj)
                    && p.JmbgKorisnika.Equals(podsetnik.JmbgKorisnika)
                    && p.VrstaPodsetnika== VrstaPodsetnika.UzimanjeLeka;
        }

        public void PosaljiAnketuOLekaru(string JmbgPacijenta, string JmbgLekara)
        {
            Pacijent pacijent = PacijentServis.GetByJmbg(JmbgPacijenta);
            Lekar lekar = LekarServis.GetByJmbg(JmbgLekara);
            AnketeServis.GetAnketaOLekaru(JmbgLekara);
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

        public int NabaviNovePodsetnike(string jmbg)
        {
            int brojNovihPodsetnika = 0;
            brojNovihPodsetnika += NabaviPodsetnikeOUzimanjuLeka(jmbg);
            brojNovihPodsetnika += NabaviKorisnickePodsetnike(jmbg);
            return brojNovihPodsetnika;
        }

        private int NabaviKorisnickePodsetnike(string jmbg)
        {
            int brojNovihPodsetnika = 0;
            List<Podsetnik> podsetnici = SkladisteZaPodsetnike.GetPodsetniciByJmbg(jmbg);
            for (int i = 0; i < podsetnici.Count; i++)
            {
                if (!podsetnici[i].Ubaceno && podsetnici[i].VremeObavestenja <= DateTime.Now)
                {
                    brojNovihPodsetnika++;
                    podsetnici[i].Ubaceno = true;
                }
            }
            SkladisteZaPodsetnike.SaveAll(podsetnici);
            return brojNovihPodsetnika;
        }

        private int NabaviPodsetnikeOUzimanjuLeka(string jmbg)
        {
            int brojNovihPodsetnika = 0;
            List<Recept> recepti = PacijentServis.DobaviReceptePacijenta(jmbg);
            List<DateTime> terminiUzimanja = new List<DateTime>();
            if (recepti.Count > 0)
            {
                foreach (Recept recept in recepti)
                {
                    DateTime poslednjiDanUzimanja = (recept.DatumIzdavanja.AddDays(recept.BrojDana)).Date;
                    if (poslednjiDanUzimanja > DateTime.Today)
                    {
                        TimeSpan satVremena = new TimeSpan(1, 1, 0);
                        TimeSpan nula = new TimeSpan(0, 0, 0);
                        foreach (int i in recept.TerminiUzimanjaLeka)
                        {
                            if ((DateTime.Today.AddHours(i) - DateTime.Now) < satVremena
                                && (DateTime.Today.AddHours(i) - DateTime.Now) > nula)
                            {
                                if (NapraviPodsetnikZaUzimanjeLeka(jmbg, recept, i))
                                    brojNovihPodsetnika++;
                            }
                        }
                    }
                }
            }
            return brojNovihPodsetnika;
        }

        internal void PosaljiKvartalnuAnketu()
        {
            AnketeServis.GetKvartalnaAnketa(DateTime.Today);
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

        public List<Podsetnik> DobaviAktuelnePodsetnike(string jmbgPacijenta)
        {
           List<Podsetnik> podsetniciKorisnika = SkladisteZaPodsetnike.GetPodsetniciByJmbg(jmbgPacijenta);
            List<Podsetnik> uzimanjeTerapijePodsetnik = new List<Podsetnik>();
            foreach (Podsetnik p in podsetniciKorisnika)
            {
                if(p.VrstaPodsetnika == VrstaPodsetnika.UzimanjeLeka)
                {
                    uzimanjeTerapijePodsetnik.Add(p);
                }
                if(p.VrstaPodsetnika == VrstaPodsetnika.Korisnicki && p.Ubaceno)
                {
                    uzimanjeTerapijePodsetnik.Add(p);
                }
            }
            return uzimanjeTerapijePodsetnik;
        }


    }
}