using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.model
{
   public class BolnickoLecenje
    {
        public  int  idProstorije { get; set; }
        public String jmbgPacijenta { get; set; }
        public DateTime pocetakBolnickogLecenja { get; set; }
        public DateTime krajBolnickogLecenja { get; set; }
        public String jmbgLekara { get; set; }

        public BolnickoLecenje() { }

        public BolnickoLecenje(int idProstorije, string jmbgPacijenta, String jmbgLekara, DateTime pocetakBolnickogLecenja, DateTime krajBolnickogLecenja)
        {
            this.idProstorije = idProstorije;
            this.jmbgPacijenta = jmbgPacijenta;
            this.jmbgLekara = jmbgLekara;
            this.pocetakBolnickogLecenja = pocetakBolnickogLecenja;
            this.krajBolnickogLecenja = krajBolnickogLecenja;
        }
    }
}
