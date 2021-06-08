using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model;
using Bolnica.Servis;
using Bolnica.view.UpravnikView;
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.Kontroler
{
    public class VerifikacijaLekaKontroler
    {
        private static VerifikacijaLekaKontroler instance = null;
        public static VerifikacijaLekaKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaKontroler();
            }
            return instance;
        }


        public void PosaljiVerifikacijuLeka(VerifikacijaLekaDTO verifikacijaLekaDTO)
        {
            VerifikacijaLeka verifikacijaLeka = new VerifikacijaLeka
                                                    (
                                                    verifikacijaLekaDTO.VremeSlanjaZahteva, 
                                                    verifikacijaLekaDTO.Naslov,
                                                    verifikacijaLekaDTO.Sadrzaj,
                                                    verifikacijaLekaDTO.JmbgPosiljaoca, 
                                                    verifikacijaLekaDTO.JmbgPrimaoca,
                                                    verifikacijaLekaDTO.Napomena
                                                    );
            VerifikacijaLekaServis.GetInstance().PosaljiVerifikacijuLeka(verifikacijaLeka);
        }

        public void ObrisiVerifikacijuLeka() { }

        public List<VerifikacijaLekaDTO> GetAll()
        {
            List<VerifikacijaLekaDTO> verifikacije = new List<VerifikacijaLekaDTO>();
            foreach (VerifikacijaLeka verifikacija in VerifikacijaLekaServis.GetInstance().GetAll())
            {
                verifikacije.Add(new VerifikacijaLekaDTO()
                {
                    IdVerifikacijeLeka = verifikacija.VremeSlanjaZahteva.ToString() + verifikacija.JmbgPosiljaoca + verifikacija.JmbgPrimaoca,
                    VremeSlanjaZahteva = verifikacija.VremeSlanjaZahteva,
                    Naslov = verifikacija.Naslov,
                    Sadrzaj = verifikacija.Sadrzaj,
                    JmbgPrimaoca = verifikacija.JmbgPrimaoca,
                    JmbgPosiljaoca = verifikacija.JmbgPosiljaoca,
                    Napomena = verifikacija.Napomena
                });
            }
            return verifikacije;
        }

        public List<VerifikacijaLekaDTO> GetObavestenjaByJmbg(String jmbg)
        {
            List<VerifikacijaLekaDTO> verifikacije = new List<VerifikacijaLekaDTO>();
            foreach (VerifikacijaLeka verifikacija in VerifikacijaLekaServis.GetInstance().GetObavestenjaByJmbg(jmbg))
            {
                verifikacije.Add(new VerifikacijaLekaDTO()
                {
                    IdVerifikacijeLeka = verifikacija.VremeSlanjaZahteva.ToString("dd.MM.yyyy HH:mm:ss") + verifikacija.JmbgPosiljaoca + verifikacija.JmbgPrimaoca,
                    VremeSlanjaZahteva = verifikacija.VremeSlanjaZahteva,
                    Naslov = verifikacija.Naslov,
                    Sadrzaj = verifikacija.Sadrzaj,
                    JmbgPrimaoca = verifikacija.JmbgPrimaoca,
                    JmbgPosiljaoca = verifikacija.JmbgPosiljaoca,
                    Napomena = verifikacija.Napomena
                });
            }
            return verifikacije;
        }

        public List<VerifikacijePrikazDTO> GetVerifikacijePrikaz(String jmbg)
        {
            List<VerifikacijaLekaDTO> verifikacije = GetObavestenjaByJmbg(jmbg);
            List<VerifikacijePrikazDTO> verifikacijePrikaz = new List<VerifikacijePrikazDTO>();
            foreach (VerifikacijaLekaDTO v in verifikacije)
            {
                VerifikacijePrikazDTO vp = new VerifikacijePrikazDTO();
                vp.VremeSlanjaZahteva = v.VremeSlanjaZahteva;
                vp.Naslov = v.Naslov;
                vp.ImeLekara = LekarKontroler.getInstance().GetByJmbg(v.JmbgPosiljaoca).FullName;
                vp.Napomena = v.Napomena;
                vp.ID = v.IdVerifikacijeLeka;
                vp.Sadrzaj = v.Sadrzaj;
                verifikacijePrikaz.Add(vp);
            }
            return verifikacijePrikaz;
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            VerifikacijaLekaServis.GetInstance().Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            VerifikacijaLekaServis.GetInstance().SaveAll(verifikacije);
        }

        public void ObrisiVerifikacijuLeka(Object verifikacija)
        {
            VerifikacijePrikazDTO verifikacijaDTO = (VerifikacijePrikazDTO)verifikacija;
            VerifikacijaLekaServis.GetInstance().ObrisiVerifikacijuLeka(verifikacijaDTO.ID);
        }
    }
}
