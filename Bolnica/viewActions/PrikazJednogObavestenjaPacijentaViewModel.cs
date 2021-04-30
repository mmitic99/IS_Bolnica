using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    class PrikazJednogObavestenjaPacijentaViewModel
    {

        public Obavestenje obavestenje { get; set; }

        public PrikazJednogObavestenjaPacijentaViewModel()
        {

        }

        public PrikazJednogObavestenjaPacijentaViewModel(Obavestenje o)
        {
            obavestenje = o;
        }

        public PrikazJednogObavestenjaPacijentaViewModel(object selectedItem)
        {
            obavestenje = (Obavestenje)selectedItem;
        }
    }
}
