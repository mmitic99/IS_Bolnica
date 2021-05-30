using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
    public class Feedback
    {
        public string JmbgKorisnika { get; set; }
        public DateTime DatumIVreme { get; set; }
        public string Sadrzaj { get; set; }
        public int Ocena { get; set; }

    }
}
