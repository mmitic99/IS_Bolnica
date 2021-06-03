using System;
using System.Collections.Generic;
using Bolnica.model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaPodsetnike : ISkladiste<Podsetnik>
    {
        List<Podsetnik> GetPodsetniciByJmbg(String Jmbg);
    }
}
