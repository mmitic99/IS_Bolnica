using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class ReceptDTO
    {
        public ReceptDTO() { }

        public class ReceptiDTO
        {
            public List<int> terminiUzimanjaTokomDana { get; set; }
            public string ImeLeka { get; set; }
            public string SifraLeka { get; set; }
            public string DodatneNapomene { get; set; }
            public DateTime DatumIzdavanja { get; set; }
            public int BrojDana { get; set; }
            public int Doza { get; set; }
            public string Dijagnoza { get; set; }
            public string ImeDoktora { get; set; }
            public Pacijent p { get; set; }
            public Pacijent p1 { get; set; }
        }
    }
}
