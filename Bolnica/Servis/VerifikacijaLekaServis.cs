using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    public class VerifikacijaLekaServis
    {
        private static VerifikacijaLekaServis instance = null;
        public ISkladisteZaVerifikacijuLeka skladisteZaVerifikacijuLeka;
        public static VerifikacijaLekaServis GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaServis();
            }
            return instance;
        }

        public VerifikacijaLekaServis()
        {
            skladisteZaVerifikacijuLeka = new SkladisteZaVerifikacijuLekaXml();
        }

        public void PosaljiVerifikacijuLeka(VerifikacijaLeka verifikacijaLeka)
        {
            List<VerifikacijaLeka> SveVerifikacijeLeka = skladisteZaVerifikacijuLeka.GetAll();
            SveVerifikacijeLeka.Add(verifikacijaLeka);
            skladisteZaVerifikacijuLeka.SaveAll(SveVerifikacijeLeka);
        }

        public void ObrisiVerifikacijuLeka(String idVerifikacije)
        {
            List<VerifikacijaLeka> SveVerifikacije = skladisteZaVerifikacijuLeka.GetAll();
            foreach (VerifikacijaLeka verifikacija in SveVerifikacije)
            {
                if (idVerifikacije.Equals(verifikacija.IdVerifikacijeLeka))
                {
                    SveVerifikacije.Remove(verifikacija);
                    break;
                }
            }
            skladisteZaVerifikacijuLeka.SaveAll(SveVerifikacije);
        }

        public List<VerifikacijaLeka> GetAll()
        {
            return skladisteZaVerifikacijuLeka.GetAll();
        }

        public List<VerifikacijaLeka> GetObavestenjaByJmbg(String jmbg)
        {
            return skladisteZaVerifikacijuLeka.GetObavestenjaByJmbg(jmbg);
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            skladisteZaVerifikacijuLeka.Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            skladisteZaVerifikacijuLeka.SaveAll(verifikacije);
        }
    }
}
