using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;

namespace Bolnica.model
{
    public class AnketaLekar
    {
        public String JmbgLekara { get; set; }
        public double ProsecnaOcena { get; set; }
        public List<String> Komentari { get; set; }
        public int BrojAnketa { get; set; }

        public List<String> ispunjeneAnkete {get; set;}
        public AnketaLekar()
        {

        }

        public AnketaLekar(String JmbgLekara)
        {
            this.JmbgLekara = JmbgLekara;
            ProsecnaOcena = 0.0;
            Komentari = new List<string>();
            BrojAnketa = 0;
            ispunjeneAnkete = new List<string>();
        }

        public void DodajPopunjenuAnketu(PopunjenaAnketaPoslePregledaDTO anketa)
        {
            Komentari.Add(anketa.Komentar);
            ProsecnaOcena = azuriranjeProsecneOcene(anketa.Ocena, ProsecnaOcena);
            ispunjeneAnkete.Add(anketa.IDAnkete);
            ++BrojAnketa;
        }

        public double azuriranjeProsecneOcene(double novaOcena, double ProsecnaOcena)
        {
            return (ProsecnaOcena * BrojAnketa + novaOcena) / (BrojAnketa + 1);
        }
    }
}
