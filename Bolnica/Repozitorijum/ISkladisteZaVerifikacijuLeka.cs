using System;
using System.Collections.Generic;
using Bolnica.model;

namespace Bolnica.Repozitorijum
{
    public interface ISkladisteZaVerifikacijuLeka : ISkladiste<VerifikacijaLeka>
    {
        List<VerifikacijaLeka> GetObavestenjaByJmbg(String jmbg);
    }
}