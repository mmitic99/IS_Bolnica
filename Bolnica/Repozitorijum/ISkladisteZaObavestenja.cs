using System;
using System.Collections.Generic;
using Bolnica.Repozitorijum;
using Model;

namespace Repozitorijum
{
    public interface ISkladisteZaObavestenja : ISkladiste<Obavestenje>
    {
        List<Obavestenje> GetObavestenjaByJmbg(String korisnickoIme);

    }
}