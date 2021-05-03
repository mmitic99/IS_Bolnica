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
            skladisteZaProstorije = new SkladisteZaProstorije();
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

        public void RenovirajProstoriju(String BrojProstorije, DateTime PocetakRenoviranja, DateTime KrajRenoviranja)
        {
           /* int id = -1;
            foreach (Prostorija p in UpravnikWindow.GetInstance().ListaProstorija) 
            {
                if (BrojProstorije == p.BrojSobe_)
                    id = p.IdProstorije_;
            } 
            if (DaLiJeSLobodnaProstorijaZaRenoviranje(id, DateTime DatumPocetka, DateTime DatumKraja)) { }
            */
            Renoviranje renoviranje = new Renoviranje(BrojProstorije, PocetakRenoviranja, KrajRenoviranja);
            UpravnikWindow.GetInstance().SvaRenoviranja.Add(renoviranje);
            SkladisteZaRenoviranja.GetInstance().SaveAll(UpravnikWindow.GetInstance().SvaRenoviranja);
            OsveziPrikazRenoviranja();
            OcistiTextPoljaRenoviranja();
        }

        public void ZavrsiRenoviranje(int index)
        {
            int IdProstorije = UpravnikWindow.GetInstance().SvaRenoviranja[index].IdProstorije;
            UpravnikWindow.GetInstance().SvaRenoviranja.RemoveAt(index);
            SkladisteZaRenoviranja.GetInstance().SaveAll(UpravnikWindow.GetInstance().SvaRenoviranja);
            OsveziPrikazRenoviranja();
            DeselektujMoguceRenoviranje(IdProstorije);
        }

        public void AzurirajRenoviranjaProstorija()
        {
            List<int> IdProstorijaZaBrisanjeIzRenoviranja = new List<int>();
            for (int i = 0; i < UpravnikWindow.GetInstance().SvaRenoviranja.Count; i++)
            {
                if (DateTime.Now.Date >= UpravnikWindow.GetInstance().SvaRenoviranja[i].DatumPocetkaRenoviranja.Date && DateTime.Now.Date <= UpravnikWindow.GetInstance().SvaRenoviranja[i].DatumZavrsetkaRenoviranja.Date)
                {
                    for (int j = 0; j < UpravnikWindow.GetInstance().ListaProstorija.Count; j++)
                    {
                        if (UpravnikWindow.GetInstance().ListaProstorija[j].IdProstorije_.Equals(UpravnikWindow.GetInstance().SvaRenoviranja[i].IdProstorije))
                            UpravnikWindow.GetInstance().ListaProstorija[j].RenoviraSe_ = true;
                    }
                }
                else if (DateTime.Now.Date > UpravnikWindow.GetInstance().SvaRenoviranja[i].DatumZavrsetkaRenoviranja)
                {
                    IdProstorijaZaBrisanjeIzRenoviranja.Add(UpravnikWindow.GetInstance().SvaRenoviranja[i].IdProstorije);
                    for (int j = 0; j < UpravnikWindow.GetInstance().ListaProstorija.Count; j++)
                    {
                        if (UpravnikWindow.GetInstance().ListaProstorija[j].IdProstorije_.Equals(UpravnikWindow.GetInstance().SvaRenoviranja[i].IdProstorije))
                        {
                            UpravnikWindow.GetInstance().ListaProstorija[j].RenoviraSe_ = false;
                        }
                    }
                }
            }
            foreach (int ID in IdProstorijaZaBrisanjeIzRenoviranja)
            {
                for (int i = 0; i < UpravnikWindow.GetInstance().SvaRenoviranja.Count; i++)
                {
                    if (UpravnikWindow.GetInstance().SvaRenoviranja[i].IdProstorije == ID)
                        UpravnikWindow.GetInstance().SvaRenoviranja.RemoveAt(i);
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            SkladisteZaRenoviranja.GetInstance().SaveAll(UpravnikWindow.GetInstance().SvaRenoviranja);
            UpravnikWindow.GetInstance().TabelaRenoviranja.ItemsSource = new ObservableCollection<Renoviranje>(UpravnikWindow.GetInstance().SvaRenoviranja);
            UpravnikWindow.GetInstance().TabelaProstorija.ItemsSource = new ObservableCollection<Prostorija>(UpravnikWindow.GetInstance().ListaProstorija);
        }

        public void DeselektujMoguceRenoviranje(int id)
        {
            for (int i = 0; i < UpravnikWindow.GetInstance().ListaProstorija.Count; i++)
                if (UpravnikWindow.GetInstance().ListaProstorija[i].IdProstorije_.Equals(id))
                    UpravnikWindow.GetInstance().ListaProstorija[i].RenoviraSe_ = false;
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
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
            List<Termin> SviTermini = SkladisteZaTermine.getInstance().GetAll();
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
            List<ZakazanaPreraspodelaStatickeOpreme> svePreraspodele = SkladisteZaZakazanuPreraspodeluStatickeOpreme.GetInstance().GetAll();
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
            List<Renoviranje> Renoviranja = SkladisteZaRenoviranja.GetInstance().GetAll();
            foreach (Renoviranje renoviranje in Renoviranja)
            {
                if (renoviranje.IdProstorije == id)
                {
                    return renoviranje;
                }
            }
            return null;
        }

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime datumIVremePreraspodele, double trajanje)
        {
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
            Renoviranje renoviranjeZaProstoriju = GetRenoviranjeByIdProstorije(iDProstorije);
            foreach (Termin t in terminiProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) > 0 && DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina.AddMinutes(t.TrajanjeTermina)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if (DateTime.Compare(datumIVremePreraspodele, t.DatumIVremeTermina) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(trajanje), t.DatumIVremeTermina) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
                if (DateTime.Compare(t.DatumIVremeTermina, datumIVremePreraspodele) == 0)
                {
                    slobodan = false;
                    break;
                }
            }
            foreach (ZakazanaPreraspodelaStatickeOpreme prer in preraspodeleProstorije)
            {
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) > 0 && DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele.AddMinutes(prer.TrajanjePreraspodele)) < 0) //da li pocetak upada) //da li kraj upada
                {
                    slobodan = false;
                    break;
                }
                if (DateTime.Compare(datumIVremePreraspodele, prer.DatumIVremePreraspodele) < 0 && DateTime.Compare(datumIVremePreraspodele.AddMinutes(trajanje), prer.DatumIVremePreraspodele) > 0) //da li je mozda taj vez zakazani termin unutar potencijalnog termina
                {
                    slobodan = false;
                    break;
                }
                if (DateTime.Compare(prer.DatumIVremePreraspodele, datumIVremePreraspodele) == 0)
                {
                    slobodan = false;
                    break;
                }
            }
            /* if (DateTime.Compare(renoviranjeZaProstoriju.DatumPocetkaRenoviranja, datumIVremePreraspodele) <= 0 && DateTime.Compare(renoviranjeZaProstoriju.DatumPocetkaRenoviranja, datumIVremePreraspodele) >= 0)
             {
                 slobodan = false;      
             } */
            return slobodan;
        }

        public bool DaLiJeSLobodnaProstorijaZaRenoviranje(int iDProstorije, DateTime DatumPocetka, DateTime DatumKraja)
        {
            bool slobodan = true;
            List<Termin> terminiProstorije = GetTerminiByIdProstorije(iDProstorije);
            List<ZakazanaPreraspodelaStatickeOpreme> preraspodeleProstorije = GetPreraspodeleByIdProstorije(iDProstorije);
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
            return SkladisteZaProstorije.GetInstance().GetAll();
        }

        public void Save(Prostorija prostorija)
        {
            SkladisteZaProstorije.GetInstance().Save(prostorija);
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void DodajStacionarnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.Add(new StacionarnaOprema(tipOpreme, kolicina));
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
            OcistiTextPoljaStatickeOpremeUMagacinu();
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.RemoveAt(index);
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
        }
        public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.Add(new PotrosnaOprema(tipOpreme, kolicina));
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina();
            OcistiTextPoljaDinamickeOpremeUMagacinu();
        }

        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.RemoveAt(index);
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina();
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka_[index].Kolicina_ + kolicina) >= 0)
                {
                    prostorije[i].Staticka_[index].Kolicina_ += kolicina;
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
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
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna_[index].KolicinaOpreme_ += kolicina) >= 0)
                {
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
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
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    return soba;
            }
            return p;
        }

        public void DodajProstoriju(Prostorija p)
        {
            Prostorija prostorija = p;
            int indexPoslednjeProstorije = UpravnikWindow.GetInstance().ListaProstorija.Count - 1;
            IdProstorijeGenerator = UpravnikWindow.GetInstance().ListaProstorija[indexPoslednjeProstorije].IdProstorije_;
            prostorija.IdProstorije_ = ++IdProstorijeGenerator;
            UpravnikWindow.GetInstance().ListaProstorija.Add(prostorija);
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            int StariIdProstorije = UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_;
            bool RenoviranjeStatus = UpravnikWindow.GetInstance().ListaProstorija[index].RenoviraSe_;
            UpravnikWindow.GetInstance().ListaProstorija[index] = p;
            UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_ = StariIdProstorije;
            UpravnikWindow.GetInstance().ListaProstorija[index].RenoviraSe_ = RenoviranjeStatus;
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzbrisiProstoriju(int index)
        {
            UpravnikWindow.GetInstance().ListaProstorija.RemoveAt(index);
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            OsveziPrikazProstorija();
        }

        public void PrebaciStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
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
                        UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                        UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                        if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                        OsveziPrikazTabelaOpreme();
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
                    UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                    UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                    if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                    OsveziPrikazTabelaOpreme();
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
            for (int i = 0; i<UpravnikWindow.GetInstance().ListaProstorija[indexIzKojeProstorije].Staticka_.Count; i++)
                {
                    if (UpravnikWindow.GetInstance().ListaProstorija[indexIzKojeProstorije].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                        return i;
                }
            return -1;
        }

    public void PrebaciZakazanuStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = GetIndexOpremeKojaSePrebacuje(indexIzKojeProstorije, nazivOpreme);
            for (int i = 0; i < prostorije[indexUKojuProstoriju].Staticka_.Count; i++)
            {
                if (prostorije[indexUKojuProstoriju].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
                    {
                        prostorije[indexUKojuProstoriju].Staticka_[i].Kolicina_ += kolicina;
                        prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                        UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                        UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                        if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                        OsveziPrikazTabelaOpreme();
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
                    UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                    UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                    if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                    OsveziPrikazTabelaOpreme();
                }
                else
                {
                    MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
                }
            }
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            prostorije[indexProstorije].Staticka_.RemoveAt(indexOpreme);
            if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexProstorije].Staticka_;
            OsveziPrikazTabelaStatickeOpremeMagacina();
            OsveziPrikazTabelaOpreme();
        }

        public void IzmeniStacionarnuOpremuProstorije(int indexProstorije, int indexOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            if ((prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ + kolicina) < 0)
                MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ += kolicina;
                if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
                SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexProstorije].Staticka_;
                OsveziPrikazTabelaStatickeOpremeMagacina();
                OsveziPrikazTabelaOpreme();
            }
        }

        public bool ProveriValidnostProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadratura)
        {
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;

            Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
            if (sablon.IsMatch(BrojProstorije))
            {

                foreach (Prostorija Soba in UpravnikWindow.GetInstance().ListaProstorija)
                {
                    if (BrojProstorije.Equals(Soba.BrojSobe_))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        checkBrojProstorije = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9]$");
            if (sablon.IsMatch(Sprat))
                checkSprat = true;
            else
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kvadratura))
                checkKvadratura = true;
            else
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IndexSelektovaneVrsteProstorije != -1)
                checkVrsta = true;
            else
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (checkKvadratura == true && checkSprat == true && checkBrojProstorije == true && checkVrsta == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostIzmeneProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadratura)
        {
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;
            int indexSelektovaneProstorije = UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.SelectedIndex;

            Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
            if (sablon.IsMatch(BrojProstorije))
            {
                foreach (Prostorija Soba in UpravnikWindow.GetInstance().ListaProstorija)
                {
                    if (Soba.BrojSobe_ != UpravnikWindow.GetInstance().ListaProstorija[indexSelektovaneProstorije].BrojSobe_ && BrojProstorije.Equals(Soba.BrojSobe_))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        checkBrojProstorije = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9]$");
            if (sablon.IsMatch(Sprat))
                checkSprat = true;
            else
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kvadratura))
                checkKvadratura = true;
            else
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IndexSelektovaneVrsteProstorije != -1)
                checkVrsta = true;
            else
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (checkKvadratura == true && checkSprat == true && checkBrojProstorije == true && checkVrsta == true)
                return true;
            else
                return false;
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
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            oprema = prostorije[index].Staticka_;
            return oprema;
        }

        public void OsveziPrikazProstorija()
        {
            UpravnikWindow.GetInstance().TabelaProstorija.ItemsSource = new ObservableCollection<Prostorija>(UpravnikWindow.GetInstance().ListaProstorija);
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.ItemsSource = new ObservableCollection<Prostorija>(UpravnikWindow.GetInstance().ListaProstorija);
            UpravnikWindow.GetInstance().TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(UpravnikWindow.GetInstance().ListaProstorija);
            UpravnikWindow.GetInstance().TabelaProstorijeUKojuSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(UpravnikWindow.GetInstance().ListaProstorija);
        }

        private void OsveziPrikazTabelaStatickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaStatickeMagacinIzmeni.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaMagacin);
            UpravnikWindow.GetInstance().TabelaStatickeMagacin.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaMagacin);
        }

        private void OsveziPrikazTabelaDinamickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaDinamickeMagacinIzmeni.ItemsSource = new ObservableCollection<PotrosnaOprema>(UpravnikWindow.GetInstance().PotrosnaMagacin);
            UpravnikWindow.GetInstance().TabelaDinamickeMagacin.ItemsSource = new ObservableCollection<PotrosnaOprema>(UpravnikWindow.GetInstance().PotrosnaMagacin);
        }

        private void OsveziPrikazTabelaOpreme()
        {
            UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima);
            UpravnikWindow.GetInstance().TabelaOpremeUKojuSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje);
        }

        private void OsveziPrikazRenoviranja()
        {
            UpravnikWindow.GetInstance().TabelaRenoviranja.ItemsSource = new ObservableCollection<Renoviranje>(UpravnikWindow.GetInstance().SvaRenoviranja);
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
            prostorije = SkladisteZaProstorije.GetInstance().GetAll();
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
            prostorije = SkladisteZaProstorije.GetInstance().GetAll();
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
            prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija p in prostorije)
            {
                if (p.BrojSobe_ == brojProstorije)
                    return p.Sprat_;
            }
            return -1;
        }

        public async Task AzurirajStanjeOpremeAkoJeBiloPrebacivanja()
        {
            List<ZakazanaPreraspodelaStatickeOpreme> SvePreraspodele = SkladisteZaZakazanuPreraspodeluStatickeOpreme.GetInstance().GetAll();
            int indexUKoju = -1;
            int indexIzKoje = -1;
            foreach (ZakazanaPreraspodelaStatickeOpreme preraspodela in SvePreraspodele) 
            {
                for (int i = 0; i < UpravnikWindow.GetInstance().ListaProstorija.Count; i++)
                {
                    if (preraspodela.IdProstorijeUKojUSePrenosiOprema == UpravnikWindow.GetInstance().ListaProstorija[i].IdProstorije_)
                        indexUKoju = i;
                    else if (preraspodela.IdProstorijeIzKojeSePrenosiOprema == UpravnikWindow.GetInstance().ListaProstorija[i].IdProstorije_)
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
                //OtkaziPreraspodeluStatickeOpreme();
            }
            else
            {
                await Task.Delay(preraspodela.DatumIVremePreraspodele.AddMinutes(60) - DateTime.Now);
                //await Task.Delay(10000);
                PrebaciZakazanuStacionarnuOpremuUProstoriju(indexIzKojeProstorije, indexUKojuProstoriju, preraspodela.NazivOpreme, preraspodela.KolicinaOpreme);
                OsveziPrikazTabelaStatickeOpremeMagacina();
                OsveziPrikazTabelaOpreme();
                //OtkaziPreraspodeluStatickeOpreme();
            }
        }

        public SkladisteZaProstorije skladisteZaProstorije;
        public SkladisteZaTermine skladisteZaTermine;

    }
}