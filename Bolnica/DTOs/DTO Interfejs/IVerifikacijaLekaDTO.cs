using System;

namespace Bolnica.DTOs
{
    public interface IVerifikacijaLekaDTO
    {
        string IdVerifikacijeLeka { get; set; }
        string ImeUpravnika { get; set; }
        string JmbgPosiljaoca { get; set; }
        string JmbgPrimaoca { get; set; }
        string Napomena { get; set; }
        string Naslov { get; set; }
        string Sadrzaj { get; set; }
        DateTime VremeSlanjaZahteva { get; set; }
    }
}