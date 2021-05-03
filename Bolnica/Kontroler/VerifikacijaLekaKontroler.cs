using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void PosaljiVerifikacijuLeka() { }
        public void ObrisiVerifikacijuLeka() { }
    }
}
