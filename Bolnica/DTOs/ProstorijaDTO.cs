using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Enum;

namespace Bolnica.DTOs
{
    public class ProstorijaDTO
    {
        public int IdProstorije { get; set; }
        public int Sprat { get; set; }
        public String BrojSobe { get; set; }
        public VrstaProstorije VrstaProstorije { get; set; }
        public bool RenoviraSe { get; set; }
        public double Kvadratura { get; set; }
        public List<StacionarnaOpremaDTO> Staticka { get; set; }
        public List<PotrosnaOpremaDTO> Potrosna { get; set; }

        public ProstorijaDTO(String BrojProstorije, int Sprat, Model.Enum.VrstaProstorije Vrsta, double Kvadratura)
        {
            this.BrojSobe = BrojProstorije;
            this.Sprat = Sprat;
            this.VrstaProstorije = Vrsta;
            this.Kvadratura = Kvadratura;
        }

        public ProstorijaDTO() { }
    }
}
