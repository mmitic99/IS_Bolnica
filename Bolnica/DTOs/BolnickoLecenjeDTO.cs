using Bolnica.Repozitorijum.XmlSkladiste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    
    public class BolnickoLecenjeDTO
    {
        public String jmbgLekara { get; set; }
        public String jmbgPacijenta { get; set; }
        public String imeLekara
        {
            get
            {
               return SkladisteZaLekaraXml.GetInstance().getByJmbg(jmbgLekara).FullName;
            }

            set { }
        }
       
        public String imePacijenta
        { get
            {
                return SkladistePacijentaXml.GetInstance().GetByJmbg(jmbgPacijenta).FullName;
            }
            set
            {

            } }
        public String brojSobe { get; set; }
        public DateTime DatumOtpustanja { get; set; }
        public DateTime DatumPrijema { get; set; }
        public int idProstorije { get; set; }
           

    }
   
}
