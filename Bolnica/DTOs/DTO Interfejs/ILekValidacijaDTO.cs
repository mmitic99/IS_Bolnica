namespace Bolnica.DTOs
{
    public interface ILekValidacijaDTO
    {
        string JacinaLeka { get; set; }
        int KlasaLeka { get; set; }
        string KolicinaLeka { get; set; }
        string NazivLeka { get; set; }
        string SastavLeka { get; set; }
        int VrstaLeka { get; set; }
        string ZamenskiLek { get; set; }
    }
}