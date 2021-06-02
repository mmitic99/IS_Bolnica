using Model;

namespace Bolnica.Repozitorijum
{
    public interface ISkladisteZaLekove : ISkladiste<Lek>
    {
        Lek GetById(int id);
    }
}