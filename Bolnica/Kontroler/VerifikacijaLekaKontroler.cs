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

        public void ObrisiVerifikacijuLeka() { }

        public List<VerifikacijaLeka> GetAll()
        {
            return VerifikacijaLekaServis.GetInstance().GetAll();
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            VerifikacijaLekaServis.GetInstance().Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            VerifikacijaLekaServis.GetInstance().SaveAll(verifikacije);
        }

        public void NamapirajSadrzajVerifikacije(int index)
        {
            List<VerifikacijaLeka> SveVerifikacijeLekova = VerifikacijaLekaServis.GetInstance().GetAll();
            UpravnikWindow.GetInstance().Sadrzaj.Text = SveVerifikacijeLekova[index].Sadrzaj;
        }
    }
}
