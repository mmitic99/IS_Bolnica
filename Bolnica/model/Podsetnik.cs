using Bolnica.model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class Podsetnik : Notifikacija
    {
        public bool Ubaceno { get; set; }
        public VrstaPodsetnika VrstaPodsetnika { get; set; }

    }
}
