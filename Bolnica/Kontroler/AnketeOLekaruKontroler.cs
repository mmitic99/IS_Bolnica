using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class AnketeOLekaruKontroler
    {
        private AnketeOLekaruServis AnketeOLekaruServis;

        public AnketeOLekaruKontroler()
        {
            this.AnketeOLekaruServis = new AnketeOLekaruServis();
        }

        public bool SacuvajAnketuOLekaru(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            bool uspesnoSacuvana = false;
            if (uneteSveOceneLekarAnketa(popunjena))
            {
                PopunjenaAnketaPoslePregledaDTO popunjenaAnketa = KlasifikujParametrePopunjeneAnketeOLekaru(popunjena);
                uspesnoSacuvana = AnketeOLekaruServis.SacuvajAnketuOLekaru(popunjenaAnketa);
            }
            return uspesnoSacuvana;
        }

        private bool uneteSveOceneLekarAnketa(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            bool isPopunjena = false;
            if ((String)popunjena.Ocena != "-1" && popunjena.Komentar != null)
            {
                isPopunjena = true;
            }
            return isPopunjena;
        }

        private PopunjenaAnketaPoslePregledaDTO KlasifikujParametrePopunjeneAnketeOLekaru(PopunjenaAnketaPoslePregledaObjectDTO popunjena)
        {
            PopunjenaAnketaPoslePregledaDTO popunjenaAnketa = new PopunjenaAnketaPoslePregledaDTO()
            {
                IDAnkete = (String)popunjena.IDAnkete,
                JmbgLekara = (String)popunjena.JmbgLekara,
                Komentar = (String)popunjena.Komentar,
                Ocena = Double.Parse((String)popunjena.Ocena)
            };
            return popunjenaAnketa;
        }

        internal AnketaLekar GetAnketaOLekaruByJmbg(string jmbgLekara)
        {
            return AnketeOLekaruServis.GetAnketaOLekaru(jmbgLekara);
        }

        internal bool DaLiJeKorisnikPopunioAnketu(PrikacenaAnketaPoslePregledaDTO anketaOLekaru)
        {
            return AnketeOLekaruServis.DaLiJeKorisnikPopunioAnketuOLekaru(anketaOLekaru);
        }
    }
}
