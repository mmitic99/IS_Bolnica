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
        private LekarKontroler LekarKontroler;

        public AnketaOLekaruViewModel()
        {

        }
        public AnketaOLekaruViewModel(AnketaLekar anketa)
        {
            anketa = anketa;
            this.LekarKontroler = new LekarKontroler();
            PunoImeLekara = LekarKontroler.GetByJmbg(anketa.JmbgLekara).FullName;

        }
    }
}
