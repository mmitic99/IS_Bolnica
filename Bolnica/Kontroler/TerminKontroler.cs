using Bolnica.DTOs;
using Model;
using Model.Enum;
using Servis;
using System;
using System.Collections;
using System.Collections.Generic;
using Bolnica.model;

namespace Kontroler
{

    public class TerminKontroler
    {
        public Servis.TerminServis terminServis;

        public static TerminKontroler instance = null;

        public static TerminKontroler getInstance()
        {
            if (instance == null)
            {
                return new TerminKontroler();
            }
            else
            {
                return instance;
            }
        }

        public TerminKontroler()
        {
            terminServis = new TerminServis();
            instance = this;
        }

        public bool ZakaziTermin(TerminDTO terminDto)
        {
            Termin termin = new Termin()
            {
                IDTermina = terminDto.IDTermina,
                JmbgLekara = terminDto.JmbgLekara,
                brojSobe = terminDto.brojSobe,
                VrstaTermina = terminDto.VrstaTermina,
                TrajanjeTermina = terminDto.TrajanjeTermina,
                opisTegobe = terminDto.opisTegobe,
                JmbgPacijenta = terminDto.JmbgPacijenta,
                DatumIVremeTermina = terminDto.DatumIVremeTermina,
                IdProstorije = terminDto.IdProstorije
            };
            termin.IDTermina = termin.generateRandId();
            termin.IdProstorije = ProstorijeServis.GetInstance().GetPrvaPogodna(termin);
            return terminServis.ZakaziTermin(termin);
        }

        public bool OtkaziTermin(string IdTermina)
        {
            return terminServis.OtkaziTermin(IdTermina);
        }

        public bool IzmeniTermin(TerminDTO terminDto, Object stariIdTermina = null)
        {
            Termin termin = new Termin()
            {
                IDTermina = terminDto.IDTermina,
                JmbgLekara = terminDto.JmbgLekara,
                brojSobe = terminDto.brojSobe,
                VrstaTermina = terminDto.VrstaTermina,
                TrajanjeTermina = terminDto.TrajanjeTermina,
                opisTegobe = terminDto.opisTegobe,
                JmbgPacijenta = terminDto.JmbgPacijenta,
                DatumIVremeTermina = terminDto.DatumIVremeTermina,
                IdProstorije = terminDto.IdProstorije
            };
            if (stariIdTermina != null)
            {
                return terminServis.IzmeniTermin((Termin)termin, ((Termin)stariIdTermina).IDTermina);
            }
            return terminServis.IzmeniTermin((Termin)termin, null);

        }

        public IEnumerable<int> GetMesecnePreglede(List<string> sviDaniUMesecu)
        {
            List<int> mesecniPreglediPoDanu = new List<int>(new int[sviDaniUMesecu.Count]);

            foreach (Termin termin in terminServis.GetAll())
            {
                for (int dan = 1; dan <= sviDaniUMesecu.Count; dan++)
                {
                    if (termin.VrstaTermina == VrstaPregleda.Pregled && termin.DatumIVremeTermina.Year == DateTime.Now.Year &&
                        termin.DatumIVremeTermina.Month == DateTime.Now.Month && termin.DatumIVremeTermina.Day == dan)
                    {
                        mesecniPreglediPoDanu[dan - 1]++;
                    }
                }
            }

            return mesecniPreglediPoDanu;
        }
        public IEnumerable<int> GetMesecneOperacije(List<string> sviDaniUMesecu)
        {
            List<int> Termini = new List<int>(new int[sviDaniUMesecu.Count]);

            foreach (Termin termin in terminServis.GetAll())
            {
                for (int dan = 1; dan <= sviDaniUMesecu.Count; dan++)
                {
                    if (termin.VrstaTermina == VrstaPregleda.Operacija && termin.DatumIVremeTermina.Year == DateTime.Now.Year &&
                        termin.DatumIVremeTermina.Month == DateTime.Now.Month && termin.DatumIVremeTermina.Day == dan)
                    {
                        Termini[dan-1]++;
                    }
                }
            }

            return Termini;
        }

        public List<Termin> GetAll()
        {
            return terminServis.GetAll();
        }


        public void Save(Termin termin)
        {
            terminServis.Save(termin);
        }

        public void SaveAll(List<Termin> termini)
        {
            terminServis.SaveAll(termini);
        }

        public List<Termin> GetByJmbg(string jmbg)
        {
            return TerminServis.getInstance().GetByJmbgPacijenta(jmbg);
        }

        internal DateTime? PrviMoguciDanZakazivanja(Object prethodniTermin)
        {
            Termin prethodnTermin = (Termin)prethodniTermin;
            return TerminServis.getInstance().DobaviPrviMoguciDanZakazivanja(prethodnTermin);
        }

        internal DateTime? PoslednjiMoguciDanZakazivanja(Object prethodniTermin)
        {
            Termin prethodnTermin = (Termin)prethodniTermin;
            return TerminServis.getInstance().DobaviPPoslednjiMogiDanZakazivanja(prethodnTermin);
        }

        public TerminDTO GetById(String idTermina)
        {
            Termin termin = terminServis.GetById(idTermina);
            return new TerminDTO()
            {
                JmbgLekara = termin.JmbgLekara,
                IDTermina = termin.IDTermina,
                brojSobe = termin.brojSobe,
                VrstaTermina = termin.VrstaTermina,
                TrajanjeTermina = termin.TrajanjeTermina,
                opisTegobe = termin.opisTegobe,
                JmbgPacijenta = termin.JmbgPacijenta,
                DatumIVremeTermina = termin.DatumIVremeTermina,
                IdProstorije = termin.IdProstorije
            };
        }

        public IEnumerable GetBuduciTerminPacLekar()
        {
            return terminServis.GetBuduciTerminPacLekar();
        }

        public void RemoveByID(string iDTermina)
        {
            terminServis.RemoveByID(iDTermina);
        }

        public ParametriZaTrazenjeTerminaKlasifikovanoDTO KlasifikujUlazneParametre(ParametriZaTrazenjeMogucihTerminaDTO parametriDTO)
        {
            ParametriZaTrazenjeTerminaKlasifikovanoDTO parametriKlasifikovanoDTO = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
            {
                JmbgLekara = ((LekarDTO)parametriDTO.IzabraniLekar).Jmbg,
                PrethodnoZakazaniTermin = (Termin)parametriDTO.PrethodnoZakazaniTermin,
                trajanjeUMinutama = (int)parametriDTO.trajanjeUMinutama,
                tegobe = (String)parametriDTO.OpisTegobe
            };

            parametriKlasifikovanoDTO.IzabraniDani = new List<DateTime>((IEnumerable<DateTime>)parametriDTO.IzabraniDatumi);
            parametriKlasifikovanoDTO.JmbgPacijenta = DobaviJmbgPacijentaPoUnosu(parametriDTO);
            parametriKlasifikovanoDTO.Pocetak = new TimeSpan((int)parametriDTO.PocetnaSatnica + 6, (int)parametriDTO.PocetakMinut * 30, 0);
            parametriKlasifikovanoDTO.Kraj = new TimeSpan((int)parametriDTO.KrajnjaSatnica + 6, (int)parametriDTO.KrajnjiMinuti * 30, 0);
            parametriKlasifikovanoDTO.Prioritet = PronadjiPrioritet((bool)parametriDTO.NemaPrioritet, (bool)parametriDTO.PrioritetLekar);
            parametriKlasifikovanoDTO.vrstaTermina = PronadjiVrstuTermina(parametriDTO.vrstaTermina);
            if (parametriDTO.PrethodnoZakazaniTermin != null)
                parametriKlasifikovanoDTO.PrethodnoZakazaniTermin = (Termin)parametriDTO.PrethodnoZakazaniTermin;

            return parametriKlasifikovanoDTO;
        }

        public List<TerminDTO> NadjiHitanTermin(string jmbgPacijenta, string vrstaSpecijalizacije)
        {
            List<TerminDTO> termini = new List<TerminDTO>();
            foreach (Termin termin in terminServis.NadjiHitanTermin(jmbgPacijenta, vrstaSpecijalizacije))
            {
                termini.Add(new TerminDTO()
                {
                    JmbgLekara = termin.JmbgLekara,
                    IDTermina = termin.IDTermina,
                    brojSobe = termin.brojSobe,
                    VrstaTermina = termin.VrstaTermina,
                    TrajanjeTermina = termin.TrajanjeTermina,
                    opisTegobe = termin.opisTegobe,
                    JmbgPacijenta = termin.JmbgPacijenta,
                    DatumIVremeTermina = termin.DatumIVremeTermina,
                    IdProstorije = termin.IdProstorije
                });
            }
            return termini;
        }

        private VrstaPregleda PronadjiVrstuTermina(object zahtevaocTermina)
        {
            if ((int)zahtevaocTermina == 0)
                return VrstaPregleda.Pregled;
            else
                return VrstaPregleda.Operacija;
        }

        private string DobaviJmbgPacijentaPoUnosu(ParametriZaTrazenjeMogucihTerminaDTO parametriDTO)
        {
            if (parametriDTO.PrethodnoZakazaniTermin == null)
                return (String)parametriDTO.Pacijent;
            else
            {
                return ((Termin)parametriDTO.PrethodnoZakazaniTermin).JmbgPacijenta;
            }
        }

        internal object GetAllPossibleApointmentsDates(object selectedItem)
        {
            return TerminServis.getInstance().DobaviMoguceSveDaneZakazivanja((Termin)selectedItem);
        }


        private int PronadjiPrioritet(bool nemaPrioritet, bool prioritetLekar)
        {
            int prioritet;
            if (nemaPrioritet) return 0;
            else if (prioritetLekar) return 1;
            else return 2;
        }

        public List<TerminDTO> NadjiTermineZaParametre(ParametriZaTrazenjeMogucihTerminaDTO parametriDTO)
        {
            List<TerminDTO> termini = new List<TerminDTO>();
            foreach (Termin termin in TerminServis.getInstance().NadjiTermineZaParametre(KlasifikujUlazneParametre(parametriDTO)))
            {
                termini.Add(new TerminDTO()
                {
                    JmbgLekara = termin.JmbgLekara,
                    IDTermina = termin.IDTermina,
                    brojSobe = termin.brojSobe,
                    VrstaTermina = termin.VrstaTermina,
                    TrajanjeTermina = termin.TrajanjeTermina,
                    opisTegobe = termin.opisTegobe,
                    JmbgPacijenta = termin.JmbgPacijenta,
                    DatumIVremeTermina = termin.DatumIVremeTermina,
                    IdProstorije = termin.IdProstorije
                });
            }
            return termini;
        }

        public List<TerminDTO> NadjiTermineZaParametre(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametriDTO)
        {
            List<TerminDTO> termini = new List<TerminDTO>();
            foreach (Termin termin in terminServis.NadjiTermineZaParametre(parametriDTO))
            {
                termini.Add(new TerminDTO()
                {
                    JmbgLekara = termin.JmbgLekara,
                    IDTermina = termin.IDTermina,
                    brojSobe = termin.brojSobe,
                    VrstaTermina = termin.VrstaTermina,
                    TrajanjeTermina = termin.TrajanjeTermina,
                    opisTegobe = termin.opisTegobe,
                    JmbgPacijenta = termin.JmbgPacijenta,
                    DatumIVremeTermina = termin.DatumIVremeTermina,
                    IdProstorije = termin.IdProstorije
                });
            }
            return termini;
        }

        public Termin GetTerminZaDatumILekara(DateTime datumIVreme, string jmbgLekara)
        {
            return terminServis.GetTerminZaDatumILekara(datumIVreme, jmbgLekara);
        }
    }
}