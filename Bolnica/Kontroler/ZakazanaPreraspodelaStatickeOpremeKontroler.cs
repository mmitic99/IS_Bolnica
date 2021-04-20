using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class ZakazanaPreraspodelaStatickeOpremeKontroler
    {
        private static ZakazanaPreraspodelaStatickeOpremeKontroler instance = null;

        public static ZakazanaPreraspodelaStatickeOpremeKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new ZakazanaPreraspodelaStatickeOpremeKontroler();
            }
            return instance;
        }

        public void ZakaziPreraspodeluStatickeOpreme(ZakazanaPreraspodelaStatickeOpreme preraspodela)
        {
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().ZakaziPreraspodeluStatickeOpreme(preraspodela);
        }

        public void OtkaziPreraspodeluStatickeOpreme(int index)
        {
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().OtkaziPreraspodeluStatickeOpreme(index);
        }

        public bool ProveraValidnostiPreraspodeleOpreme(String trajanje)
        {
            return ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().ProveraValidnostiPreraspodeleOpreme(trajanje);
        }
    }
}


