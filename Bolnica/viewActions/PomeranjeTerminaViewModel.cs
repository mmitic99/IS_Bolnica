using Bolnica.model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class PomeranjeTerminaViewModel
    {
        public Pacijent Pacijent { get; }
        public Termin TerminZaPomeranje { get; set; }

        public PomeranjeTerminaViewModel(Pacijent pacijent)
        {
            this.Pacijent = pacijent;
        }

        public void SelektovanJeTermin(object selectedItem)
        {
            this.TerminZaPomeranje = (Termin)selectedItem;
        }
    }
}
