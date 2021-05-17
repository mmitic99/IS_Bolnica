using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;

namespace Bolnica.viewActions
{
    class PrikazJednogObavestenjaPacijentaViewModel
    {

        public ObavestenjeDTO obavestenje { get; set; }

        public PrikazJednogObavestenjaPacijentaViewModel()
        {

        }

        public PrikazJednogObavestenjaPacijentaViewModel(ObavestenjeDTO o)
        {
            obavestenje = o;
        }

        public PrikazJednogObavestenjaPacijentaViewModel(object selectedItem)
        {
            obavestenje = (ObavestenjeDTO)selectedItem;
        }
    }
}
