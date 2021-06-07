using System;
using Bolnica.Repozitorijum;
using Model;

namespace Repozitorijum
{
    public interface ISkladisteZaLekara : ISkladiste<Lekar>
    {
        Lekar getByJmbg(String jmbg);
        void IzmeniLekara(string jmbgLekara, Lekar lekar);

    }
}