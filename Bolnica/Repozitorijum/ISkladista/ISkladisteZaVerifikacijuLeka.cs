using System;
using System.Collections.Generic;
using Bolnica.model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaVerifikacijuLeka : ISkladiste<VerifikacijaLeka>
    {
        List<VerifikacijaLeka> GetObavestenjaByJmbg(String jmbg);
    }
}