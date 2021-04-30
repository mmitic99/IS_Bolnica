using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis
{
    class SpecijalizacijaServis
    {
        private SkladisteZaSpecijalizaciju skladisteZaSpecijalizaciju;

        public SpecijalizacijaServis()
        {
            skladisteZaSpecijalizaciju = new SkladisteZaSpecijalizaciju();
        }
        public List<Specijalizacija> GetAll()
        {
            return skladisteZaSpecijalizaciju.GetAll();
        }

        public bool Save(Specijalizacija specijalizacija)
        {
            skladisteZaSpecijalizaciju.Save(specijalizacija);
            return true;
        }

        public void SaveAll(List<Specijalizacija> specijalizacije)
        {
            skladisteZaSpecijalizaciju.SaveAll(specijalizacije);
        }
    }
}
