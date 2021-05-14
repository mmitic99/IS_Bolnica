using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model;
using Bolnica.model.Enum;
using Bolnica.Repozitorijum;

namespace Bolnica.Servis
{
    public class RadnoVremeServis
    {
        private SkladisteRadnihVremena skladisteRadnihVremena;

        public RadnoVremeServis()
        {
            skladisteRadnihVremena = new SkladisteRadnihVremena();
        }
        public List<RadnoVreme> GetAll()
        {
            return skladisteRadnihVremena.GetAll();
        }

        public bool Save(RadnoVreme moguceRadnoVreme)
        {
            bool sacuvaj = false;

            List<RadnoVreme> radnoVremeLekara = (List<RadnoVreme>) GetByJmbg(moguceRadnoVreme.JmbgLekara);

            if (radnoVremeLekara.Count == 0)
            {
                sacuvaj = true;
            }

            foreach (RadnoVreme radnoVreme in GetByJmbg(moguceRadnoVreme.JmbgLekara))
            {
                if ((moguceRadnoVreme.DatumIVremePocetka >= radnoVreme.DatumIVremePocetka &&
                     moguceRadnoVreme.DatumIVremePocetka <= radnoVreme.DatumIVremeZavrsetka) ||
                    (moguceRadnoVreme.DatumIVremeZavrsetka >= radnoVreme.DatumIVremePocetka &&
                     moguceRadnoVreme.DatumIVremeZavrsetka <= radnoVreme.DatumIVremeZavrsetka) ||
                    (radnoVreme.DatumIVremePocetka >= moguceRadnoVreme.DatumIVremePocetka &&
                     radnoVreme.DatumIVremePocetka <= moguceRadnoVreme.DatumIVremeZavrsetka) ||
                    (radnoVreme.DatumIVremeZavrsetka >= moguceRadnoVreme.DatumIVremePocetka &&
                     radnoVreme.DatumIVremeZavrsetka <= moguceRadnoVreme.DatumIVremeZavrsetka)
                    )
                {
                    sacuvaj = false;
                    break;
                }

                sacuvaj = true;
            }

            if(sacuvaj)
                skladisteRadnihVremena.Save(moguceRadnoVreme);

            return sacuvaj;
        }

        public void SaveAll(List<RadnoVreme> radnaVremena)
        {
            skladisteRadnihVremena.SaveAll(radnaVremena);
        }

        public IEnumerable GetByJmbg(string jmbgLekara)
        {
            List<RadnoVreme> svaRadnaVremena = GetAll();
            List<RadnoVreme> radnaVremenaLekara = new List<RadnoVreme>();

            foreach (RadnoVreme radnoVreme in svaRadnaVremena)
            {
                if (radnoVreme.JmbgLekara.Equals(jmbgLekara))
                {
                    radnaVremenaLekara.Add(radnoVreme);
                }
            }

            return radnaVremenaLekara;
        }

        public bool Obrisi(string idRadnogVremena)
        {
            List<RadnoVreme> radnaVremena = GetAll();
            bool uspesno = false;
            foreach (var radnoVreme in radnaVremena)
            {
                if (radnoVreme.IdRadnogVremena.Equals(idRadnogVremena))
                {
                    uspesno = radnaVremena.Remove(radnoVreme);
                    break;
                }
            }
            SaveAll(radnaVremena);
            return uspesno;
        }

        public IEnumerable<RadnoVreme> GetByJmbgAkoRadi(string jmbgLekara)
        {
            List<RadnoVreme> svaRadnaVremena = (List<RadnoVreme>) GetAll();
            List<RadnoVreme> radnaVremena = new List<RadnoVreme>();
            foreach (RadnoVreme radnoVreme in svaRadnaVremena)
            {
                if (radnoVreme.JmbgLekara.Equals(jmbgLekara) && radnoVreme.StatusRadnogVremena == StatusRadnogVremena.Radi)
                {
                    radnaVremena.Add(radnoVreme);
                }
            }

            return radnaVremena;
        }
    }
}