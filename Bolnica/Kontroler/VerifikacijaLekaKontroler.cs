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
using Bolnica.Repozitorijum.Factory.VerifikacijaLekaFactory;

namespace Bolnica.Kontroler
{
    public class VerifikacijaLekaKontroler : IVerifikacijaLekaKontroler
    {   
        private IVerifikacijaLekaServis verifikacijaLekaServis;
        public  VerifikacijaLekaKontroler()
        {
            verifikacijaLekaServis = new VerifikacijaLekaServis();
        }



        public void PosaljiVerifikacijuLeka(IVerifikacijaLekaDTO verifikacijaLekaDTO)
        {
            IVerifikacijaLeka verifikacijaLeka = VerifikacijaLekaFactory.CreateVerifikacijaLeka();
            verifikacijaLeka.IdVerifikacijeLeka = verifikacijaLekaDTO.VremeSlanjaZahteva.ToString("dd.MM.yyyy HH:mm:ss") + verifikacijaLekaDTO.JmbgPosiljaoca + verifikacijaLekaDTO.JmbgPrimaoca;
            verifikacijaLeka.JmbgPosiljaoca = verifikacijaLekaDTO.JmbgPosiljaoca;
            verifikacijaLeka.JmbgPrimaoca = verifikacijaLekaDTO.JmbgPrimaoca;
            verifikacijaLeka.Napomena = verifikacijaLekaDTO.Napomena;
            verifikacijaLeka.Naslov = verifikacijaLekaDTO.Naslov;
            verifikacijaLeka.Sadrzaj = verifikacijaLekaDTO.Sadrzaj;
            verifikacijaLeka.VremeSlanjaZahteva = verifikacijaLekaDTO.VremeSlanjaZahteva;
            verifikacijaLekaServis.PosaljiVerifikacijuLeka(verifikacijaLeka);
        }

        public void ObrisiVerifikacijuLeka(Object verifikacija)
        {
            IVerifikacijaLekaDTO verifikacijaDTO = (IVerifikacijaLekaDTO)verifikacija;
            verifikacijaLekaServis.ObrisiVerifikacijuLeka(verifikacijaDTO.IdVerifikacijeLeka);
        }

        public List<IVerifikacijaLekaDTO> GetAll()
        {
            List<IVerifikacijaLekaDTO> verifikacije = new List<IVerifikacijaLekaDTO>();
            foreach (IVerifikacijaLeka verifikacija in verifikacijaLekaServis.GetAll())
            {
                IVerifikacijaLekaDTO verifikacijaDTO = VerifikacijaLekaFactory.CreateVerifikacijaLekaDTO();
                verifikacijaDTO.IdVerifikacijeLeka = verifikacija.VremeSlanjaZahteva.ToString("dd.MM.yyyy HH:mm:ss") + verifikacija.JmbgPosiljaoca + verifikacija.JmbgPrimaoca;
                verifikacijaDTO.VremeSlanjaZahteva = verifikacija.VremeSlanjaZahteva;
                verifikacijaDTO.Naslov = verifikacija.Naslov;
                verifikacijaDTO.Sadrzaj = verifikacija.Sadrzaj;
                verifikacijaDTO.JmbgPrimaoca = verifikacija.JmbgPrimaoca;
                verifikacijaDTO.JmbgPosiljaoca = verifikacija.JmbgPosiljaoca;
                verifikacijaDTO.Napomena = verifikacija.Napomena;
                verifikacije.Add(verifikacijaDTO);
            }
            return verifikacije;
        }

        public List<IVerifikacijaLekaDTO> GetObavestenjaByJmbg(String jmbg)
        {
            List<IVerifikacijaLekaDTO> verifikacije = new List<IVerifikacijaLekaDTO>();
            foreach (IVerifikacijaLeka verifikacija in verifikacijaLekaServis.GetObavestenjaByJmbg(jmbg))
            {
                IVerifikacijaLekaDTO verifikacijaDTO = VerifikacijaLekaFactory.CreateVerifikacijaLekaDTO();
                verifikacijaDTO.IdVerifikacijeLeka = verifikacija.VremeSlanjaZahteva.ToString("dd.MM.yyyy HH:mm:ss") + verifikacija.JmbgPosiljaoca + verifikacija.JmbgPrimaoca;
                    verifikacijaDTO.VremeSlanjaZahteva = verifikacija.VremeSlanjaZahteva;
                verifikacijaDTO.Naslov = verifikacija.Naslov;
                verifikacijaDTO.Sadrzaj = verifikacija.Sadrzaj;
                verifikacijaDTO.JmbgPrimaoca = verifikacija.JmbgPrimaoca;
                verifikacijaDTO.JmbgPosiljaoca = verifikacija.JmbgPosiljaoca;
                verifikacijaDTO.Napomena = verifikacija.Napomena;
                    verifikacije.Add(verifikacijaDTO);
            }
            return verifikacije;
        }

        public void Save(IVerifikacijaLeka verifikacija)
        {
            verifikacijaLekaServis.Save(verifikacija);
        }

        public void SaveAll(List<IVerifikacijaLeka> verifikacije)
        {
            verifikacijaLekaServis.SaveAll(verifikacije);
        }

    }
}
