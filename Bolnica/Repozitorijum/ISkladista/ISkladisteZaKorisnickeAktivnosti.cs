using System;
using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaKorisnickeAktivnosti : ISkladiste<KorisnickeAktivnostiNaAplikaciji>
    {
        KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika);
    }
}