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
                if (renoviranje.BrojProstorije == p.BrojSobe_)
                    id = p.IdProstorije_;
            }
            if (DaLiJeSLobodnaProstorijaZaRenoviranje(id, renoviranje.DatumPocetkaRenoviranja, renoviranje.DatumZavrsetkaRenoviranja))
            {
                SvaRenoviranja.Add(renoviranje);
                SkladisteZaRenoviranjaXml.GetInstance().SaveAll(SvaRenoviranja);
                OsveziPrikazRenoviranja();
                OcistiTextPoljaRenoviranja();
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
            OsveziPrikazRenoviranja();
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
                        if (SveProstorije[j].IdProstorije_.Equals(SvaRenoviranja[i].IdProstorije))
                            SveProstorije[j].RenoviraSe_ = true;
                    }
                }
                else if (DateTime.Now.Date > SvaRenoviranja[i].DatumZavrsetkaRenoviranja)
                {
                    IdProstorijaZaBrisanjeIzRenoviranja.Add(SvaRenoviranja[i].IdProstorije);
                    for (int j = 0; j < SveProstorije.Count; j++)
                    {
                        if (SveProstorije[j].IdProstorije_.Equals(SvaRenoviranja[i].IdProstorije))
                        {
                            SveProstorije[j].RenoviraSe_ = false;
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
            UpravnikWindow.GetInstance().TabelaRenoviranja.ItemsSource = new ObservableCollection<Renoviranje>(SvaRenoviranja);
            UpravnikWindow.GetInstance().TabelaProstorija.ItemsSource = new ObservableCollection<Prostorija>(SveProstorije);
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.ItemsSource = new ObservableCollection<Prostorija>(SveProstorije);
        }

        public void DeselektujMoguceRenoviranje(int id)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            for (int i = 0; i < SveProstorije.Count; i++)
                if (SveProstorije[i].IdProstorije_.Equals(id))
                    SveProstorije[i].RenoviraSe_ = false;
            skladisteZaProstorije.SaveAll(SveProstorije);
            OsveziPrikazProstorija();

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

        public bool ProveriTermine(bool slobodan, DateTime datumIVremePreraspodele, List<Termin> terminiProstorije, double trajanje)
        {
            foreach (Termin t in terminiProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) > 0 && DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    return false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(trajanje), t.DatumIVremeTermina) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
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

        public bool ProveriPreraspodeleOpreme(DateTime datumIVremePreraspodele, List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije, double trajanje)
        {
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in preraspodeleProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) > 0 && DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele.AddMinutes(prer.TrajanjePreraspodele)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    return false;
                }
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(trajanje), prer.DatumIVremePreraspodele) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
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
            slobodan = ProveriTermine(slobodan, datumIVremePreraspodele, terminiProstorije, trajanje);
            if (slobodan == false)
                return false;
            slobodan = ProveriPreraspodeleOpreme(datumIVremePreraspodele, preraspodeleProstorije, trajanje);
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
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.Add(new StacionarnaOprema(tipOpreme, kolicina));
                    StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
            OcistiTextPoljaStatickeOpremeUMagacinu();
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.RemoveAt(index);
                    StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
        }
        public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna_;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.Add(new PotrosnaOprema(tipOpreme, kolicina));
                    PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina();
            OcistiTextPoljaDinamickeOpremeUMagacinu();
        }

        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna_;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.RemoveAt(index);
                    PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            skladisteZaProstorije.SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina();
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka_[index].Kolicina_ + kolicina) >= 0)
                {
                    prostorije[i].Staticka_[index].Kolicina_ += kolicina;
                    StacionarnaMagacin = prostorije[i].Staticka_;
                    skladisteZaProstorije.SaveAll(prostorije);
                    OsveziPrikazTabelaStatickeOpremeMagacina();
                    OcistiTextPoljaStatickeOpremeUMagacinu();
                    break;
                }
                else if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka_[index].Kolicina_ + kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void IzmeniDinamickuOpremuUMagacinu(int index, int kolicina)
        {
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna_;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna_[index].KolicinaOpreme_ += kolicina) >= 0)
                {
                    PotrosnaMagacin = prostorije[i].Potrosna_;
                    skladisteZaProstorije.SaveAll(prostorije);
                    OsveziPrikazTabelaDinamickeOpremeMagacina();
                    OcistiTextPoljaDinamickeOpremeUMagacinu();
                    break;
                }
                else if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna_[index].KolicinaOpreme_ += kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više potrošne opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Prostorija GetMagacin()
        {
            Prostorija p = new Prostorija();
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    return soba;
            }
            return p;
        }

        public void DodajProstoriju(Prostorija p)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            Prostorija prostorija = p;
            int indexPoslednjeProstorije = SveProstorije.Count - 1;
            IdProstorijeGenerator = SveProstorije[indexPoslednjeProstorije].IdProstorije_;
            prostorija.IdProstorije_ = ++IdProstorijeGenerator;
            SveProstorije.Add(prostorija);
            skladisteZaProstorije.SaveAll(SveProstorije);
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            int StariIdProstorije = SveProstorije[index].IdProstorije_;
            bool RenoviranjeStatus = SveProstorije[index].RenoviraSe_;
            SveProstorije[index] = p;
            SveProstorije[index].IdProstorije_ = StariIdProstorije;
            SveProstorije[index].RenoviraSe_ = RenoviranjeStatus;
            skladisteZaProstorije.SaveAll(SveProstorije);
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzbrisiProstoriju(int index)
        {
            List<Prostorija> SveProstorije = SkladisteZaProstorijeXml.GetInstance().GetAll();
            SveProstorije.RemoveAt(index);
            skladisteZaProstorije.SaveAll(SveProstorije);
            OsveziPrikazProstorija();
        }

        public void PrebaciStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.SelectedIndex;
            for (int i = 0; i < prostorije[indexUKojuProstoriju].Staticka_.Count; i++)
            {
                if (prostorije[indexUKojuProstoriju].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
                    {
                        prostorije[indexUKojuProstoriju].Staticka_[i].Kolicina_ += kolicina;
                        prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                        if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        skladisteZaProstorije.SaveAll(prostorije);
                        OsveziPrikazTabelaOpreme(indexIzKojeProstorije, indexUKojuProstoriju);
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
                if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
                {
                    prostorije[indexUKojuProstoriju].Staticka_.Add(new StacionarnaOprema(nazivOpreme, kolicina));
                    prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                    if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    skladisteZaProstorije.SaveAll(prostorije);
                    OsveziPrikazTabelaOpreme(indexIzKojeProstorije, indexUKojuProstoriju);
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
            for (int i = 0; i < SveProstorije[indexIzKojeProstorije].Staticka_.Count; i++)
            {
                if (SveProstorije[indexIzKojeProstorije].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                    return i;
            }
            return -1;
        }
        public void IzvrsiPrebacivanjeOpreme(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina, int indexOpreme, List<Prostorija> prostorije, int i)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
            {
                if (i == -1)
                    prostorije[indexUKojuProstoriju].Staticka_.Add(new StacionarnaOprema(nazivOpreme, kolicina));
                else
                    prostorije[indexUKojuProstoriju].Staticka_[i].Kolicina_ += kolicina;
                prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                    OsveziPrikazTabelaStatickeOpremeMagacina();
                }
                if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                    OsveziPrikazTabelaStatickeOpremeMagacina();
                }
                skladisteZaProstorije.SaveAll(prostorije);
                OsveziPrikazTabelaOpreme(indexIzKojeProstorije, indexUKojuProstoriju);
            }
            else
            {
                MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
            }
        }


        public void PrebaciZakazanuStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = GetIndexOpremeKojaSePrebacuje(indexIzKojeProstorije, nazivOpreme);
            for (int i = 0; i < prostorije[indexUKojuProstoriju].Staticka_.Count; i++)
            {
                if (prostorije[indexUKojuProstoriju].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    IzvrsiPrebacivanjeOpreme(indexIzKojeProstorije, indexUKojuProstoriju, nazivOpreme, kolicina, indexOpreme, prostorije, i);
                }
            }
            if (nazivOpremeVecPrisutan == false)
            {
                IzvrsiPrebacivanjeOpreme(indexIzKojeProstorije, indexUKojuProstoriju, nazivOpreme, kolicina, indexOpreme, prostorije, -1);
            }
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            prostorije[indexProstorije].Staticka_.RemoveAt(indexOpreme);
            if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
            skladisteZaProstorije.SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
            OsveziPrikazTabelaOpreme(-1, indexProstorije);
        }

        public void IzmeniStacionarnuOpremuProstorije(int indexProstorije, int indexOpreme, int kolicina)
        {
            List<StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            List<Prostorija> prostorije = skladisteZaProstorije.GetAll();
            if ((prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ + kolicina) < 0)
                MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ += kolicina;
                if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
                skladisteZaProstorije.SaveAll(prostorije);
                OsveziPrikazTabelaStatickeOpremeMagacina();
                OsveziPrikazTabelaOpreme(-1, indexProstorije);
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
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;

            checkBrojProstorije = ValidirajBrojProstorije(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe);
            if (checkBrojProstorije == false)
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            checkSprat = Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat);
            if (checkSprat == false)
            { 
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            checkKvadratura = Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura);
            if (checkKvadratura == false)
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (prostorija.VrstaProstorije != -1)
                checkVrsta = true;
            else
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
                    if (unos.Equals(Soba.BrojSobe_))
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
                    if (Soba.BrojSobe_ != SveProstorije[indexSelektovaneProstorije].BrojSobe_ && unos.Equals(Soba.BrojSobe_))
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
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;

            checkBrojProstorije = ValidirajBrojProstorijeIzmena(new Regex(@"^[0-9a-zA-Z]+$"), prostorija.BrojSobe);
            if (checkBrojProstorije == false)
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            checkSprat = Validiraj(new Regex(@"^[0-9]$"), prostorija.Sprat);
            if (checkSprat == false)
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            checkKvadratura = Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), prostorija.Kvadratura);
            if (checkKvadratura == false)
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (prostorija.VrstaProstorije != -1)
                checkVrsta = true;
            else
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
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[+-]?[0-9]*$");
            if (sablon.IsMatch(Kolicina) && !Kolicina.Equals(""))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta izmena količine opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkKolicina == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[+]?[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kolicina) && !Kolicina.Equals(""))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta količine opreme koja se prebacuje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkKolicina == true)
                return true;
            else
                return false;
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
            oprema = prostorije[index].Staticka_;
            return oprema;
        }

        public void OsveziPrikazProstorija()
        {
            UpravnikWindow.GetInstance().TabelaProstorija.ItemsSource = new ObservableCollection<Prostorija>(SkladisteZaProstorijeXml.GetInstance().GetAll());
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.ItemsSource = new ObservableCollection<Prostorija>(SkladisteZaProstorijeXml.GetInstance().GetAll());
            UpravnikWindow.GetInstance().TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(SkladisteZaProstorijeXml.GetInstance().GetAll());
            UpravnikWindow.GetInstance().TabelaProstorijeUKojuSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(SkladisteZaProstorijeXml.GetInstance().GetAll());
        }

        private void OsveziPrikazTabelaStatickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaStatickeMagacinIzmeni.ItemsSource = new ObservableCollection<StacionarnaOprema>(GetMagacin().Staticka_);
            UpravnikWindow.GetInstance().TabelaStatickeMagacin.ItemsSource = new ObservableCollection<StacionarnaOprema>(GetMagacin().Staticka_);
        }

        private void OsveziPrikazTabelaDinamickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaDinamickeMagacinIzmeni.ItemsSource = new ObservableCollection<PotrosnaOprema>(GetMagacin().Potrosna_);
            UpravnikWindow.GetInstance().TabelaDinamickeMagacin.ItemsSource = new ObservableCollection<PotrosnaOprema>(GetMagacin().Potrosna_);
        }

        private void OsveziPrikazTabelaOpreme(int indexIzKojeProstorije, int indexUKojuProstoriju)
        {
            if (indexIzKojeProstorije != -1)
                UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(SkladisteZaProstorijeXml.GetInstance().GetAll()[indexIzKojeProstorije].Staticka_);
            UpravnikWindow.GetInstance().TabelaOpremeUKojuSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(SkladisteZaProstorijeXml.GetInstance().GetAll()[indexUKojuProstoriju].Staticka_);
        }

        private void OsveziPrikazRenoviranja()
        {
            UpravnikWindow.GetInstance().TabelaRenoviranja.ItemsSource = new ObservableCollection<Renoviranje>(SkladisteZaRenoviranjaXml.GetInstance().GetAll());
        }

        private void OcistiTextPoljaProstorije()
        {
            UpravnikWindow.GetInstance().BrojProstorijeTextBox.Clear();
            UpravnikWindow.GetInstance().SpratTextBox.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBox.Clear();
            UpravnikWindow.GetInstance().BrojProstorijeTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().SpratTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBoxIzmeni.Clear();
        }

        private void OcistiTextPoljaRenoviranja()
        {
            UpravnikWindow.GetInstance().BrojProstorijeRenoviranje.Text = "";
            UpravnikWindow.GetInstance().DatumZavrsetkaRenoviranja.Text = "";
            UpravnikWindow.GetInstance().DatumPocetkaRenoviranja.Text = "";

        }

        private void OcistiTextPoljaStatickeOpremeUMagacinu()
        {
            UpravnikWindow.GetInstance().KolicinaStatickeOpreme.Clear();
            UpravnikWindow.GetInstance().KolicinaStatickeOpremeIzmeni.Clear();
            UpravnikWindow.GetInstance().NazivStatickeOpreme.Clear();
            UpravnikWindow.GetInstance().NazivStatickeOpremeIzmeni.Clear();
        }

        private void OcistiTextPoljaDinamickeOpremeUMagacinu()
        {
            UpravnikWindow.GetInstance().KolicinaDinamickeOpreme.Clear();
            UpravnikWindow.GetInstance().KolicinaDinamickeOpremeIzmeni.Clear();
            UpravnikWindow.GetInstance().NazivDinamickeOpreme.Clear();
            UpravnikWindow.GetInstance().NazivDinamickeOpremeIzmeni.Clear();
        }

        public int GetIdProstorijeByBrojProstorije(String brojProstorije)
        {
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe_ == brojProstorije)
                    return p.IdProstorije_;
            }
            return -1;
        }

        public Model.Enum.VrstaProstorije GetVrstaProstorijeByBrojProstorije(String brojProstorije)
        {
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe_ == brojProstorije)
                    return p.VrstaProstorije_;
            }
            return Model.Enum.VrstaProstorije.Soba_za_preglede;
        }

        public int GetSpratProstorijeByBrojProstorije(String brojProstorije)
        {
            List<Prostorija> prostorije = new List<Prostorija>();
            prostorije = skladisteZaProstorije.GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe_ == brojProstorije)
                    return p.Sprat_;
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
                    if (preraspodela.IdProstorijeUKojUSePrenosiOprema == SveProstorije[i].IdProstorije_)
                        indexUKoju = i;
                    else if (preraspodela.IdProstorijeIzKojeSePrenosiOprema == SveProstorije[i].IdProstorije_)
                        indexIzKoje = i;
                }
                await PrebaciZakazanuOpremu(indexIzKoje, indexUKoju, preraspodela);
            }
        }

        public async Task PrebaciZakazanuOpremu(int indexIzKojeProstorije, int indexUKojuProstoriju, ZakazanaPreraspodelaStatickeOpreme preraspodela)
        {
            if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.AddMinutes(60), DateTime.Now) <= 0)
            {
                PrebaciZakazanuStacionarnuOpremuUProstoriju(indexIzKojeProstorije, indexUKojuProstoriju, preraspodela.NazivOpreme, preraspodela.KolicinaOpreme);
                AzurirajPreraspodeleOpreme();
            }
            else
            {
                await Task.Delay(preraspodela.DatumIVremePreraspodele.AddMinutes(60) - DateTime.Now);
                //await Task.Delay(10000);
                PrebaciZakazanuStacionarnuOpremuUProstoriju(indexIzKojeProstorije, indexUKojuProstoriju, preraspodela.NazivOpreme, preraspodela.KolicinaOpreme);
                OsveziPrikazTabelaStatickeOpremeMagacina();
                OsveziPrikazTabelaOpreme(indexIzKojeProstorije, indexUKojuProstoriju);
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
            UpravnikWindow.GetInstance().TabelaZakazanihPrebacivanjaOpreme.ItemsSource = new ObservableCollection<ZakazanaPreraspodelaStatickeOpreme>(SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll());
        }

        public bool ProveriValidnostPretrage(String naziv, String kolicina, int index)
        {
            bool checkNaziv = false;
            bool checkKolicina = false;
            bool checkUpit = false;

            Regex sablon = new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč]*$");
            if (sablon.IsMatch(naziv) && naziv != "")
            {
                checkNaziv = true;
            }
            else
            {
                MessageBox.Show("Neispravno unet naziv opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9]*$");
            if (sablon.IsMatch(kolicina) && kolicina != "")
                checkKolicina = true;
            else
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
                checkUpit = true;
            if (checkNaziv == true && checkKolicina == true && checkUpit == true)
                return true;
            else
                return false;
        }

        public List<Prostorija> PretraziProstorijePoOpremi(String naziv, String kolicina, int index)
        {
            List<Prostorija> pronadjeneProstorije = new List<Prostorija>();
            List<Prostorija> sveProstorije = skladisteZaProstorije.GetAll();
            int kolicinaOpreme = Int32.Parse(kolicina);
            bool pronadjenaOprema = false;
            foreach (Prostorija p in sveProstorije)
            {
                List<StacionarnaOprema> opremaProstorije = p.Staticka_;
                pronadjenaOprema = false;
                foreach (StacionarnaOprema oprema in opremaProstorije)
                {
                    if (index == 0)
                    {
                        if (oprema.TipStacionarneOpreme_.Equals(naziv) && oprema.Kolicina_ > kolicinaOpreme)
                        {
                            pronadjeneProstorije.Add(p);
                            pronadjenaOprema = true;
                        }
                    }
                    else if (index == 1)
                    {
                        if (oprema.TipStacionarneOpreme_.Equals(naziv))
                        {
                            pronadjenaOprema = true;
                            if (oprema.Kolicina_ < kolicinaOpreme)
                                pronadjeneProstorije.Add(p);
                        }
                    }
                    else if (index == 2)
                    {
                        if (oprema.TipStacionarneOpreme_.Equals(naziv) && oprema.Kolicina_ == kolicinaOpreme)
                        {
                            pronadjeneProstorije.Add(p);
                            pronadjenaOprema = true;
                        }
                    }
                }
                if (pronadjenaOprema == false && index == 1 && kolicinaOpreme > 0)
                {
                    pronadjeneProstorije.Add(p);
                }
                if ((index == 2 && kolicinaOpreme == 0) || (index == 1 && kolicinaOpreme == 0))
                {
                    MessageBox.Show("Nevalidna pretraga !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
            return pronadjeneProstorije;
        }

        public ISkladisteZaProstorije skladisteZaProstorije;
        public ISkladisteZaTermine skladisteZaTermine;

    }
}