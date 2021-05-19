using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Repozitorijum
{
    public interface ISkladiste<T>
    {
        List<T> GetAll();
        void Save(T entitet);
        void SaveAll(List<T> entitet);
    }
}
