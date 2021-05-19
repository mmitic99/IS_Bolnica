using Bolnica.Servis;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontroler
{
    class SpecijalizacijaKontroler
    {
        private SpecijalizacijaServis specijalizacijaServis;

        public SpecijalizacijaKontroler()
        {
            specijalizacijaServis = new SpecijalizacijaServis();
        }

        public List<string> GetAll()
        {
            List<string> specijalizacije = new List<string>();
            foreach (Specijalizacija specijalizacija in specijalizacijaServis.GetAll())
            {
                specijalizacije.Add(specijalizacija.VrstaSpecijalizacije);
            }

            return specijalizacije;
        }

        public bool Save(Specijalizacija specijalizacija)
        {
            return specijalizacijaServis.Save(specijalizacija);
        }

        public void SaveAll(List<Specijalizacija> specijalizacije)
        {
            specijalizacijaServis.SaveAll(specijalizacije);
        }
    }
}
