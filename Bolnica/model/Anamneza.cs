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
        public String IdAnamneze
        {
            get
            {
                return ImeLekara + DatumAnamneze.ToString();
            }
            set
            {

            }
        }


        public Anamneza(string anamnezaDijalog, DateTime datumAnamneze, string imeLekara)
        {
            this.AnamnezaDijalog = anamnezaDijalog;
            this.DatumAnamneze = datumAnamneze;
            this.ImeLekara = imeLekara;
            this.IdAnamneze = generateRandId();
        }

        public Anamneza()
        {
        }
        public String generateRandId()
        {
            return ImeLekara + DatumAnamneze.ToString();
        }
    }
}
