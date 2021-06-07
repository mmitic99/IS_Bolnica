using Bolnica.Repozitorijum;
using Model;

namespace Repozitorijum
{
    public interface ISkladisteZaProstorije : ISkladiste<Prostorija>
    {
        Prostorija GetById(int id);

    }
}