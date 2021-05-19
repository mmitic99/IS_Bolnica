using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.viewActions
{
    class AnketaOLekaruViewModel
    {
        public AnketaLekar anketa { get; set; }
        public String IdAnkete { get; set; }
        public String PunoImeLekara { get; set; }
        public AnketaOLekaruViewModel()
        {

        }
        public AnketaOLekaruViewModel(AnketaLekar anketa)
        {
            anketa = anketa;
            PunoImeLekara = SkladisteZaLekaraXml.GetInstance().getByJmbg(anketa.JmbgLekara).FullName;
        }

        public AnketaOLekaruViewModel(PrikacenaAnketaPoslePregledaDTO anketa1)
        {
            anketa = AnketeServis.GetInstance().GetAnketaOLekaru(anketa1.JmbgLekara);
            IdAnkete = anketa1.IDAnkete;
            PunoImeLekara = SkladisteZaLekaraXml.GetInstance().getByJmbg(anketa.JmbgLekara).FullName;
        }
    }
}
