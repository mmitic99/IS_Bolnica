using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class BolnickoLecenjeDTO
    {
        public String imeLekara { get; set; }
        public String imePacijenta { get; set; }
        public String brojSobe { get; set; }
        public DateTime DatumOtpustanja { get; set; }
        public DateTime DatumPrijema { get; set; }

    }
}
