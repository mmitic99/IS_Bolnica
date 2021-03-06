using Bolnica.model;
using System;

using System.Collections;
using System.Collections.Generic;

namespace Model
{
   public class Recept
   {
      public Lek lek { get; set; }

      
      public List<int> terminiUzimanjaTokomDana { get; set;}
        public int IdRecepta { get; set; }
        public string ImeLeka { get; set; }
        public string SifraLeka { get; set; }
        public string DodatneNapomene { get; set; }
        public DateTime DatumIzdavanja { get; set; }
        public int BrojDana { get; set; }
        public int Doza { get; set; }
        public List<int> TerminiUzimanjaLeka { get; set; }
        public string Dijagnoza { get; set; } 
        public string ImeDoktora { get; set; }
        public List<Beleska> KomentariPacijenta { get; set; }

        public Recept()
        {
        }

        public Recept(string imeLeka, string sifraLeka, string dodatneNapomene, DateTime datumIzdavanja, int brojDana, int doza, List<int> terminiUzimanjaLeka, string dijagnoza, string imeDoktora)
        {
            ImeLeka = imeLeka;
            SifraLeka = sifraLeka;
            DodatneNapomene = dodatneNapomene;
            DatumIzdavanja = datumIzdavanja;
            BrojDana = brojDana;
            Doza = doza;
            TerminiUzimanjaLeka = terminiUzimanjaLeka;
            Dijagnoza = dijagnoza;
            ImeDoktora = imeDoktora;
            KomentariPacijenta = new List<Beleska>();
        }

    }
}