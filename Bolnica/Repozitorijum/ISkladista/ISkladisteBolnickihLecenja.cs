using Bolnica.model;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladisteBolnickihLecenja : ISkladiste<BolnickoLecenje>
    {
        void RemoveById(string jmbgPacijenta);
        
    }
}
