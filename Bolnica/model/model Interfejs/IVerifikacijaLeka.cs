using System;

namespace Bolnica.model
{
    public interface IVerifikacijaLeka
    {
        string IdVerifikacijeLeka { get; set; }
        string JmbgPosiljaoca { get; set; }
        string JmbgPrimaoca { get; set; }
        string Napomena { get; set; }
        string Naslov { get; set; }
        string Sadrzaj { get; set; }
        DateTime VremeSlanjaZahteva { get; set; }
    }
}