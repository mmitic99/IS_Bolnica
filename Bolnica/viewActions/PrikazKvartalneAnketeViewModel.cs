using Bolnica.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    class PrikazKvartalneAnketeViewModel
    {
        public KvartalnaAnketa anketa { get; set; }
        public PrikazKvartalneAnketeViewModel()
        {
        }

        public PrikazKvartalneAnketeViewModel(KvartalnaAnketa kvartalnaAnketa)
        {
            anketa = kvartalnaAnketa;
        }
    }
}
