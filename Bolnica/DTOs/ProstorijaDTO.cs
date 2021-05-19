using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Bolnica.DTOs
{
    public class ProstorijaDTO
    {
        public int IdProstorije { get; set; }
        public int Sprat { get; set; }
        public String BrojSobe { get; set; }
        public VrstaProstorije VrstaProstorije { get; set; }

    }
}
