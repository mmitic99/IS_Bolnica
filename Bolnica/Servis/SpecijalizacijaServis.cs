using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    class SpecijalizacijaServis
    {
        private ISkladisteZaSpecijalizaciju skladisteZaSpecijalizaciju;

        public SpecijalizacijaServis()
        {
            skladisteZaSpecijalizaciju = new SkladisteZaSpecijalizacijuXml();
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
