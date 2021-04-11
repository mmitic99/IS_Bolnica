using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class TerminPacijentLekar
    {
        public Termin termin { get; set; }
        public Pacijent pacijent { get; set; }
        public Lekar lekar { get; set; }
    }
}
