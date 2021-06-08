using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bolnica.Validacije
{
    public class ValidacijaContext
    {
        private ValidacijaStrategy strategy;
        public ValidacijaContext(ValidacijaStrategy strategy)
        {
            this.strategy = strategy;
        }
        public void IspisiGresku(int idGreske)
        {
            strategy.IspisiPorukuGreske(idGreske);
        }
    }
}
