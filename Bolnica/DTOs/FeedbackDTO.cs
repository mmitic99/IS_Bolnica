using System;

namespace Bolnica.DTOs
{
    public class FeedbackDTO
    {
        public string JmbgKorisnika { get; set; }
        public DateTime DatumIVreme { get; set; }
        public string Sadrzaj { get; set; }
        public int Ocena { get; set; }
    }
}