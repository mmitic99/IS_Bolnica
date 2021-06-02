using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;
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

        public List<RadnoVremeDTO> GetAll()
        {
            List<RadnoVremeDTO> radnaVremena = new List<RadnoVremeDTO>();
            foreach (var radnoVreme in RadnoVremeServis.GetAll())
            {
                radnaVremena.Add(new RadnoVremeDTO()
                {
                    DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                    DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                    StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                    JmbgLekara = radnoVreme.JmbgLekara,
                    IdRadnogVremena = radnoVreme.IdRadnogVremena
                });
            }

            return radnaVremena;
        }

        public bool Save(RadnoVremeDTO radnoVreme)
        {
            return RadnoVremeServis.Save(new RadnoVreme()
            {
                DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                JmbgLekara = radnoVreme.JmbgLekara,
                IdRadnogVremena = radnoVreme.IdRadnogVremena
            });
        }

        public void SaveAll(List<RadnoVreme> radnaVremena)
        {
            RadnoVremeServis.SaveAll(radnaVremena);
        }

        public IEnumerable GetByJmbg(string jmbgLekara)
        {
            List<RadnoVremeDTO> radnaVremena = new List<RadnoVremeDTO>();
            foreach (RadnoVreme radnoVreme in RadnoVremeServis.GetByJmbg(jmbgLekara))
            {
                radnaVremena.Add(new RadnoVremeDTO()
                {
                    DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                    DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                    StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                    JmbgLekara = radnoVreme.JmbgLekara,
                    IdRadnogVremena = radnoVreme.IdRadnogVremena
                });
            }
            return radnaVremena;
        }

        public bool Obrisi(string idRadnogVremena)
        {
            return RadnoVremeServis.Obrisi(idRadnogVremena);
        }

        public IEnumerable GetRadnoVremeZaDan(DateTime selectedDate, string jmbg)
        {
            IEnumerable radnaVremena = RadnoVremeServis.GetByJmbg(jmbg);
            var radnaVremenaZaDatum = new List<RadnoVremeDTO>();
            foreach (RadnoVreme radnoVreme in radnaVremena)
            {
                if (radnoVreme.DatumIVremePocetka.Day == selectedDate.Day &&
                    radnoVreme.DatumIVremePocetka.Month == selectedDate.Month &&
                    radnoVreme.DatumIVremePocetka.Year == selectedDate.Year)
                {
                    radnaVremenaZaDatum.Add(new RadnoVremeDTO()
                    {
                        DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                        DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                        StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                        JmbgLekara = radnoVreme.JmbgLekara,
                        IdRadnogVremena = radnoVreme.IdRadnogVremena
                    });
                }
            }

            return radnaVremenaZaDatum;
        }

        public IEnumerable GetRadnoVremeZaNedelju(DateTime prviDanUNedelji, string jmbg)
        {
            DateTime zadnjiDanUNedelji = prviDanUNedelji.AddDays(6);
            IEnumerable radnaVremena = RadnoVremeServis.GetByJmbg(jmbg);
            var radnaVremenaZaDatum = new List<RadnoVremeDTO>();
            foreach (RadnoVreme radnoVreme in radnaVremena)
            {
                if (radnoVreme.DatumIVremePocetka >= prviDanUNedelji && radnoVreme.DatumIVremePocetka <= zadnjiDanUNedelji)
                {
                    radnaVremenaZaDatum.Add(new RadnoVremeDTO()
                    {
                        DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                        DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                        StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                        JmbgLekara = radnoVreme.JmbgLekara,
                        IdRadnogVremena = radnoVreme.IdRadnogVremena
                    });
                }
            }
            return radnaVremenaZaDatum;
        }

        public IEnumerable GetRadnoVremeZaMesec(DateTime izabraniDatum, string jmbg)
        {
            IEnumerable radnaVremena = RadnoVremeServis.GetByJmbg(jmbg);
            var radnaVremenaZaDatum = new List<RadnoVremeDTO>();
            foreach (RadnoVreme radnoVreme in radnaVremena)
            {
                if (radnoVreme.DatumIVremePocetka.Month == izabraniDatum.Month &&
                    radnoVreme.DatumIVremePocetka.Year == izabraniDatum.Year)
                {
                    radnaVremenaZaDatum.Add(new RadnoVremeDTO()
                    {
                        DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                        DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                        StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                        JmbgLekara = radnoVreme.JmbgLekara,
                        IdRadnogVremena = radnoVreme.IdRadnogVremena
                    });
                }
            }

            return radnaVremenaZaDatum;
        }

        public IEnumerable GetRadnoVremeZaGodinu(DateTime izabranaGodina, string jmbg)
        {
            IEnumerable radnaVremena = RadnoVremeServis.GetByJmbg(jmbg);
            var radnaVremenaZaDatum = new List<RadnoVremeDTO>();
            foreach (RadnoVreme radnoVreme in radnaVremena)
            {
                if (radnoVreme.DatumIVremePocetka.Year == izabranaGodina.Year)
                {
                    radnaVremenaZaDatum.Add(new RadnoVremeDTO()
                    {
                        DatumIVremePocetka = radnoVreme.DatumIVremePocetka,
                        DatumIVremeZavrsetka = radnoVreme.DatumIVremeZavrsetka,
                        StatusRadnogVremena = radnoVreme.StatusRadnogVremena,
                        JmbgLekara = radnoVreme.JmbgLekara,
                        IdRadnogVremena = radnoVreme.IdRadnogVremena
                    });
                }
            }

            return radnaVremenaZaDatum;
        }
    }
}
