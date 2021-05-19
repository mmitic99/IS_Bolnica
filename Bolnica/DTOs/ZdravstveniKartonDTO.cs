using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class ZdravstveniKartonDTO
    {
        public List<IzvestajDTO> Izvestaj { get; set; }
        public List<AnamnezaDTO> Anamneze { get; set; }
        public List<String> Alergeni { get; set; }
    }
}
