using Bolnica.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Repozitorijum
{
    public interface ISkladisteBolnickihLecenja : ISkladiste<BolnickoLecenje>
    {
        void RemoveByID(string jmbgPacijenta);
        
    }
}
