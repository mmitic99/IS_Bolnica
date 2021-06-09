using Bolnica.DTOs;
using Bolnica.model;
using Model.Enum;
using Servis;
using System;
using System.Collections;
using System.Collections.Generic;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using Model;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;

namespace Kontroler
{

    public class TerminKontroler
    {
        public Servis.TerminServis TerminServis;

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
            TerminServis = new TerminServis();
            instance = this;
        }

        public bool ZakaziTermin(Object selectedAppointment)
        {
            TerminDTO terminDto = (TerminDTO)selectedAppointment;
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
            return TerminServis.ZakaziTermin(termin);
        }

        public bool OtkaziTermin(string IdTermina)
        {
            return TerminServis.OtkaziTermin(IdTermina);
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
                return TerminServis.IzmeniTermin((Termin)termin, ((Termin)stariIdTermina).IDTermina);
            }
            return TerminServis.IzmeniTermin((Termin)termin, null);

        }

        public Dictionary<DateTime, String> getNotesForTheMonth(DateTime targetDate, string jmbgPacijenta)
        {
            return TerminServis.getNotesForTheMonth(targetDate, jmbgPacijenta);
        }

        public IEnumerable<int> GetMesecnePreglede(List<string> sviDaniUMesecu)
        {
            return MesecniPreglediPoDanu(sviDaniUMesecu, VrstaPregleda.Pregled);
        }
        public IEnumerable<int> GetMesecneOperacije(List<string> sviDaniUMesecu)
        {
            return MesecniPreglediPoDanu(sviDaniUMesecu, VrstaPregleda.Operacija);
        }

        private List<int> MesecniPreglediPoDanu(List<string> sviDaniUMesecu, VrstaPregleda vrstaPregleda)
        {
            List<int> mesecniTerminiPoDanu = new List<int>(new int[sviDaniUMesecu.Count]);

            foreach (Termin termin in TerminServis.GetAll())
            {
                for (int dan = 1; dan <= sviDaniUMesecu.Count; dan++)
                {
                    if (termin.VrstaTermina == vrstaPregleda && termin.DatumIVremeTermina.Year == DateTime.Now.Year &&
                        termin.DatumIVremeTermina.Month == DateTime.Now.Month && termin.DatumIVremeTermina.Day == dan)
                    {
                        mesecniTerminiPoDanu[dan - 1]++;
                    }
                }
            }
            return mesecniTerminiPoDanu;
        }

        public int DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(string jmbg)
        {
            return TerminServis.DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(jmbg);
        }

        public List<Termin> GetAll()
        {
            return TerminServis.GetAll();
        }


        public void Save(Termin termin)
        {
            TerminServis.Save(termin);
        }

        public void SaveAll(List<Termin> termini)
        {
            TerminServis.SaveAll(termini);
        }

        public IEnumerable GetByJmbgPacijenta(string jmbg)
        {
            return TerminServis.GetByJmbgPacijenta(jmbg);
        }

        public IEnumerable GetByJmbgPacijentaVremenskiPeriod(DateTime pocetak, DateTime kraj, string JmbgPacijenta)
        {
            return TerminServis.GetByJmbgPacijentaVremenskiPeriod(pocetak, kraj, JmbgPacijenta);
        }

        internal DateTime? PrviMoguciDanZakazivanja(Object prethodniTermin)
        {
            Termin prethodnTermin = (Termin)prethodniTermin;
            return TerminServis.DobaviPrviMoguciDanZakazivanja(prethodnTermin);
        }

        internal DateTime? PoslednjiMoguciDanZakazivanja(Object prethodniTermin)
        {
            Termin prethodnTermin = (Termin)prethodniTermin;
            return TerminServis.DobaviPPoslednjiMogiDanZakazivanja(prethodnTermin);
        }

        public TerminDTO GetById(String idTermina)
        {
            Termin termin = TerminServis.GetById(idTermina);
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

        internal void RemoveSelected(object selectedItem)
        {
            Termin SelektovaniTermin = (Termin)selectedItem;
            TerminServis.RemoveByID(SelektovaniTermin.IDTermina);
        }

        public IEnumerable GetBuduciTerminPacLekar()
        {
            return TerminServis.GetBuduciTerminPacLekar();
        }

        public void RemoveByID(string iDTermina)
        {
            TerminServis.RemoveByID(iDTermina);
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

        public bool DaLiJeMoguceOtkazatiTermin(object selectedItem)
        {
            Termin selektovaniTermin = (Termin)selectedItem;
            return selektovaniTermin.DatumIVremeTermina.Date > DateTime.Today
                && selektovaniTermin.VrstaTermina != VrstaPregleda.Operacija;
        }

        public List<TerminDTO> NadjiHitanTermin(string jmbgPacijenta, string vrstaSpecijalizacije)
        {
            List<TerminDTO> termini = new List<TerminDTO>();
            foreach (Termin termin in TerminServis.NadjiHitanTermin(jmbgPacijenta, vrstaSpecijalizacije))
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
            foreach (Termin termin in TerminServis.NadjiTermineZaParametre(parametriDTO))
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
            return TerminServis.GetTerminZaDatumILekara(datumIVreme, jmbgLekara);
        }

        public List<TerminDTO> GetByDateForLekar(DateTime datum, String jmbgLekara)
        {
            List<Termin> termini = TerminServis.GetByDateForLekar(datum, jmbgLekara);
            List<TerminDTO> terminiDTO = new List<TerminDTO>();
            foreach (Termin termin in termini)
            {
                TerminDTO terminDTO = new TerminDTO()
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
                terminiDTO.Add(terminDTO);
            }
            return terminiDTO;
        }

        public string GenerisiIzvestaj(DateTime datumPocetka, DateTime datumZavrsetka)
            {
                return TerminServis.GenerisiIzvestaj(datumPocetka, datumZavrsetka);

            }

        public IEnumerable GetBuduciTerminPacLekarZaDan(DateTime selectedDate)
        {
            IEnumerable sviTermini = TerminServis.GetBuduciTerminPacLekar();
            var terminiZaDatum = new List<TerminPacijentLekarDTO>();
            foreach (TerminPacijentLekarDTO termin in sviTermini)
            {
                if (termin.termin.DatumIVremeTermina.Day == selectedDate.Day &&
                    termin.termin.DatumIVremeTermina.Month == selectedDate.Month &&
                    termin.termin.DatumIVremeTermina.Year == selectedDate.Year)
                {
                    terminiZaDatum.Add(termin);
                }
            }

            return terminiZaDatum;
        }

        public IEnumerable GetBuduciTerminPacLekarZaNedelju(DateTime prviDanUNedelji)
        {
            DateTime zadnjiDanUNedelji = prviDanUNedelji.AddDays(6);
            IEnumerable sviTermini = TerminServis.GetBuduciTerminPacLekar();
            var terminiZaDatum = new List<TerminPacijentLekarDTO>();
            foreach (TerminPacijentLekarDTO termin in sviTermini)
            {
                if (termin.termin.DatumIVremeTermina >= prviDanUNedelji && termin.termin.DatumIVremeTermina <= zadnjiDanUNedelji)
                {
                    terminiZaDatum.Add(termin);
                }
            }
            return terminiZaDatum;
        }

        public IEnumerable GetBuduciTerminPacLekarZaMesec(DateTime izabraniDatum)
        {
            IEnumerable sviTermini = TerminServis.GetBuduciTerminPacLekar();
            var terminiZaDatum = new List<TerminPacijentLekarDTO>();
            foreach (TerminPacijentLekarDTO termin in sviTermini)
            {
                if (termin.termin.DatumIVremeTermina.Month == izabraniDatum.Month &&
                    termin.termin.DatumIVremeTermina.Year == izabraniDatum.Year)
                {
                    terminiZaDatum.Add(termin);
                }
            }

            return terminiZaDatum;
        }

        public IEnumerable GetBuduciTerminPacLekarZaGodinu(DateTime izabranaGodina)
        {
            IEnumerable sviTermini = TerminServis.GetBuduciTerminPacLekar();
            var terminiZaDatum = new List<TerminPacijentLekarDTO>();
            foreach (TerminPacijentLekarDTO termin in sviTermini)
            {
                if (termin.termin.DatumIVremeTermina.Year == izabranaGodina.Year)
                {
                    terminiZaDatum.Add(termin);
                }
            }

            return terminiZaDatum;
        }

        public List<Termin> GetByJmbgLekar(string tJmbgLekara)
        {
            return TerminServis.GetByJmbgLekar(tJmbgLekara);
        }
    }
}