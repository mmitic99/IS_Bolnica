using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bolnica.model;
using Bolnica.model.Enum;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.Factory.SkladisteRadnihVremenaFactory;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;
using Model;
using Servis;

namespace Bolnica.Servis
{
    public class RadnoVremeServis
    {
        private ISkladisteRadnihVremena skladisteRadnihVremena;
        private ISkladisteZaLekara skladisteZaLekara;
        private ISkladisteZaTermine skladisteZaTermine;

        private ISkladisteRadnihVremenaFactory _skladisteRadnihVremenaFactory = new SkladisteRadnihVremenaXmlFactory();
        public ISkladisteRadnihVremenaFactory SkladisteRadnihVremenaFactory
        {
            get => _skladisteRadnihVremenaFactory;
            set
            {
                _skladisteRadnihVremenaFactory = value;
                skladisteRadnihVremena = value.CreateSkladisteRadnihVremena();
            }
        }

        public RadnoVremeServis()
        {
            skladisteRadnihVremena = SkladisteRadnihVremenaFactory.CreateSkladisteRadnihVremena();
            skladisteZaLekara = SkladisteZaLekaraXml.GetInstance();
            skladisteZaTermine = new SkladisteZaTermineXml();
        }
        public List<RadnoVreme> GetAll()
        {
            return skladisteRadnihVremena.GetAll();
        }

        public bool Save(RadnoVreme moguceRadnoVreme)
        {
            bool sacuvaj = false;
            List<RadnoVreme> radnoVremeLekara = (List<RadnoVreme>) GetByJmbg(moguceRadnoVreme.JmbgLekara);
            sacuvaj = radnoVremeLekara.Count == 0 || ProveriRadnaVremenaLekara(moguceRadnoVreme);
            if(sacuvaj)
            {
                sacuvaj = IzmeniSlobodneDane(moguceRadnoVreme);
                if(sacuvaj)
                    skladisteRadnihVremena.Save(moguceRadnoVreme);
            }
            return sacuvaj;
        }

        private bool IzmeniSlobodneDane(RadnoVreme moguceRadnoVreme)
        {
            bool retVal = true;
            if (moguceRadnoVreme.StatusRadnogVremena == StatusRadnogVremena.NaOdmoru)
            {
                Lekar lekar = skladisteZaLekara.getByJmbg(moguceRadnoVreme.JmbgLekara);
                int brojDanaZaOdmor =
                    (int) Math.Ceiling((moguceRadnoVreme.DatumIVremeZavrsetka - moguceRadnoVreme.DatumIVremePocetka).TotalDays);
                if (brojDanaZaOdmor > lekar.BrojSlobodnihDana)
                {
                    retVal = false;
                }

                lekar.BrojSlobodnihDana -= brojDanaZaOdmor;
                skladisteZaLekara.IzmeniLekara(moguceRadnoVreme.JmbgLekara, lekar);
            }

            return retVal;
        }

        private bool ProveriRadnaVremenaLekara(RadnoVreme moguceRadnoVreme)
        {
            bool sacuvaj = false;
            foreach (RadnoVreme radnoVreme in GetByJmbg(moguceRadnoVreme.JmbgLekara))
            {
                if (DaLiJeMoguceRadnoVremeIspravno(moguceRadnoVreme, radnoVreme))
                {
                    sacuvaj = false;
                    break;
                }
                sacuvaj = true;
            }

            return sacuvaj;
        }

        private static bool DaLiJeMoguceRadnoVremeIspravno(RadnoVreme moguceRadnoVreme, RadnoVreme radnoVreme)
        {
            return (moguceRadnoVreme.DatumIVremePocetka >= radnoVreme.DatumIVremePocetka &&
                    moguceRadnoVreme.DatumIVremePocetka <= radnoVreme.DatumIVremeZavrsetka) ||
                   (moguceRadnoVreme.DatumIVremeZavrsetka >= radnoVreme.DatumIVremePocetka &&
                    moguceRadnoVreme.DatumIVremeZavrsetka <= radnoVreme.DatumIVremeZavrsetka) ||
                   (radnoVreme.DatumIVremePocetka >= moguceRadnoVreme.DatumIVremePocetka &&
                    radnoVreme.DatumIVremePocetka <= moguceRadnoVreme.DatumIVremeZavrsetka) ||
                   (radnoVreme.DatumIVremeZavrsetka >= moguceRadnoVreme.DatumIVremePocetka &&
                    radnoVreme.DatumIVremeZavrsetka <= moguceRadnoVreme.DatumIVremeZavrsetka);
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
                    if(NemaZakazanihTermina(radnoVreme))
                    {
                        uspesno = radnaVremena.Remove(radnoVreme);
                        SaveAll(radnaVremena);
                    }
                    break;
                }
            }
            return uspesno;
        }

        private bool NemaZakazanihTermina(RadnoVreme radnoVreme)
        {
            bool nema = true;
            List<Termin> termini = skladisteZaTermine.GetByJmbgLekar(radnoVreme.JmbgLekara);

            foreach (Termin termin in termini)
            {
                if (termin.DatumIVremeTermina >= radnoVreme.DatumIVremePocetka ||
                    termin.DatumIVremeTermina <= radnoVreme.DatumIVremeZavrsetka)
                {
                    nema = false;
                    break;
                }
            }
            return nema;
        }

        public IEnumerable<RadnoVreme> GetByJmbgAkoRadi(string jmbgLekara)
        {
            List<RadnoVreme> svaRadnaVremena = GetAll();
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