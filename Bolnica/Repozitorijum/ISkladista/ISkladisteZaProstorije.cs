using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteZaProstorije : ISkladiste<Prostorija>
    {
        Prostorija GetById(int id);

    }
}