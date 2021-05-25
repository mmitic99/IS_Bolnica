using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class ProstorijaValidacijaDTO
    {
        public String Sprat { get; set; }
        public String BrojSobe { get; set; }
        public int VrstaProstorije { get; set; }
        public String Kvadratura { get; set; }

        public ProstorijaValidacijaDTO(String BrojProstorije, String Sprat, int Vrsta, String Kvadratura)
        {
            this.BrojSobe = BrojProstorije;
            this.Sprat = Sprat;
            this.VrstaProstorije = Vrsta;
            this.Kvadratura = Kvadratura;
        }
    }
}
