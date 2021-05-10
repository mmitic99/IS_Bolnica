﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTO
{
    public class LekDTO
    {
        public LekDTO() { }

        public LekDTO(int vrsta, String kolicina, String naziv, int klasa, String jacina, String zamenskiLek, String sastav)
        {
            VrstaLeka = vrsta;
            KolicinaLeka = kolicina;
            NazivLeka = naziv;
            KlasaLeka = klasa;
            JacinaLeka = jacina;
            ZamenskiLek = zamenskiLek;
            SastavLeka = sastav;
        }
        public int VrstaLeka { get; set; }
        public String KolicinaLeka { get; set; }
        public String NazivLeka { get; set; }
        public int KlasaLeka { get; set; }
        public String JacinaLeka { get; set; }
        public String ZamenskiLek { get; set; }
        public String SastavLeka { get; set; }
    }
}
