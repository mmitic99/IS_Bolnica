using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model;
using Bolnica.Servis;

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
        public void PosaljiVerifikacijuLeka(VerifikacijaLeka vL) {
             VerifikacijaLekaServis.GetInstance().PosaljiVerifikacijuLeka(vL);
        }
        public void ObrisiVerifikacijuLeka() { }
    }
}
