using System;
using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaLekara : ISkladiste<Lekar>
    {
        Lekar getByJmbg(String jmbg);
        void IzmeniLekara(string jmbgLekara, Lekar lekar);

    }
}