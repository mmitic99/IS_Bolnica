using Bolnica.DTOs;
using Bolnica.Servis;
using Bolnica.view;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Windows;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;

namespace Servis
{
    public class ObavestenjaServis
    {

        public static ObavestenjaServis instance;
        private ISkladisteZaObavestenja SkladisteZaObavestenja;
        private PacijentServis PacijentServis;
        private AnketeServis AnketeServis;
        private LekarServis LekarServis;

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
            this.PacijentServis = new PacijentServis();
            this.SkladisteZaObavestenja = new SkladisteZaObavestenjaXml();
            this.AnketeServis = new AnketeServis();
            this.LekarServis = new LekarServis();
        }

        public List<Obavestenje> GetAll
            ()
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

            return obavestenja;
        }

        public List<Obavestenje> GetPodsetnici(String jmbg)
        {
            return SkladisteZaObavestenja.GetPodsetniciByJmbg(jmbg);
        }

        public bool NapraviPodsetnikZaUzimanjeLeka(string jmbgPacijenta, Recept r, int hours)
        {
            Pacijent pacijent = PacijentServis.GetByJmbg(jmbgPacijenta);
            Obavestenje obavestenje = new Obavestenje()
            {
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = jmbgPacijenta,
                Podsetnik = true,
                Naslov = "Podsetnik o uzimanju leka -" + r.ImeLeka,
                Sadrzaj = "Poštovani/a " + pacijent.Ime + " podsećamo vas da danas u " + (DateTime.Today.AddHours(hours)).ToString("HH:mm") + 
                " treba da uzmete Vaš lek. Prijatan dan Vam želi ,,Zdravo bolnica"
            };
            return PosaljiPodsetnik(obavestenje);
            }

        private bool PosaljiPodsetnik(Obavestenje obavestenje)
        {
            if (DaLiJePodsetnikPoslat(obavestenje)) return false;
            SkladisteZaObavestenja.Save(obavestenje);
            return true;
    
        }

        private bool DaLiJePodsetnikPoslat(Obavestenje obavestenje)
        {
            List<Obavestenje> obavestenja = SkladisteZaObavestenja.GetAll();
            foreach(Obavestenje o in obavestenja)
            {
                if (DaLiJeIstiPodsetnik(o, obavestenje)) return true;
            }
            return false;
        }

        private bool DaLiJeIstiPodsetnik(Obavestenje o, Obavestenje obavestenje)
        {
            return  o.Naslov.Equals(obavestenje.Naslov) 
                    && o.Sadrzaj.Equals(obavestenje.Sadrzaj)
                    && o.JmbgKorisnika.Equals(obavestenje.JmbgKorisnika);
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
                Podsetnik = false,
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
                Podsetnik=false,
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
            if (month == 1) return "januarska";
            else if (month == 2) return "februarska";
            else if (month == 3) return "martovska";
            else if (month == 4) return "aprilska";
            else if (month == 5) return "majska";
            else if (month == 6) return "junska";
            else if (month == 7) return "julska";
            else if (month == 8) return "avgustovska";
            else if (month == 9) return "septembarska";
            else if (month == 10) return "oktobarska";
            else if (month == 11) return "novembarska";
            else return "decembarska";
        }

        public bool IzmeniObavestenje(Obavestenje staroObavestenje, Obavestenje novoObavestenje)
        {
            ObrisiObavestenje(staroObavestenje);
            SkladisteZaObavestenja.Save(novoObavestenje);
            return true;
        }

        public bool ObrisiObavestenje(Obavestenje obavestenje)
        {
            List<Obavestenje> obavestenja = SkladisteZaObavestenja.GetAll();
            foreach (Obavestenje obavestenje1 in obavestenja)
            {
                if (obavestenje1.Equals(obavestenje))
                {
                    obavestenja.Remove(obavestenje);
                    SkladisteZaObavestenja.SaveAll(obavestenja);
                    return true;
                }
            }
            return false;
        }

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            return SkladisteZaObavestenja.GetPodsetniciByJmbg(jmbgPacijenta);
        }


    }
}