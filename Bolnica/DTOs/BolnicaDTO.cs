using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class BolnicaDTO
    {
        public String Ime { get; set; }
        public String Adresa { get; set; }
        public String Telefon { get; set; }

        public BolnicaDTO() { }
        public BolnicaDTO(string ime, string adresa, string telefon)
        {
            Ime = ime;
            Adresa = adresa;
            Telefon = telefon;
        }
    }
}
