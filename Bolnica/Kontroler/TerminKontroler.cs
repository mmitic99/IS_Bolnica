using Bolnica.DTOs;
using Model;
using Model.Enum;
using Servis;
using System;
using System.Collections;
using System.Collections.Generic;

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

        public bool ZakaziTermin(Termin termin)
        {
            termin.IdProstorije = ProstorijeServis.GetInstance().GetPrvaPogodna(termin);
            return terminServis.ZakaziTermin(termin);
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            return terminServis.OtkaziTermin(termin);
        }

        public bool IzmeniTermin(Object termin, Object stariIdTermina = null)
        {
            return terminServis.IzmeniTermin((Termin)termin, ((Termin)stariIdTermina).IDTermina);
        }

        public List<Termin> GetAll()
        {
            // TODO: implement
            return terminServis.GetAll();
        }

        public void Save(Termin termin)
        {
            // TODO: implement
            terminServis.Save(termin);
        }

        public void SaveAll(List<Termin> termini)
        {
            // TODO: implement
            terminServis.SaveAll(termini);
        }

        internal List<Termin> GetByJmbg(string jmbg)
        {

            return TerminServis.getInstance().getByJmbgPacijenta(jmbg);
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

        public List<Termin> NadjiTermineZaParametre(String jmbgLekara, String jmbgPacijenta, List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, int prioritet, String tegobe, Termin termin = null, bool sekretar = false)
        {
            ParametriZaTrazenjeTerminaKlasifikovanoDTO paramet = new ParametriZaTrazenjeTerminaKlasifikovanoDTO()
            {
                JmbgPacijenta = jmbgPacijenta,
                JmbgLekara = jmbgLekara,
                IzabraniDani = dani,
                Pocetak = pocetak,
                Kraj = kraj,
                Prioritet = prioritet,
                tegobe = tegobe,
                PrethodnoZakazaniTermin = termin,
                trajanjeUMinutama = 30,
                sekretar = sekretar
            };
            return TerminServis.getInstance().NadjiTermineZaParametre(paramet);
        }

        public Termin GetById(String idTermina)
        {
            return terminServis.GetById(idTermina);
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
                JmbgLekara = ((Lekar)parametriDTO.IzabraniLekar).Jmbg,
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

        public List<Termin> NadjiHitanTermin(string jmbgPacijenta, string vrstaSpecijalizacije)
        {
            return terminServis.NadjiHitanTermin(jmbgPacijenta, vrstaSpecijalizacije);
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

        public List<Termin> NadjiTermineZaParametre(ParametriZaTrazenjeMogucihTerminaDTO parametriDTO)
        {
            return TerminServis.getInstance().NadjiTermineZaParametre(KlasifikujUlazneParametre(parametriDTO));
        }

        public Termin GetTerminZaDatumILekara(DateTime datumIVreme, string jmbgLekara)
        {
            return terminServis.GetTerminZaDatumILekara(datumIVreme, jmbgLekara);
        }
    }
}