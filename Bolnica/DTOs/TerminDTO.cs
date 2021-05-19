using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model.Enum;
using Repozitorijum;

namespace Bolnica.DTOs
{
    public class TerminDTO
    {
        public DateTime DatumIVremeTermina { get; set; }
        public VrstaPregleda VrstaTermina { get; set; }
        public int IdProstorije { get; set; }
        public String JmbgLekara { get; set; }
        public String JmbgPacijenta { get; set; }
        public String IDTermina { get; set; }
        public String brojSobe { get; set; }
        public double TrajanjeTermina { get; set; } //u satima
        public String opisTegobe { get; set; }
        public String lekar
        {
            get
            {
                return SkladisteZaLekaraXml.GetInstance().getByJmbg(JmbgLekara).FullName;
            }
            set
            {

            }
        }
    }
}
