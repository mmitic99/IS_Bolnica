using Bolnica.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Repozitorijum
{
    public interface ISkladisteZaPodsetnike : ISkladiste<Podsetnik>
    {
        List<Podsetnik> GetPodsetniciByJmbg(String Jmbg);
    }
}
