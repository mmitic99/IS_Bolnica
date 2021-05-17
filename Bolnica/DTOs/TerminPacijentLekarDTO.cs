using Model;

namespace Bolnica.DTOs
{
    public class TerminPacijentLekarDTO
    {
        public Termin termin { get; set; }
        public Pacijent pacijent { get; set; }
        public Lekar lekar { get; set; }
    }
}
