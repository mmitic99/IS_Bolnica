using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class PomocTerminiViewModel
    {
        RelayCommand nazad { get; set; }
        public PomocTerminiViewModel(RelayCommand nazad)
        {
            this.nazad = nazad;
        }
    }
}
