using System;
using System.Collections.Generic;
using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;

namespace Repozitorijum
{
    public interface ISkladisteZaTermine : ISkladiste<Termin>
    {
        List<TerminPacijentLekarDTO> GetBuduciTerminPacLekar();
        List<Termin> GetByJmbg(String jmbg);
        List<Termin> GetByDateForLekar(DateTime datum, String jmbg);
        List<Termin> GetByJmbgLekar(String jmbg);
        Termin GetById(String id);
        void RemoveById(String id);

    }
}