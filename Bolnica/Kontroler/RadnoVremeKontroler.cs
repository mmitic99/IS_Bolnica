using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model;
using Bolnica.Servis;

namespace Bolnica.Kontroler
{
    class RadnoVremeKontroler
    {
        private RadnoVremeServis RadnoVremeServis;

        public RadnoVremeKontroler()
        {
            RadnoVremeServis = new RadnoVremeServis();
        }

        public List<RadnoVreme> GetAll()
        {
            return RadnoVremeServis.GetAll();
        }

        public bool Save(RadnoVreme radnoVreme)
        {
            return RadnoVremeServis.Save(radnoVreme);
        }

        public void SaveAll(List<RadnoVreme> radnaVremena)
        {
            RadnoVremeServis.SaveAll(radnaVremena);
        }

        public IEnumerable GetByJmbg(string jmbgLekara)
        {
            return RadnoVremeServis.GetByJmbg(jmbgLekara);
        }

        public bool Obrisi(string idRadnogVremena)
        {
            return RadnoVremeServis.Obrisi(idRadnogVremena);
        }
    }
}
