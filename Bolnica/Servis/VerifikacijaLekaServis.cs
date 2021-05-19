using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum;
using Bolnica.model;
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
        public void ObrisiVerifikacijuLeka() { }
        public List<VerifikacijaLeka> GetAll()
        {
            return skladisteZaVerifikacijuLeka.GetAll();
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
