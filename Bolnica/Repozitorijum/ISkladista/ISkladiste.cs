using System.Collections.Generic;

namespace Bolnica.Repozitorijum.ISkladista
{
    public interface ISkladiste<T>
    {
        List<T> GetAll();
        void Save(T entitet);
        void SaveAll(List<T> entitet);
    }
}
