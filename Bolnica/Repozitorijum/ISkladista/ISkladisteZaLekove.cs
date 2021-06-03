using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaLekove : ISkladiste<Lek>
    {
        Lek GetById(int id);
    }
}