/***********************************************************************
 * Module:  SkladisteZaKorisnickeAktivnostiXml.cs
 * Author:  PC
 * Purpose: Definition of the Class Repozitorijum.SkladisteZaKorisnickeAktivnostiXml
 ***********************************************************************/

using System;
using Bolnica.Repozitorijum;
using Model;

namespace Repozitorijum
{
    public interface ISkladisteZaKorisnickeAktivnosti : ISkladiste<KorisnickeAktivnostiNaAplikaciji>
    {
        KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika);
    }
}