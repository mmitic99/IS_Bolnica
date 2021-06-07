using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.view.UpravnikView;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Validacije;

namespace Servis
{
    public class ProstorijeServis
    {

        private static ProstorijeServis instance = null;
        public int IdProstorijeGenerator = 0;

        public ISkladisteZaProstorije skladisteZaProstorije;
        public ISkladisteZaTermine skladisteZaTermine;
        public ISkladisteZaNaprednaRenoviranja skladisteZaNaprednaRenoviranja;
        public ISkladisteZaRenoviranja skladisteZaRenoviranja;
        public ISkladisteZaZakazanuPreraspodeluStatickeOpreme skladisteZaZakazanuPreraspodeluStatickeOpreme;

        public ValidacijaContext validacija = new ValidacijaContext(new ProstorijaStrategy());
        public static ProstorijeServis GetInstance()
        {
            if (instance == null)
            {
                instance = new ProstorijeServis();
            }
            return instance;
        }

        public ProstorijeServis()
        {
            skladisteZaProstorije = new SkladisteZaProstorijeXml();
            skladisteZaTermine = new SkladisteZaTermineXml();
            skladisteZaNaprednaRenoviranja = new SkladisteZaNaprednaRenoviranjaXml();
            skladisteZaRenoviranja = new SkladisteZaRenoviranjaXml();
            skladisteZaZakazanuPreraspodeluStatickeOpreme = new SkladisteZaZakazanuPreraspodeluStatickeOpremeXml();
        }

        public List<Prostorija> GetByVrstaProstorije(VrstaProstorije vrstaProstorije)
        {
            List<Prostorija> sveProstorije = GetAll();
            List<Prostorija> odgovarajuceProstorije = new List<Prostorija>();
            foreach (Prostorija p in sveProstorije)
            {
                if (p.VrstaProstorije == vrstaProstorije)
                {
                    odgovarajuceProstorije.Add(p);
                }
            }
            return odgovarajuceProstorije;
        }

        private VrstaProstorije GetPogodnaVrstaProstorije(VrstaPregleda vrstaPregleda)
        {
            VrstaProstorije vrsta;
            if (vrstaPregleda == VrstaPregleda.Operacija)
                vrsta = VrstaProstorije.Operaciona_sala;
            else
                vrsta = VrstaProstorije.Soba_za_preglede;
            return vrsta;
        }

        internal int GetPrvaPogodna(Termin termin)
        {
            int id = -1;
            List<Prostorija> pogodneProsotije = GetByVrstaProstorije(GetPogodnaVrstaProstorije(termin.VrstaTermina));
            foreach (Prostorija p in pogodneProsotije)
            {
                if (DaLiJeSLobodnaProstorija(p.IdProstorije, termin.DatumIVremeTermina, termin.TrajanjeTermina))
                {
                    id = p.IdProstorije;
                }
            }
            return id;
        }

        public bool PostojiSlobodnaProstorija(ParamsToCheckAvailabilityOfRoomDTO parameters)
        {
            bool postojiSlobodna = false;
            List<Prostorija> pogodneProstorije = GetByVrstaProstorije(GetPogodnaVrstaProstorije(parameters.InterventionType));
            foreach (Prostorija p in pogodneProstorije)
            {
                if (DaLiJeSLobodnaProstorija(p.IdProstorije, parameters.startTime, parameters.durationInMinutes))
                {
                    postojiSlobodna = true;
                    break;
                }
            }
            return postojiSlobodna;
        }

        public void RenovirajProstoriju(Renoviranje renoviranje)
        {
            List<Renoviranje> SvaRenoviranja = skladisteZaRenoviranja.GetAll();
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            int id = -1;
            foreach (Prostorija p in SveProstorije)
            {
                if (renoviranje.BrojProstorije == p.BrojSobe)
                    id = p.IdProstorije;
            }
            if (DaLiJeSLobodnaProstorijaZaRenoviranje(id, renoviranje.DatumPocetkaRenoviranja, renoviranje.DatumZavrsetkaRenoviranja))
            {
                SvaRenoviranja.Add(renoviranje);
                skladisteZaRenoviranja.SaveAll(SvaRenoviranja);
            }
            else  //5
                validacija.IspisiGresku(5);
        }

        public void ZavrsiRenoviranje(int index)
        {
            List<Renoviranje> SvaRenoviranja = skladisteZaRenoviranja.GetAll();
            int IdProstorije = SvaRenoviranja[index].IdProstorije;
            SvaRenoviranja.RemoveAt(index);
            skladisteZaRenoviranja.SaveAll(SvaRenoviranja);
            DeselektujMoguceRenoviranje(IdProstorije);
        }
        private void ObrisiZavrsenaRenoviranja(ref List<Renoviranje> SvaRenoviranja, List<int> IdProstorijaZaBrisanjeIzRenoviranja)
        {
            foreach (int ID in IdProstorijaZaBrisanjeIzRenoviranja)
            {
                for (int i = 0; i < SvaRenoviranja.Count; i++)
                {
                    if (SvaRenoviranja[i].IdProstorije == ID)
                        SvaRenoviranja.RemoveAt(i);
                }
            }
        }

        public void AzurirajRenoviranjaProstorija()
        {
            List<Renoviranje> SvaRenoviranja = skladisteZaRenoviranja.GetAll();
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            List<int> IdProstorijaZaBrisanjeIzRenoviranja = new List<int>();
            for (int i = 0; i < SvaRenoviranja.Count; i++)
            {
                if (DateTime.Now.Date >= SvaRenoviranja[i].DatumPocetkaRenoviranja.Date && DateTime.Now.Date <= SvaRenoviranja[i].DatumZavrsetkaRenoviranja.Date)
                {
                    for (int j = 0; j < SveProstorije.Count; j++)
                    {
                        if (SveProstorije[j].IdProstorije.Equals(SvaRenoviranja[i].IdProstorije))
                            SveProstorije[j].RenoviraSe = true;
                    }
                }
                else if (DateTime.Now.Date > SvaRenoviranja[i].DatumZavrsetkaRenoviranja)
                {
                    IdProstorijaZaBrisanjeIzRenoviranja.Add(SvaRenoviranja[i].IdProstorije);
                    for (int j = 0; j < SveProstorije.Count; j++)
                    {
                        if (SveProstorije[j].IdProstorije.Equals(SvaRenoviranja[i].IdProstorije))
                        {
                            SveProstorije[j].RenoviraSe = false;
                        }
                    }
                }
            }
            ObrisiZavrsenaRenoviranja(ref SvaRenoviranja, IdProstorijaZaBrisanjeIzRenoviranja);
            skladisteZaProstorije.SaveAll(SveProstorije);
            skladisteZaRenoviranja.SaveAll(SvaRenoviranja);
        }

        public void DeselektujMoguceRenoviranje(int id)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].IdProstorije.Equals(id))
                    SveProstorije[i].RenoviraSe = false;
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public List<Termin> GetTerminiByIdProstorije(int id)
        {
            List<Termin> TerminiProstorije = new List<Termin>();
            List<Termin> SviTermini = skladisteZaTermine.GetAll();
            foreach (Termin t in SviTermini)
            {
                if (t.IdProstorije.Equals(id))
                {
                    TerminiProstorije.Add(t);
                }
            }
            return TerminiProstorije;
        }

        public List<ZakazanaPreraspodelaStatickeOpreme> GetPreraspodeleByIdProstorije(int id)
        {
            List<ZakazanaPreraspodelaStatickeOpreme> svePreraspodele = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = new List<ZakazanaPreraspodelaStatickeOpreme>();
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in svePreraspodele)
            {
                if (prer.IdProstorijeIzKojeSePrenosiOprema.Equals(id) || prer.IdProstorijeUKojUSePrenosiOprema.Equals(id))
                {
                    preraspodeleProstorije.Add(prer);
                }
            }
            return preraspodeleProstorije;
        }

        public Renoviranje GetRenoviranjeByIdProstorije(int id)
        {
            Renoviranje ren = new Renoviranje();
            List<Renoviranje> Renoviranja = skladisteZaRenoviranja.GetAll();
            foreach (Renoviranje renoviranje in Renoviranja)
            {
                if (renoviranje.IdProstorije == id)
                {
                    ren = renoviranje;
                }
            }
            return ren;
        }

        public bool ProveriTermine(DateTime datumIVremePreraspodele, List<Termin> terminiProstorije)
        {
            bool validno = true;
            foreach (Termin t in terminiProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) > 0 && DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    validno = false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(60), t.DatumIVremeTermina) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    validno = false;
                }
                if (DateTime.Compare(t.DatumIVremeTermina, datumIVremePreraspodele) == 0)
                {
                    validno = false;
                }
            }
            return validno;
        }

        public bool ProveriPreraspodeleOpreme(DateTime datumIVremePreraspodele, List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije)
        {
            bool validno = true;
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in preraspodeleProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) > 0 && DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele.AddMinutes(prer.TrajanjePreraspodele)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    validno = false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(60), prer.DatumIVremePreraspodele) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    validno = false;
                }
                if (DateTime.Compare(prer.DatumIVremePreraspodele, datumIVremePreraspodele) == 0)
                {
                    validno = false;
                }
            }
            return validno;
        }

        public bool ProveriRenoviranje(Renoviranje renoviranjeZaProstoriju, DateTime datumIVremePreraspodele)
        {
            bool validno = true;
            if (renoviranjeZaProstoriju != null)
            {
                DateTime pomerenZbogPonoci = renoviranjeZaProstoriju.DatumZavrsetkaRenoviranja.AddHours(23);
                if (DateTime.Compare(renoviranjeZaProstoriju.DatumPocetkaRenoviranja, datumIVremePreraspodele) <= 0 && DateTime.Compare(pomerenZbogPonoci, datumIVremePreraspodele) >= 0)
                {
                    validno = false;
                }
            }
            return validno;
        }

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime datumIVremePreraspodele, double trajanje)
        {
            bool skrozSlobodan = true;
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
            Renoviranje renoviranjeZaProstoriju = GetRenoviranjeByIdProstorije(iDProstorije);
            slobodan = ProveriTermine(datumIVremePreraspodele, terminiProstorije);
            if (slobodan == false)
                skrozSlobodan = false;
            slobodan = ProveriPreraspodeleOpreme(datumIVremePreraspodele, preraspodeleProstorije);
            if (slobodan == false)
                skrozSlobodan = false;
            slobodan = ProveriRenoviranje(renoviranjeZaProstoriju, datumIVremePreraspodele);
            if (slobodan == false)
                skrozSlobodan = false;
            return skrozSlobodan;
        }

        public bool DaLiJeSLobodnaProstorijaZaRenoviranje(int iDProstorije, DateTime DatumPocetka, DateTime DatumKraja)
        {
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
            Renoviranje renoviranjeZaProstoriju = GetRenoviranjeByIdProstorije(iDProstorije);
            if (renoviranjeZaProstoriju != null)
            { //6
                validacija.IspisiGresku(6);
                slobodan = false;
            }
            foreach (Termin t in terminiProstorije)
            {
                if (DatumPocetka.Date <= t.DatumIVremeTermina.Date && DatumKraja.Date >= t.DatumIVremeTermina.Date)
                {
                    slobodan = false;
                    break;
                }
            }
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in preraspodeleProstorije)
            {
                if (DatumPocetka.Date <= prer.DatumIVremePreraspodele.Date && DatumKraja.Date >= prer.DatumIVremePreraspodele.Date)
                {
                    slobodan = false;
                    break;
                }
            }
            return slobodan;
        }

        public List<Prostorija> GetAll()
        {
            return skladisteZaProstorije.GetAll();
        }

        public void Save(Prostorija prostorija)
        {
            skladisteZaProstorije.Save(prostorija);
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            skladisteZaProstorije.SaveAll(prostorije);
        }

        public void DodajStacionarnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka.Add(new StacionarnaOprema(tipOpreme, kolicina));
                    StacionarnaMagacin = prostorije[i].Staticka;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka.RemoveAt(index);
                    StacionarnaMagacin = prostorije[i].Staticka;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
        }
        public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna.Add(new PotrosnaOprema(tipOpreme, kolicina));
                    PotrosnaMagacin = prostorije[i].Potrosna;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
        }

        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna.RemoveAt(index);
                    PotrosnaMagacin = prostorije[i].Potrosna;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka[index].Kolicina + kolicina) >= 0)
                {
                    prostorije[i].Staticka[index].Kolicina += kolicina;
                    StacionarnaMagacin = prostorije[i].Staticka;
                    skladisteZaProstorije.SaveAll(prostorije);
                    break;
                }  //7
                else if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka[index].Kolicina + kolicina) < 0)
                    validacija.IspisiGresku(7);
            }
        }

        public void IzmeniDinamickuOpremuUMagacinu(int index, int kolicina)
        {
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna[index].KolicinaOpreme += kolicina) >= 0)
                {
                    PotrosnaMagacin = prostorije[i].Potrosna;
                    skladisteZaProstorije.SaveAll(prostorije);
                    break;
                } //8
                else if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna[index].KolicinaOpreme += kolicina) < 0)
                    validacija.IspisiGresku(8);
            }
        }

        public Prostorija GetMagacin()
        {
            Prostorija p = new Prostorija();
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                    p = soba;
            }
            return p;
        }

        public void DodajProstoriju(Prostorija p)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            Prostorija prostorija = p;
            int indexPoslednjeProstorije = SveProstorije.Count - 1;
            IdProstorijeGenerator = SveProstorije[indexPoslednjeProstorije].IdProstorije;
            prostorija.IdProstorije = ++IdProstorijeGenerator;
            SveProstorije.Add(prostorija);
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            int StariIdProstorije = SveProstorije[index].IdProstorije;
            bool RenoviranjeStatus = SveProstorije[index].RenoviraSe;
            SveProstorije[index] = p;
            SveProstorije[index].IdProstorije = StariIdProstorije;
            SveProstorije[index].RenoviraSe = RenoviranjeStatus;
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void IzbrisiProstoriju(int index)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            SveProstorije.RemoveAt(index);
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void PrebaciStacionarnuOpremuUProstoriju(PrebacivanjeOpremeInfoDTO prebacivanjeInfo, int indexOpreme)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            bool nazivOpremeVecPrisutan = false;
            int indexPronadjeneOpreme = -1;
            for (int i = 0; i < prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Count; i++)
            {
                if (prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka[i].TipStacionarneOpreme.Equals(prebacivanjeInfo.NazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    indexPronadjeneOpreme = i;
                }
            }
            if (prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina - prebacivanjeInfo.KolicinaOpreme >= 0)
            {
                if (nazivOpremeVecPrisutan == true)
                    prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka[indexPronadjeneOpreme].Kolicina += prebacivanjeInfo.KolicinaOpreme;
                else
                    prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Add(new StacionarnaOprema(prebacivanjeInfo.NazivOpreme, prebacivanjeInfo.KolicinaOpreme));

                prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina -= prebacivanjeInfo.KolicinaOpreme;
                skladisteZaProstorije.SaveAll(prostorije);
            }
            else
            { //9
                validacija.IspisiGresku(9);
            }
        }

        public int GetIndexOpremeKojaSePrebacuje(int indexIzKojeProstorije, string nazivOpreme)
        {
            int index = -1;
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < SveProstorije[indexIzKojeProstorije].Staticka.Count; i++)
            {
                if (SveProstorije[indexIzKojeProstorije].Staticka[i].TipStacionarneOpreme.Equals(nazivOpreme))
                    index = i;
            }
            return index;
        }
        public void IzvrsiPrebacivanjeOpreme(PrebacivanjeOpremeInfoDTO prebacivanjeInfo, int indexOpreme, int i)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            if (prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina - prebacivanjeInfo.KolicinaOpreme >= 0)
            {
                if (i == -1)
                    prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Add(new StacionarnaOprema(prebacivanjeInfo.NazivOpreme, prebacivanjeInfo.KolicinaOpreme));
                else
                    prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka[i].Kolicina += prebacivanjeInfo.KolicinaOpreme;

                prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina -= prebacivanjeInfo.KolicinaOpreme;
                if (prostorije[prebacivanjeInfo.IndexIzKojeProstorije].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    StacionarnaMagacin = prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka;
                }
                if (prostorije[prebacivanjeInfo.IndexUKojuProstoriju].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    StacionarnaMagacin = prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka;
                }
                skladisteZaProstorije.SaveAll(prostorije);
            }
            else
            { //9
                validacija.IspisiGresku(9);
            }
        }


        public void PrebaciZakazanuStacionarnuOpremuUProstoriju(PrebacivanjeOpremeInfoDTO prebacivanjeInfo)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = GetIndexOpremeKojaSePrebacuje(prebacivanjeInfo.IndexIzKojeProstorije, prebacivanjeInfo.NazivOpreme);
            for (int i = 0; i < prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Count; i++)
            {
                if (prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka[i].TipStacionarneOpreme.Equals(prebacivanjeInfo.NazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    IzvrsiPrebacivanjeOpreme(prebacivanjeInfo, indexOpreme, i);
                }
            }
            if (nazivOpremeVecPrisutan == false)
            {
                IzvrsiPrebacivanjeOpreme(prebacivanjeInfo, indexOpreme, -1);
            }
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            prostorije[indexProstorije].Staticka.RemoveAt(indexOpreme);
            if (prostorije[indexProstorije].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                StacionarnaMagacin = prostorije[indexProstorije].Staticka;
            skladisteZaProstorije.SaveAll(prostorije);
        }

        public void IzmeniStacionarnuOpremuProstorije(IzmenaOpremeInfoDTO izmenaOpremeInfo)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            if ((prostorije[izmenaOpremeInfo.IndexProstorije].Staticka[izmenaOpremeInfo.IndexOpreme].Kolicina + izmenaOpremeInfo.KolicinaOpreme) < 0)
                validacija.IspisiGresku(10);
            else //10
            {
                prostorije[izmenaOpremeInfo.IndexProstorije].Staticka[izmenaOpremeInfo.IndexOpreme].Kolicina += izmenaOpremeInfo.KolicinaOpreme;
                if (prostorije[izmenaOpremeInfo.IndexProstorije].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                    StacionarnaMagacin = prostorije[izmenaOpremeInfo.IndexProstorije].Staticka;
                skladisteZaProstorije.SaveAll(prostorije);
            }
        }

        private bool Validiraj(Regex sablon, String unos)
        {
            bool validno = true;
            if (!sablon.IsMatch(unos))
                validno = false;
            return validno;
        }

        public bool ProveriValidnostProstorije(ProstorijaValidacijaDTO prostorija)
        {
            int idGreske = 0;
            bool validno = true;
            if (ValidirajBrojProstorije(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe) == false)
            {
                validno = false;
                idGreske = 1;
            }

            if (Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat) == false)
            {
                validno = false;
                idGreske = 2;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura) == false)
            {
                validno = false;
                idGreske = 3;
            }

            if (prostorija.VrstaProstorije == -1)
            {
                validno = false;
                idGreske = 4;
            }
            validacija.IspisiGresku(idGreske);
            return validno;
        }

        public bool ValidirajBrojProstorije(Regex sablon, String unos)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            bool validno = true;
            if (sablon.IsMatch(unos))
            {
                foreach (Prostorija Soba in SveProstorije)
                {
                    if (unos.Equals(Soba.BrojSobe))
                    { //11
                        validacija.IspisiGresku(11);
                        validno = false;
                    }
                }
            }
            return validno;
        }

        private bool ValidirajBrojProstorijeIzmena(Regex sablon, String unos, int indexSelektovaneProstorije)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            bool validno = true;
            if (sablon.IsMatch(unos))
            {
                foreach (Prostorija Soba in SveProstorije)
                {
                    if (Soba.BrojSobe != SveProstorije[indexSelektovaneProstorije].BrojSobe && unos.Equals(Soba.BrojSobe))
                    { //11
                        validacija.IspisiGresku(11);
                        validno = false;
                    }
                }
            }
            return validno;
        }

        public bool ProveriValidnostIzmeneProstorije(ProstorijaValidacijaDTO prostorija, int indexProstorije)
        {
            bool validno = true;
            int idGreske = 0;
            if (ValidirajBrojProstorijeIzmena(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe, indexProstorije) == false)
            {
                validno = false;
                idGreske = 1;
            }

            if (Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat) == false)
            {
                validno = false;
                idGreske = 2;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura) == false)
            {
                validno = false;
                idGreske = 3;
            }

            if (prostorija.VrstaProstorije == -1)
            {
                validno = false;
                idGreske = 4;
            }
            validacija.IspisiGresku(idGreske);
            return validno;
        }

        public bool ProveriValidnostOpreme(String NazivOpreme, String Kolicina)
        {
            bool validno = true;
            int idGreske = 0;
            bool checkNaziv = false;
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč]*$");
            if (!sablon.IsMatch(NazivOpreme))
            { //12
                validno = false;
                idGreske = 12;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (!sablon.IsMatch(Kolicina))
            { //13
                validno = false;
                idGreske = 13;
            }
            validacija.IspisiGresku(idGreske);
            return validno;
        }

        public bool ProveriValidnostKolicineOpreme(String Kolicina)
        {
            bool validno = true;
            Regex sablon = new Regex(@"^[+-]?[0-9]*$");
            if (Validiraj(new Regex(@"^[+-]?[0-9]*$"), Kolicina) == false || Kolicina.Equals(""))
            { //14
                validacija.IspisiGresku(14);
                validno = false;
            }
            else
                validno = true;
            return validno;
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            bool validno = true;
            if (Validiraj(new Regex(@"^[+]?[1-9]{1}[0-9]*$"), Kolicina) == false || Kolicina.Equals(""))
            { //15
                validacija.IspisiGresku(15);
                validno = false;
            }
            else
                validno = true;
            return validno;
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            VrstaProstorije vrsta = Model.Enum.VrstaProstorije.Magacin;
            if (IndexSelektovaneVrsteProstorije == 0)
                vrsta = Model.Enum.VrstaProstorije.Soba_za_preglede;
            else if (IndexSelektovaneVrsteProstorije == 1)
                vrsta = Model.Enum.VrstaProstorije.Operaciona_sala;
            else if (IndexSelektovaneVrsteProstorije == 2)
                vrsta = Model.Enum.VrstaProstorije.Soba_za_bolesnike;
            return vrsta;
        }

        public List<StacionarnaOprema> GetStacionarnaOpremaProstorije(int index)
        {
            List<StacionarnaOprema> oprema = new List<StacionarnaOprema>();
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            oprema = prostorije[index].Staticka;
            return oprema;
        }

        public int GetIdProstorijeByBrojProstorije(String brojProstorije)
        {
            int id = -1;
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    id = p.IdProstorije;
            }
            return id;
        }

        public Model.Enum.VrstaProstorije GetVrstaProstorijeByBrojProstorije(String brojProstorije)
        {
            VrstaProstorije vrsta = Model.Enum.VrstaProstorije.Soba_za_preglede;
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    vrsta = p.VrstaProstorije;
            }
            return vrsta;
        }

        public int GetSpratProstorijeByBrojProstorije(String brojProstorije)
        {
            int sprat = 0;
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    sprat = p.Sprat;
            }
            return sprat;
        }

        private Prostorija GetProstorijaById(int id)
        {
            Prostorija prostorija = new Prostorija();
            foreach (Prostorija p in skladisteZaProstorije.GetAll())
            {
                if (p.IdProstorije == id)
                    prostorija = p;
            }
            return prostorija;
        }

        private void PrebaciSvuOpremuUMagacin(int idProstorije)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            Prostorija magacin = GetMagacin();
            Prostorija prostorija = GetProstorijaById(idProstorije);
            bool vecPostojiOpremaUMagacinu = false;
            int indexPronadjeneOpreme = -1;
            foreach (StacionarnaOprema prostorijaOprema in prostorija.Staticka)
            {
                for (int i = 0; i < magacin.Staticka.Count; i++)
                {
                    if (prostorijaOprema.TipStacionarneOpreme.Equals(magacin.Staticka[i].TipStacionarneOpreme))
                    {
                        vecPostojiOpremaUMagacinu = true;
                        indexPronadjeneOpreme = i;
                    }
                }
                if (vecPostojiOpremaUMagacinu == true)
                    magacin.Staticka[indexPronadjeneOpreme].Kolicina += prostorijaOprema.Kolicina;
                else
                    magacin.Staticka.Add(new StacionarnaOprema(prostorijaOprema.TipStacionarneOpreme, prostorijaOprema.Kolicina));
                vecPostojiOpremaUMagacinu = false;
            }
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                {
                    SveProstorije[i] = magacin;
                    break;
                }
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        private void ObrisiPodeljenuProstoriju(int id)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].IdProstorije == id)
                {
                    SveProstorije.RemoveAt(i);
                    break;
                }
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        private int GetSpratById(int id)
        {
            int sprat = 0;
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].IdProstorije == id)
                {
                    sprat = SveProstorije[i].Sprat;
                }
            return sprat;
        }

        private void DodajNovonastaluProstoriju(String broj, int sprat)
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            Prostorija p = new Prostorija(broj, sprat, Model.Enum.VrstaProstorije.Soba_za_preglede, 0);
            int indexPoslednjeProstorije = SveProstorije.Count - 1;
            IdProstorijeGenerator = SveProstorije[indexPoslednjeProstorije].IdProstorije;
            p.IdProstorije = ++IdProstorijeGenerator;
        }

        private void PodeliProstoriju(NaprednoRenoviranje renoviranje)
        {
            PrebaciSvuOpremuUMagacin(GetIdProstorijeByBrojProstorije(renoviranje.BrojGlavneProstorije));
            ObrisiPodeljenuProstoriju(GetIdProstorijeByBrojProstorije(renoviranje.BrojGlavneProstorije));
            DodajProstoriju(new Prostorija(renoviranje.BrojProstorije1, GetSpratById(GetIdProstorijeByBrojProstorije(renoviranje.BrojGlavneProstorije)), 
                                           Model.Enum.VrstaProstorije.Soba_za_preglede, 0));
            DodajProstoriju(new Prostorija(renoviranje.BrojProstorije2, GetSpratById(GetIdProstorijeByBrojProstorije(renoviranje.BrojGlavneProstorije)), 
                                           Model.Enum.VrstaProstorije.Soba_za_preglede, 0));
        }

        private void SpojiProstorije(NaprednoRenoviranje renoviranje)
        {
            PrebaciSvuOpremuUMagacin(GetIdProstorijeByBrojProstorije(renoviranje.BrojProstorije1));
            PrebaciSvuOpremuUMagacin(GetIdProstorijeByBrojProstorije(renoviranje.BrojProstorije2));
            ObrisiPodeljenuProstoriju(GetIdProstorijeByBrojProstorije(renoviranje.BrojProstorije1));
            ObrisiPodeljenuProstoriju(GetIdProstorijeByBrojProstorije(renoviranje.BrojProstorije2));
            DodajProstoriju(new Prostorija(renoviranje.BrojGlavneProstorije, GetSpratById(GetIdProstorijeByBrojProstorije(renoviranje.BrojProstorije1)),
                                           Model.Enum.VrstaProstorije.Soba_za_preglede, 0));
        }


        public async Task AzurirajNaprednaRenoviranjaProstorija()
        {
            foreach (NaprednoRenoviranje renoviranje in skladisteZaNaprednaRenoviranja.GetAll())
            {
                if (DateTime.Compare(renoviranje.DatumZavrsetkaRenoviranja, DateTime.Now) <= 0)
                {
                    if (renoviranje.Podela == true)
                    {
                        PodeliProstoriju(renoviranje);
                    }
                    else if (renoviranje.Spajanje == true)
                    {
                        SpojiProstorije(renoviranje);
                    }
                }
                else
                {
                    await Task.Delay(renoviranje.DatumZavrsetkaRenoviranja - DateTime.Now);
                    if (renoviranje.Podela == true)
                    {
                        PodeliProstoriju(renoviranje);
                    }
                    else if (renoviranje.Spajanje == true)
                    {
                        SpojiProstorije(renoviranje);
                    }
                }
            }
            ObrisiZavrsenaNaprednaRenoviranja();
            AzurirajRenoviranjeFlegProstorije();
        }

        private void ObrisiZavrsenaNaprednaRenoviranja()
        {
            List<NaprednoRenoviranje> SvaRenoviranja = skladisteZaNaprednaRenoviranja.GetAll();
            List<int> indexiZaBrisanje = new List<int>();
            for (int k = 0; k < SvaRenoviranja.Count; k++)
            {
                List<NaprednoRenoviranje> pomocnaLista = skladisteZaNaprednaRenoviranja.GetAll();
                for (int i = 0; i < skladisteZaNaprednaRenoviranja.GetAll().Count; i++)
                {
                    if (DateTime.Compare(pomocnaLista[i].DatumZavrsetkaRenoviranja.AddHours(23).AddMinutes(59), DateTime.Now) < 0)
                    {
                        pomocnaLista.RemoveAt(i);
                        break;
                    }
                }
                skladisteZaNaprednaRenoviranja.SaveAll(pomocnaLista);
            }
        }

        private void PodesiPodelu(ref List<NaprednoRenoviranje> SvaRenoviranja, ref List<Prostorija> SveProstorije, int i)
        {
            if (DateTime.Now.Date >= SvaRenoviranja[i].DatumPocetkaRenoviranja.Date && DateTime.Now.Date <= SvaRenoviranja[i].DatumZavrsetkaRenoviranja.Date)
                for (int j = 0; j < SveProstorije.Count; j++)
                {
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojGlavneProstorije))
                        SveProstorije[j].RenoviraSe = true;
                }
            else if (DateTime.Now.Date > SvaRenoviranja[i].DatumZavrsetkaRenoviranja)
                for (int j = 0; j < SveProstorije.Count; j++)
                {
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojGlavneProstorije))
                        SveProstorije[j].RenoviraSe = false;
                }
        }

        private void PodesiSpajanje(ref List<NaprednoRenoviranje> SvaRenoviranja, ref List<Prostorija> SveProstorije, int i)
        {
            if (DateTime.Now.Date >= SvaRenoviranja[i].DatumPocetkaRenoviranja.Date && DateTime.Now.Date <= SvaRenoviranja[i].DatumZavrsetkaRenoviranja.Date)
                for (int j = 0; j < SveProstorije.Count; j++)
                {
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojProstorije1))
                        SveProstorije[j].RenoviraSe = true;
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojProstorije2))
                        SveProstorije[j].RenoviraSe = true;
                }
            else if (DateTime.Now.Date > SvaRenoviranja[i].DatumZavrsetkaRenoviranja)
                for (int j = 0; j < SveProstorije.Count; j++)
                {
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojProstorije1))
                        SveProstorije[j].RenoviraSe = false;
                    if (SveProstorije[j].BrojSobe.Equals(SvaRenoviranja[i].BrojProstorije2))
                        SveProstorije[j].RenoviraSe = false;
                }
        }

        public void AzurirajRenoviranjeFlegProstorije()
        {
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            List<NaprednoRenoviranje> SvaRenoviranja = skladisteZaNaprednaRenoviranja.GetAll();
            for (int i = 0; i < SvaRenoviranja.Count; i++)
            {
                if (SvaRenoviranja[i].Podela)
                {
                    PodesiSpajanje(ref SvaRenoviranja, ref SveProstorije, i);
                }
                else if (SvaRenoviranja[i].Spajanje)
                {
                    PodesiPodelu(ref SvaRenoviranja, ref SveProstorije, i);
                }
            }
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public async Task AzurirajStanjeOpremeAkoJeBiloPrebacivanja()
        {
            List<ZakazanaPreraspodelaStatickeOpreme> SvePreraspodele = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            int indexUKoju = -1;
            int indexIzKoje = -1;
            List<Prostorija> SveProstorije = skladisteZaProstorije.GetAll();
            foreach (ZakazanaPreraspodelaStatickeOpreme preraspodela in SvePreraspodele)
            {
                for (int i = 0; i < SveProstorije.Count; i++)
                {
                    if (preraspodela.IdProstorijeUKojUSePrenosiOprema == SveProstorije[i].IdProstorije)
                        indexUKoju = i;
                    else if (preraspodela.IdProstorijeIzKojeSePrenosiOprema == SveProstorije[i].IdProstorije)
                        indexIzKoje = i;
                }
                PrebaciZakazanuOpremu(indexIzKoje, indexUKoju, preraspodela);
            }
        }

        public async Task PrebaciZakazanuOpremu(int indexIzKojeProstorije, int indexUKojuProstoriju, ZakazanaPreraspodelaStatickeOpreme preraspodela)
        {
            PrebacivanjeOpremeInfoDTO prebacivanjeInfo = new PrebacivanjeOpremeInfoDTO
                                                             (
                                                             indexIzKojeProstorije,
                                                             indexUKojuProstoriju,
                                                             preraspodela.NazivOpreme,
                                                             preraspodela.KolicinaOpreme
                                                             );
            if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.AddMinutes(60), DateTime.Now) <= 0)
            {
                PrebaciZakazanuStacionarnuOpremuUProstoriju(prebacivanjeInfo);
                AzurirajPreraspodeleOpreme();
            }
            else
            {
                await Task.Delay(preraspodela.DatumIVremePreraspodele.AddMinutes(60) - DateTime.Now);
                //await Task.Delay(10000);
                PrebaciZakazanuStacionarnuOpremuUProstoriju(prebacivanjeInfo);
                AzurirajPreraspodeleOpreme();
            }
        }
        public void PopuniInformacijeZaBrisanjePreraspodela(List<int> IdIzKojeProstorije, List<int> IdUKojuProstoriju, List<DateTime> vremePreraspodele)
        {
            List <ZakazanaPreraspodelaStatickeOpreme> PreraspodeleStatickeOpreme = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            for (int i = 0; i < PreraspodeleStatickeOpreme.Count; i++)
            {
                if (DateTime.Now > PreraspodeleStatickeOpreme[i].DatumIVremePreraspodele.AddMinutes(59))
                {
                    IdIzKojeProstorije.Add(PreraspodeleStatickeOpreme[i].IdProstorijeIzKojeSePrenosiOprema);
                    IdUKojuProstoriju.Add(PreraspodeleStatickeOpreme[i].IdProstorijeUKojUSePrenosiOprema);
                    vremePreraspodele.Add(PreraspodeleStatickeOpreme[i].DatumIVremePreraspodele);
                }
            }
        }

        public void IzbrisiPotrebnePreraspodele(List<int> IdIzKojeProstorije, List<int> IdUKojuProstoriju, List<DateTime> vremePreraspodele)
        {
            List<ZakazanaPreraspodelaStatickeOpreme> PreraspodeleStatickeOpreme = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            for (int k = 0; k < IdIzKojeProstorije.Count; k++)
            {
                for (int i = 0; i < PreraspodeleStatickeOpreme.Count; i++)
                {
                    if (PreraspodeleStatickeOpreme[i].IdProstorijeIzKojeSePrenosiOprema == IdIzKojeProstorije[k] &&
                        PreraspodeleStatickeOpreme[i].IdProstorijeUKojUSePrenosiOprema == IdUKojuProstoriju[k] &&
                        PreraspodeleStatickeOpreme[i].DatumIVremePreraspodele == vremePreraspodele[k])
                        PreraspodeleStatickeOpreme.RemoveAt(i);
                }
            }
            skladisteZaZakazanuPreraspodeluStatickeOpreme.SaveAll(PreraspodeleStatickeOpreme);
        }

        public void AzurirajPreraspodeleOpreme()
        {
            List<int> IdIzKojeProstorije = new List<int>();
            List<int> IdUKojuProstoriju = new List<int>();
            List<DateTime> vremePreraspodele = new List<DateTime>();
            PopuniInformacijeZaBrisanjePreraspodela(IdIzKojeProstorije, IdUKojuProstoriju, vremePreraspodele);
            IzbrisiPotrebnePreraspodele(IdIzKojeProstorije, IdUKojuProstoriju, vremePreraspodele);
        }

        public bool ProveriValidnostPretrage(String naziv, String kolicina, int index)
        {
            bool validno = true;
            int idGreske = 0;
            if (Validiraj(new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč\s]*$"), naziv) == false || naziv == "")
            { //12
                validno = false;
                idGreske = 12;
            }

            if (Validiraj(new Regex(@"^[0-9]*$"), kolicina) == false || kolicina == "")
            { //13
                validno = false;
                idGreske = 13;
            }

            if (index == -1)
            { //16
                validno = false;
                idGreske = 16;
            }
            validacija.IspisiGresku(idGreske);
            return validno;
        }

        public List<Prostorija> PretraziProstorijePoOpremi(PretragaInfoDTO info)
        {

            List<Prostorija> pronadjeneProstorije = new List<Prostorija>();
            List<Prostorija> sveProstorije = skladisteZaProstorije.GetAll();
            int kolicinaOpreme = Int32.Parse(info.KolicinaOpreme);
            bool pronadjenaOprema = false;
            foreach (Prostorija p in sveProstorije)
            {
                List<StacionarnaOprema> opremaProstorije = p.Staticka;
                pronadjenaOprema = false;
                ProstorijaInfoDTO prostorijaInfo = new ProstorijaInfoDTO(info, opremaProstorije, p);
                PretraziOpremuPoProstorijama(prostorijaInfo, ref pronadjeneProstorije, ref pronadjenaOprema);
                if (pronadjenaOprema == false && info.IndexComboBox == 1 && kolicinaOpreme > 0)
                {
                    pronadjeneProstorije.Add(p);
                }
                if ((info.IndexComboBox == 2 && kolicinaOpreme == 0) || (info.IndexComboBox == 1 && kolicinaOpreme == 0))
                { //17
                    validacija.IspisiGresku(17);
                    break;
                }
            }
            return pronadjeneProstorije;
        }

        private void PretraziOpremuPoProstorijama(ProstorijaInfoDTO info, ref List<Prostorija> pronadjeneProstorije, ref bool pronadjenaOprema)
        {
            int kolicinaOpreme = Int32.Parse(info.pretragaInfo.KolicinaOpreme);
            foreach (StacionarnaOprema oprema in info.OpremaProstorije)
            {
                if (info.pretragaInfo.IndexComboBox == 0)
                {
                    if (oprema.TipStacionarneOpreme.Equals(info.pretragaInfo.NazivOpreme) && oprema.Kolicina > kolicinaOpreme)
                    {
                        pronadjeneProstorije.Add(info.Prostorija);
                        pronadjenaOprema = true;
                    }
                }
                else if (info.pretragaInfo.IndexComboBox == 1)
                {
                    if (oprema.TipStacionarneOpreme.Equals(info.pretragaInfo.NazivOpreme))
                    {
                        pronadjenaOprema = true;
                        if (oprema.Kolicina < kolicinaOpreme)
                            pronadjeneProstorije.Add(info.Prostorija);
                    }
                }
                else if (info.pretragaInfo.IndexComboBox == 2)
                {
                    if (oprema.TipStacionarneOpreme.Equals(info.pretragaInfo.NazivOpreme) && oprema.Kolicina == kolicinaOpreme)
                    {
                        pronadjeneProstorije.Add(info.Prostorija);
                        pronadjenaOprema = true;
                    }
                }
            }
        }
        public void DodajNaprednoRenoviranje(NaprednoRenoviranje renoviranje)
        {
            List<NaprednoRenoviranje> SvaNapredna = skladisteZaNaprednaRenoviranja.GetAll();
            SvaNapredna.Add(renoviranje);
            skladisteZaNaprednaRenoviranja.SaveAll(SvaNapredna);
        }

        public void ObrisiNaprednoRenoviranje(int index)
        {
            List<NaprednoRenoviranje> SvaRenoviranja = skladisteZaNaprednaRenoviranja.GetAll();
            SvaRenoviranja.RemoveAt(index);
            skladisteZaNaprednaRenoviranja.SaveAll(SvaRenoviranja);
        }

        public List<Renoviranje> GetAllRenoviranja()
        {
            return skladisteZaRenoviranja.GetAll();
        }

        public void Save(Renoviranje renoviranje)
        {
            skladisteZaRenoviranja.Save(renoviranje);
        }

        public void SaveAll(List<Renoviranje> renoviranja)
        {
            skladisteZaRenoviranja.SaveAll(renoviranja);
        }
        public List<NaprednoRenoviranje> GetAllNaprednaRenoviranja()
        {
            return skladisteZaNaprednaRenoviranja.GetAll();
        }

        public void Save(NaprednoRenoviranje renoviranje)
        {
            skladisteZaNaprednaRenoviranja.Save(renoviranje);
        }

        public void SaveAll(List<NaprednoRenoviranje> renoviranja)
        {
            skladisteZaNaprednaRenoviranja.SaveAll(renoviranja);
        }

        public Prostorija GetById(int bolnickoLecenjeIdProstorije)
        {
            return skladisteZaProstorije.GetById(bolnickoLecenjeIdProstorije);
        }
    }
}