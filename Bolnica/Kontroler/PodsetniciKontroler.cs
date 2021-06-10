using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    class PodsetniciKontroler
    {
        public static PodsetniciKontroler instance = null;
        public PodsetniciServis podsetniciServis;

        public static PodsetniciKontroler getInstance()
        {
            if (instance == null)
            {
                return new PodsetniciKontroler();
            }
            else
            {
                return instance;
            }
        }

        public PodsetniciKontroler()
        {
            instance = this;
            podsetniciServis = new PodsetniciServis();
        }


        public List<Podsetnik> GetPodsetnici(string jmbg)
        {
            return podsetniciServis.GetPodsetnici(jmbg);
        }

        public bool NapraviPodsetnikZaUzimanjeLeka(string jmbgPacijenta, Recept r, int hours)
        {
            return podsetniciServis.NapraviPodsetnikZaUzimanjeLeka(jmbgPacijenta, r, hours);
        }

        public bool NapraviKorisnickiPodsetnik(KorisnickiPodsetnikFrontDTO podsetnikDTO)
        {
            KorisnickiPodsetnikKlasifikovnoDTO podsetnikKlasifikovano = KlasifikujParametreKorisnickogPodsetnika(podsetnikDTO);
            return podsetniciServis.NapravikorisnickePodsetnike(podsetnikKlasifikovano);
        }

        private KorisnickiPodsetnikKlasifikovnoDTO KlasifikujParametreKorisnickogPodsetnika(KorisnickiPodsetnikFrontDTO podsetnikDTO)
        {
            KorisnickiPodsetnikKlasifikovnoDTO podsetnikKlasifikovnoDTO = new KorisnickiPodsetnikKlasifikovnoDTO()
            {
                JmbgKorisnika = ((Pacijent)podsetnikDTO.JmbgKorisnika).Jmbg,
                Datumi = new List<DateTime>((IEnumerable<DateTime>)podsetnikDTO.Datumi),
                Naslov = (String)podsetnikDTO.Naslov,
                Sadrzaj = (String)podsetnikDTO.Sadrzaj
            };
            int minut = (int)podsetnikDTO.Minut;
            int sat = (int)podsetnikDTO.Sat;

            TimeSpan vreme = new TimeSpan(sat, minut, 0);
            for (int i = 0; i < podsetnikKlasifikovnoDTO.Datumi.Count; i++)
            {
                podsetnikKlasifikovnoDTO.Datumi[i] = podsetnikKlasifikovnoDTO.Datumi[i].Date.Add(vreme);
            }
            return podsetnikKlasifikovnoDTO;
        }

        public List<Podsetnik> DobaviPodsetnikeZaTerapiju(Pacijent pacijent)
        {
            return podsetniciServis.DobaviAktuelnePodsetnike(pacijent.Jmbg);
        }

        public int nabaviNovePodsetnike(Pacijent pacijent)
        {
            return podsetniciServis.NabaviNovePodsetnike(pacijent.Jmbg);
        }
    }
}
