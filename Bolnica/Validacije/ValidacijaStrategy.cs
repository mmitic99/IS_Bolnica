﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bolnica.Validacije
{
    public interface ValidacijaStrategy
    {
        void IspisiPorukuGreske(int idGreske);
    }
}
