using Bolnica.DTOs;
using Bolnica.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Repozitorijum.Factory.VerifikacijaLekaFactory
{
   public static class VerifikacijaLekaFactory
    {
        public static IVerifikacijaLeka CreateVerifikacijaLeka()
        {
            return new VerifikacijaLeka();
        }
        public static IVerifikacijaLekaDTO CreateVerifikacijaLekaDTO()
        {
            return new VerifikacijaLekaDTO();
        }
        public static IVerifikacijaLekaDTO CreateVerifikacijaLekaDTOSaParametrima(DateTime vremeSlanja, String naslov, String sadrzaj, String jmbgPosiljaoca, String jmbgPrimaoca, String napomena)
        {
            return new VerifikacijaLekaDTO( vremeSlanja,  naslov, sadrzaj,  jmbgPosiljaoca,  jmbgPrimaoca,  napomena);
        }
        
    }
}
