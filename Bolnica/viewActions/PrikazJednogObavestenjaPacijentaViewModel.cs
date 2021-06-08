using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.model;

namespace Bolnica.viewActions
{
    public class PrikazJednogObavestenjaPacijentaViewModel
    {

        public ObavestenjeDTO obavestenje { get; set; }
        public KvartalnaAnketa ZakacenaKvartalnaAnketa {get; set;}
        public AnketaLekar ZakacenaAnketaOLekaru { get; set; }
        private AnketeKontroler AnketeKontroler;


        public PrikazJednogObavestenjaPacijentaViewModel()
        {

        }

        public PrikazJednogObavestenjaPacijentaViewModel(ObavestenjeDTO o)
        {
            obavestenje = o;
        }

        public PrikazJednogObavestenjaPacijentaViewModel(object selectedItem)
        {
            this.obavestenje = (ObavestenjeDTO)selectedItem;
            this.AnketeKontroler = new AnketeKontroler();
            PrikaciKvartalnuAnketu();
            PrikaciAnketuOLekaru();
        }

        private void PrikaciAnketuOLekaru()
        {
            if(obavestenje.anketaOLekaru!=null)
            {
                this.ZakacenaAnketaOLekaru = AnketeKontroler.GetAnketaOLekaruByJmbg(obavestenje.anketaOLekaru.JmbgLekara);
                MainViewModel.getInstance().AnketaOLekaruVM = new AnketaOLekaruViewModel(obavestenje.anketaOLekaru);
            }
        }

        private void PrikaciKvartalnuAnketu()
        {
            if (obavestenje.kvartalnaAnketa != new DateTime(0))
            {
                this.ZakacenaKvartalnaAnketa = AnketeKontroler.GetByDate(obavestenje.VremeObavestenja);
            }
        }
    }
}
