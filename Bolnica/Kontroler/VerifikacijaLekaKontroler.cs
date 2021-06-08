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

        public void ObrisiVerifikacijuLeka(Object verifikacija)
        {
            VerifikacijaLekaDTO verifikacijaDTO = (VerifikacijaLekaDTO)verifikacija;
            VerifikacijaLekaServis.GetInstance().ObrisiVerifikacijuLeka(verifikacijaDTO.IdVerifikacijeLeka);
        }

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

        public void Save(VerifikacijaLeka verifikacija)
        {
            VerifikacijaLekaServis.GetInstance().Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            VerifikacijaLekaServis.GetInstance().SaveAll(verifikacije);
        }

    }
}
