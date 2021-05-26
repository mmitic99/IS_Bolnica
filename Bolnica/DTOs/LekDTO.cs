using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class LekDTO
    {
        public LekDTO() { }

        public LekDTO(VrstaLeka vrsta, double kolicina, String naziv, KlasaLeka klasa, int jacina, String zamenskiLek, String sastav)
        {
            VrstaLeka = vrsta;
            KolicinaLeka = kolicina;
            NazivLeka = naziv;
            KlasaLeka = klasa;
            JacinaLeka = jacina;
            ZamenskiLek = zamenskiLek;
            SastavLeka = sastav;
        }
        public VrstaLeka VrstaLeka { get; set; }
        public double KolicinaLeka { get; set; }
        public String NazivLeka { get; set; }
        public KlasaLeka KlasaLeka { get; set; }
        public int IdLeka { get; set; }
        public int JacinaLeka { get; set; }
        public String ZamenskiLek { get; set; }
        public String SastavLeka { get; set; }
        public String SiftaLeka
        {
            get
            {
                return NazivLeka.Substring(0, 3).ToUpper();
            }
            set
            {

            }
        }
    }
}
