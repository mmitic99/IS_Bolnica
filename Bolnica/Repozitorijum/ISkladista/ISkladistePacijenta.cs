using System;
using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladistePacijenta : ISkladiste<Pacijent>
    {
        Pacijent GetByJmbg(String jmbg);
    }
}
