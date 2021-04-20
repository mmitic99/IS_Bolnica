using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Anamneza
    {
        public string AnamnezaDijalog { get; set; }
        public DateTime DatumAnamneze { get; set; }
        public string ImeLekara { get; set; }

        public Anamneza(string anamnezaDijalog, DateTime datumAnamneze, string imeLekara)
        {   
            AnamnezaDijalog = anamnezaDijalog;
            DatumAnamneze = datumAnamneze;
            ImeLekara = imeLekara;
        }

        public Anamneza()
        {
        }
    }
}
