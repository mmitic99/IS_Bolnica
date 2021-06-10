using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.model.Enum;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis
{
    class PodsetniciServis
    {
        private ISkladisteZaPodsetnike SkladisteZaPodsetnike;
        private PacijentServis PacijentServis;
        private LekarServis LekarServis;

        public PodsetniciServis()
        {
            this.PacijentServis = new PacijentServis();
            this.SkladisteZaPodsetnike = new SkladisteZaPodsetnikeXml();
            this.LekarServis = new LekarServis();
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

        public bool NapravikorisnickePodsetnike(KorisnickiPodsetnikKlasifikovnoDTO podsetnikKlasifikovano)
        {
            foreach (DateTime datum in podsetnikKlasifikovano.Datumi)
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
            foreach (Podsetnik p in podsetnici)
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
            return p.Naslov.Equals(podsetnik.Naslov)
                    && p.Sadrzaj.Equals(podsetnik.Sadrzaj)
                    && p.JmbgKorisnika.Equals(podsetnik.JmbgKorisnika)
                    && p.VrstaPodsetnika == VrstaPodsetnika.UzimanjeLeka;
        }

        public int NabaviNovePodsetnike(string jmbg)
        {
            int brojNovihPodsetnika = 0;
            brojNovihPodsetnika += NabaviBrojPodsetnikaOUzimanjuLeka(jmbg);
            brojNovihPodsetnika += NabaviBrojKorisnickihPodsetnika(jmbg);
            return brojNovihPodsetnika;
        }

        private int NabaviBrojKorisnickihPodsetnika(string jmbg)
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

        private int NabaviBrojPodsetnikaOUzimanjuLeka(string jmbg)
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

        public List<Podsetnik> DobaviAktuelnePodsetnike(string jmbgPacijenta)
        {
            List<Podsetnik> podsetniciKorisnika = SkladisteZaPodsetnike.GetPodsetniciByJmbg(jmbgPacijenta);
            List<Podsetnik> uzimanjeTerapijePodsetnik = new List<Podsetnik>();
            foreach (Podsetnik p in podsetniciKorisnika)
            {
                if (p.VrstaPodsetnika == VrstaPodsetnika.UzimanjeLeka)
                {
                    uzimanjeTerapijePodsetnik.Add(p);
                }
                if (p.VrstaPodsetnika == VrstaPodsetnika.Korisnicki && p.Ubaceno)
                {
                    uzimanjeTerapijePodsetnik.Add(p);
                }
            }
            return uzimanjeTerapijePodsetnik;
        }
    }
}
