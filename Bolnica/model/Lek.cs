using Model.Enum;
using System;

namespace Model
{
    public class Lek
    {
        public Lek() {}

        public Lek(VrstaLeka vrsta, double kolicina, String naziv, KlasaLeka klasa, int jacina, String zamenskiLek, String sastav) 
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
   }
}