using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.view.UpravnikView;
using Model;
using Model.Enum;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Servis
{
    public class ProstorijeServis
    {

        private static ProstorijeServis instance = null;
        public int IdProstorijeGenerator = 0;
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
            if (vrstaPregleda == VrstaPregleda.Operacija)
                return VrstaProstorije.Operaciona_sala;
            else return VrstaProstorije.Soba_za_preglede;
        }

        internal int GetPrvaPogodna(Termin termin)
        {
            List<Prostorija> pogodneProsotije = GetByVrstaProstorije(GetPogodnaVrstaProstorije(termin.VrstaTermina));
            foreach (Prostorija p in pogodneProsotije)
            {
                if (DaLiJeSLobodnaProstorija(p.IdProstorije, termin.DatumIVremeTermina, termin.TrajanjeTermina))
                {
                    return p.IdProstorije;
                }
            }
            return -1;
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
            List<Renoviranje> SvaRenoviranja = SkladisteZaRenoviranjaXml.GetInstance().GetAll();
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            int id = -1;
            foreach (Prostorija p in SveProstorije)
            {
                if (renoviranje.BrojProstorije == p.BrojSobe)
                    id = p.IdProstorije;
            }
            if (DaLiJeSLobodnaProstorijaZaRenoviranje(id, renoviranje.DatumPocetkaRenoviranja, renoviranje.DatumZavrsetkaRenoviranja))
            {
                SvaRenoviranja.Add(renoviranje);
                SkladisteZaRenoviranjaXml.GetInstance().SaveAll(SvaRenoviranja);
            }
            else
                MessageBox.Show("Prostorija ima zakazan termin ili preraspodelu opreme u tom rasponu datuma !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ZavrsiRenoviranje(int index)
        {
            List<Renoviranje> SvaRenoviranja = SkladisteZaRenoviranjaXml.GetInstance().GetAll();
            int IdProstorije = SvaRenoviranja[index].IdProstorije;
            SvaRenoviranja.RemoveAt(index);
            SkladisteZaRenoviranjaXml.GetInstance().SaveAll(SvaRenoviranja);
            DeselektujMoguceRenoviranje(IdProstorije);
        }

        public void AzurirajRenoviranjaProstorija()
        {
            List<Renoviranje> SvaRenoviranja = SkladisteZaRenoviranjaXml.GetInstance().GetAll();
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
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

            foreach (int ID in IdProstorijaZaBrisanjeIzRenoviranja)
            {
                for (int i = 0; i < SvaRenoviranja.Count; i++)
                {
                    if (SvaRenoviranja[i].IdProstorije == ID)
                        SvaRenoviranja.RemoveAt(i);
                }
            }
            skladisteZaProstorije.SaveAll(SveProstorije);
            SkladisteZaRenoviranjaXml.GetInstance().SaveAll(SvaRenoviranja);
        }

        public void DeselektujMoguceRenoviranje(int id)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].IdProstorije.Equals(id))
                    SveProstorije[i].RenoviraSe = false;
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void IzmeniKolicinuLeka(int idLeka, double kolicina, int izProstorije, int uProstoriju)
        {
            // TODO: implement
        }

        public int DobaviProstoriju(DateTime pocetakTermina, DateTime krajTermina, Model.Enum.VrstaProstorije vrstaProstorije)
        {
            // TODO: implement
            return 0;
        }

        public List<Termin> PrikaziTermine(int idProstorije)
        {
            // TODO: implement
            return null;
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
            List<ZakazanaPreraspodelaStatickeOpreme> svePreraspodele = SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll();
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
            List<Renoviranje> Renoviranja = SkladisteZaRenoviranjaXml.GetInstance().GetAll();
            foreach (Renoviranje renoviranje in Renoviranja)
            {
                if (renoviranje.IdProstorije == id)
                {
                    return renoviranje;
                }
            }
            return null;
        }

        public bool ProveriTermine(DateTime datumIVremePreraspodele, List<Termin> terminiProstorije)
        {
            foreach (Termin t in terminiProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) > 0 && DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    return false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(60), t.DatumIVremeTermina) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    return false;
                }
                if (DateTime.Compare(t.DatumIVremeTermina, datumIVremePreraspodele) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ProveriPreraspodeleOpreme(DateTime datumIVremePreraspodele, List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije)
        {
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in preraspodeleProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) > 0 && DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele.AddMinutes(prer.TrajanjePreraspodele)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    return false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(60), prer.DatumIVremePreraspodele) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    return false;
                }
                if (DateTime.Compare(prer.DatumIVremePreraspodele, datumIVremePreraspodele) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ProveriRenoviranje(Renoviranje renoviranjeZaProstoriju, DateTime datumIVremePreraspodele)
        {
            if (renoviranjeZaProstoriju != null)
            {
                DateTime pomerenZbogPonoci = renoviranjeZaProstoriju.DatumZavrsetkaRenoviranja.AddHours(23);
                if (DateTime.Compare(renoviranjeZaProstoriju.DatumPocetkaRenoviranja, datumIVremePreraspodele) <= 0 && DateTime.Compare(pomerenZbogPonoci, datumIVremePreraspodele) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime datumIVremePreraspodele, double trajanje)
        {
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
            Renoviranje renoviranjeZaProstoriju = GetRenoviranjeByIdProstorije(iDProstorije);
            slobodan = ProveriTermine(datumIVremePreraspodele, terminiProstorije);
            if (slobodan == false)
                return false;
            slobodan = ProveriPreraspodeleOpreme(datumIVremePreraspodele, preraspodeleProstorije);
            if (slobodan == false)
                return false;
            slobodan = ProveriRenoviranje(renoviranjeZaProstoriju, datumIVremePreraspodele);
            if (slobodan == false)
                return false;
            return true;
        }

        public bool DaLiJeSLobodnaProstorijaZaRenoviranje(int iDProstorije, DateTime DatumPocetka, DateTime DatumKraja)
        {
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
            Renoviranje renoviranjeZaProstoriju = GetRenoviranjeByIdProstorije(iDProstorije);
            if (renoviranjeZaProstoriju != null)
            {
                MessageBox.Show("Već je zakazano renoviranje za datu prostoriju !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
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
                }
                else if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka[index].Kolicina + kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
                else if (prostorije[i].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna[index].KolicinaOpreme += kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više potrošne opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Prostorija GetMagacin()
        {
            Prostorija p = new Prostorija();
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                    return soba;
            }
            return p;
        }

        public void DodajProstoriju(Prostorija p)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            Prostorija prostorija = p;
            int indexPoslednjeProstorije = SveProstorije.Count - 1;
            IdProstorijeGenerator = SveProstorije[indexPoslednjeProstorije].IdProstorije;
            prostorija.IdProstorije = ++IdProstorijeGenerator;
            SveProstorije.Add(prostorija);
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            int StariIdProstorije = SveProstorije[index].IdProstorije;
            bool RenoviranjeStatus = SveProstorije[index].RenoviraSe;
            SveProstorije[index] = p;
            SveProstorije[index].IdProstorije = StariIdProstorije;
            SveProstorije[index].RenoviraSe = RenoviranjeStatus;
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void IzbrisiProstoriju(int index)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            SveProstorije.RemoveAt(index);
            skladisteZaProstorije.SaveAll(SveProstorije);
        }

        public void PrebaciStacionarnuOpremuUProstoriju(PrebacivanjeOpremeInfoDTO prebacivanjeInfo)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka;
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.SelectedIndex;
            for (int i = 0; i < prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Count; i++)
            {
                if (prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka[i].TipStacionarneOpreme.Equals(prebacivanjeInfo.NazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    if (prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina - prebacivanjeInfo.KolicinaOpreme >= 0)
                    {
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
                    {
                        MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
                    }
                }
            }
            if (nazivOpremeVecPrisutan == false)
            {
                if (prostorije[prebacivanjeInfo.IndexIzKojeProstorije].Staticka[indexOpreme].Kolicina - prebacivanjeInfo.KolicinaOpreme >= 0)
                {
                    prostorije[prebacivanjeInfo.IndexUKojuProstoriju].Staticka.Add(new StacionarnaOprema(prebacivanjeInfo.NazivOpreme, prebacivanjeInfo.KolicinaOpreme));
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
                {
                    MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
                }
            }
        }
        public int GetIndexOpremeKojaSePrebacuje(int indexIzKojeProstorije, string nazivOpreme)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            for (int i = 0; i < SveProstorije[indexIzKojeProstorije].Staticka.Count; i++)
            {
                if (SveProstorije[indexIzKojeProstorije].Staticka[i].TipStacionarneOpreme.Equals(nazivOpreme))
                    return i;
            }
            return -1;
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
            {
                MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                prostorije[izmenaOpremeInfo.IndexProstorije].Staticka[izmenaOpremeInfo.IndexOpreme].Kolicina += izmenaOpremeInfo.KolicinaOpreme;
                if (prostorije[izmenaOpremeInfo.IndexProstorije].VrstaProstorije == Model.Enum.VrstaProstorije.Magacin)
                    StacionarnaMagacin = prostorije[izmenaOpremeInfo.IndexProstorije].Staticka;
                skladisteZaProstorije.SaveAll(prostorije);
            }
        }

        private bool Validiraj(Regex sablon, String unos)
        {
            if (sablon.IsMatch(unos))
                return true;
            else
                return false;
        }

        public bool ProveriValidnostProstorije(ProstorijaValidacijaDTO prostorija)
        {
            if (ValidirajBrojProstorije(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe) == false)
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat) == false)
            { 
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura) == false)
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (prostorija.VrstaProstorije == -1)
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool ValidirajBrojProstorije(Regex sablon, String unos)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            if (sablon.IsMatch(unos))
            {
                foreach (Prostorija Soba in SveProstorije)
                {
                    if (unos.Equals(Soba.BrojSobe))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ValidirajBrojProstorijeIzmena(Regex sablon, String unos)
        {
            int indexSelektovaneProstorije = UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.SelectedIndex;
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            if (sablon.IsMatch(unos))
            {
                foreach (Prostorija Soba in SveProstorije)
                {
                    if (Soba.BrojSobe != SveProstorije[indexSelektovaneProstorije].BrojSobe && unos.Equals(Soba.BrojSobe))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ProveriValidnostIzmeneProstorije(ProstorijaValidacijaDTO prostorija)
        {
            if (ValidirajBrojProstorijeIzmena(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe) == false)
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat) == false)
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura) == false)
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (prostorija.VrstaProstorije == -1)
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool ProveriValidnostOpreme(String NazivOpreme, String Kolicina)
        {
            bool checkNaziv = false;
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč]*$");
            if (sablon.IsMatch(NazivOpreme))
            {
                checkNaziv = true;
            }
            else
            {
                MessageBox.Show("Neispravno unet naziv opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kolicina))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta količina opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkNaziv == true && checkKolicina == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostKolicineOpreme(String Kolicina)
        {
            Regex sablon = new Regex(@"^[+-]?[0-9]*$");
            if (Validiraj(new Regex(@"^[+-]?[0-9]*$"), Kolicina) == false || Kolicina.Equals(""))
            {
                MessageBox.Show("Neispravno uneta izmena količine opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            if (Validiraj(new Regex(@"^[+]?[1-9]{1}[0-9]*$"), Kolicina) == false || Kolicina.Equals(""))
            {
                MessageBox.Show("Neispravno uneta količine opreme koja se prebacuje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            if (IndexSelektovaneVrsteProstorije == 0)
                return Model.Enum.VrstaProstorije.Soba_za_preglede;
            else if (IndexSelektovaneVrsteProstorije == 1)
                return Model.Enum.VrstaProstorije.Operaciona_sala;
            else if (IndexSelektovaneVrsteProstorije == 2)
                return Model.Enum.VrstaProstorije.Soba_za_bolesnike;
            else
                return Model.Enum.VrstaProstorije.Magacin;
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
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    return p.IdProstorije;
            }
            return -1;
        }

        public Model.Enum.VrstaProstorije GetVrstaProstorijeByBrojProstorije(String brojProstorije)
        {
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    return p.VrstaProstorije;
            }
            return Model.Enum.VrstaProstorije.Soba_za_preglede;
        }

        public int GetSpratProstorijeByBrojProstorije(String brojProstorije)
        {
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe == brojProstorije)
                    return p.Sprat;
            }
            return -1;
        }

        public async Task AzurirajStanjeOpremeAkoJeBiloPrebacivanja()
        {
            List<ZakazanaPreraspodelaStatickeOpreme> SvePreraspodele = SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll();
            int indexUKoju = -1;
            int indexIzKoje = -1;
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
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
            List <ZakazanaPreraspodelaStatickeOpreme> PreraspodeleStatickeOpreme = SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll();
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
            List<ZakazanaPreraspodelaStatickeOpreme> PreraspodeleStatickeOpreme = SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll();
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
            SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().SaveAll(PreraspodeleStatickeOpreme);
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
            if (Validiraj(new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč\s]*$"), naziv) == false || naziv == "")
            {
                MessageBox.Show("Neispravno unet naziv opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[0-9]*$"), kolicina) == false || kolicina == "")
            {
                MessageBox.Show("Neispravno uneta količina opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (index == -1)
            {
                MessageBox.Show("Selektujte način pretrage iz padajućeg menija !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
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
                {
                    MessageBox.Show("Nevalidna pretraga !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
    public List<Renoviranje> GetAllRenoviranja()
        {
            return SkladisteZaRenoviranjaXml.GetInstance().GetAll();
        }

        public void Save(Renoviranje renoviranje)
        {
            SkladisteZaRenoviranjaXml.GetInstance().Save(renoviranje);
        }

        public void SaveAll(List<Renoviranje> renoviranja)
        {
            SkladisteZaRenoviranjaXml.GetInstance().SaveAll(renoviranja);
        }

        public ISkladisteZaProstorije skladisteZaProstorije;
        public ISkladisteZaTermine skladisteZaTermine;

    }
}