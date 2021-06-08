using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;

namespace Bolnica.viewActions
{
    public class AnketaOLekaruViewModel
    {
        public AnketaLekar Anketa { get; set; }
        public String IdAnkete { get; set; }
        public String PunoImeLekara { get; set; }
        public String JmbgLekara { get; set; }
        private LekarKontroler LekarKontroler;

        public AnketaOLekaruViewModel()
        {

        }
        public AnketaOLekaruViewModel(PrikacenaAnketaPoslePregledaDTO anketaOLekaru)
        {
            this.LekarKontroler = new LekarKontroler();
            IdAnkete = anketaOLekaru.IDAnkete;
            PunoImeLekara = LekarKontroler.GetByJmbg(anketaOLekaru.JmbgLekara).FullName;
            JmbgLekara = anketaOLekaru.JmbgLekara;
        }
        public AnketaOLekaruViewModel(AnketaLekar anketa)
        {
            anketa = anketa;
            this.LekarKontroler = new LekarKontroler();
            PunoImeLekara = LekarKontroler.GetByJmbg(anketa.JmbgLekara).FullName;
            JmbgLekara = anketa.JmbgLekara;

        }
    }
}
