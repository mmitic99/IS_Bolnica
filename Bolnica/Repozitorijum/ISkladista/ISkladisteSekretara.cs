using Model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteSekretara : ISkladiste<Sekretar>
    {
        Sekretar GetByJmbg(string jmbg);
    }
}