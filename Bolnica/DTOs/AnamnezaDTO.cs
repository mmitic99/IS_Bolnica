using System;

namespace Bolnica.DTOs
{
    public class AnamnezaDTO
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
    }
}