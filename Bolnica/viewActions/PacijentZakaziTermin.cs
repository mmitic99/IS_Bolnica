using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class PacijentZakaziTermin
    {
        public String JmbgPacijenta {get; set;}

        public PacijentZakaziTermin(Pacijent pacijent)
        {
            JmbgPacijenta = pacijent.Jmbg;
        }
    }
}
