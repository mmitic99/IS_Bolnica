using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void PosaljiVerifikacijuLeka() { }
        public void ObrisiVerifikacijuLeka() { }
    }
}
