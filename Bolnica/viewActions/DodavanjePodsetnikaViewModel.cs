using Bolnica.model;
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
        public Beleska beleska { get; set; }

        public DodavanjePodsetnikaViewModel(Pacijent pacijent, model.Beleska beleska = null)
        {
            this.pacijent = pacijent;
            this.beleska = beleska;
            if(beleska == null)
            {
                beleska = new model.Beleska()
                {
                    Naslov = "",
                    Opis = ""

                };
            }
        }
    }
}
