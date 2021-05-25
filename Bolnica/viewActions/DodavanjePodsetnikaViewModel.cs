using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class DodavanjePodsetnikaViewModel
    {
        public Pacijent pacijent { get; set; }

        public DodavanjePodsetnikaViewModel(Pacijent pacijent)
        {
            this.pacijent = pacijent;
        }
    }
}
