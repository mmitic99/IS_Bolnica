using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum;
using Bolnica.model;

namespace Bolnica.Servis
{
    public class VerifikacijaLekaServis
    {
        private static VerifikacijaLekaServis instance = null;
        public static VerifikacijaLekaServis GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaServis();
            }
            return instance;
        }

        public void PosaljiVerifikacijuLeka(VerifikacijaLekaServis verifikacijaLeka)
        {
            List<VerifikacijaLeka> SveVerifikacijeLeka = SkladisteZaVerifikacijuLeka.GetInstance().GetAll();
        }
        public void ObrisiVerifikacijuLeka() { }
    }
}
