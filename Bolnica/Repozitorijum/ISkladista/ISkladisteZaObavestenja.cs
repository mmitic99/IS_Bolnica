using System;
using System.Collections.Generic;
using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaObavestenja : ISkladiste<Obavestenje>
    {
        List<Obavestenje> GetObavestenjaByJmbg(String korisnickoIme);

    }
}