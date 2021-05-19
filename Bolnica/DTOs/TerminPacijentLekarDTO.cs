using Bolnica.model;
using Model;

namespace Bolnica.DTOs
{
    public class TerminPacijentLekarDTO
    {
        public TerminDTO termin { get; set; }
        public PacijentDTO pacijent { get; set; }
        public LekarDTO lekar { get; set; }
    }
}
