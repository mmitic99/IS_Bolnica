using System;
using System.Collections.Generic;
using Bolnica.DTOs;
using Bolnica.model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaTermine : ISkladiste<Termin>
    {
        List<TerminPacijentLekarDTO> GetBuduciTerminPacLekar();
        List<Termin> GetByJmbgPacijenta(String jmbg);
        List<Termin> GetByDateForLekar(DateTime datum, String jmbg);
        List<Termin> GetByJmbgLekar(String jmbg);
        Termin GetById(String id);
        void RemoveById(String id);

    }
}