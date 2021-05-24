using Model;
using Repozitorijum;
using Servis;
using System;
using System.Collections.Generic;
using Bolnica.DTOs;
using Bolnica.model;
using Model.Enum;
using Bolnica.view.UpravnikView;
using System.Collections.ObjectModel;

namespace Kontroler
{
    public class ProstorijeKontroler
    {

        private static ProstorijeKontroler instance = null;

        public static ProstorijeKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new ProstorijeKontroler();
            }
            return instance;
        }

        public ProstorijeKontroler()
        {
            prostorijeServis = new ProstorijeServis();
        }

        public void RenovirajProstoriju(RenoviranjeDTO renoviranjeDTO)
        {
            Renoviranje renoviranje = new Renoviranje(renoviranjeDTO.BrojProstorije, renoviranjeDTO.DatumPocetkaRenoviranja, renoviranjeDTO.DatumZavrsetkaRenoviranja);
            ProstorijeServis.GetInstance().RenovirajProstoriju(renoviranje);
        }

        public void ZavrsiRenoviranje(int index)
        {
            ProstorijeServis.GetInstance().ZavrsiRenoviranje(index);
        }

        public void AzurirajRenoviranjaProstorija()
        {
            ProstorijeServis.GetInstance().AzurirajRenoviranjaProstorija();
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

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime pocetakTermina, double trajanje)
        {
            return ProstorijeServis.GetInstance().DaLiJeSLobodnaProstorija(iDProstorije, pocetakTermina, trajanje);
        }

        public List<ProstorijaDTO> GetAll()
        {
            List<ProstorijaDTO> prostorije = new List<ProstorijaDTO>();
            foreach (Prostorija prostorija in prostorijeServis.GetAll())
            {
                prostorije.Add(new ProstorijaDTO()
                {
                    BrojSobe = prostorija.BrojSobe,
                    IdProstorije = prostorija.IdProstorije,
                    Sprat = prostorija.Sprat,
                    VrstaProstorije = prostorija.VrstaProstorije,
                    Kvadratura = prostorija.Kvadratura_,
                    RenoviraSe = prostorija.RenoviraSe_
                });
            }
            return prostorije;
        }

        public List<Prostorija> GetAllProstorije()
        {
            return ProstorijeServis.GetInstance().GetAll();
        }

        public void Save(Prostorija prostorija)
        {
            // TODO: implement
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            // TODO: implement
        }

        public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajPotrosnuOpremuUMagacin(tipOpreme, kolicina);
        }

        public void DodajStacionarnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().DodajStacionarnuOpremuUMagacin(tipOpreme, kolicina);
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiStacionarnuOpremuIzMagacina(index);
        }
        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiPotrosnuOpremuIzMagacina(index);
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniStacionarnuOpremuUMagacinu(index, kolicina);
        }

        public void IzmeniDinamickuOpremuUMagacinu(int index, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniDinamickuOpremuUMagacinu(index, kolicina);
        }

        public Prostorija GetMagacin()
        {
            return ProstorijeServis.GetInstance().GetMagacin();
        }

        public void DodajProstoriju(ProstorijaDTO prostorijaDTO)
        {
            //Prostorija(String BrojProstorije, int Sprat, Model.Enum.VrstaProstorije Vrsta, double Kvadratura)
            Prostorija prostorija = new Prostorija(prostorijaDTO.BrojSobe, prostorijaDTO.Sprat, prostorijaDTO.VrstaProstorije, prostorijaDTO.Kvadratura);
            ProstorijeServis.GetInstance().DodajProstoriju(prostorija);
        }

        public void IzmeniProstoriju(int index, ProstorijaDTO prostorijaDTO)
        {
            //Prostorija(String BrojProstorije, int Sprat, Model.Enum.VrstaProstorije Vrsta, double Kvadratura)
            Prostorija prostorija = new Prostorija(prostorijaDTO.BrojSobe, prostorijaDTO.Sprat, prostorijaDTO.VrstaProstorije, prostorijaDTO.Kvadratura);
            ProstorijeServis.GetInstance().IzmeniProstoriju(index, prostorija);
        }

        public void IzbrisiProstoriju(int index)
        {
            ProstorijeServis.GetInstance().IzbrisiProstoriju(index);
        }

        public bool ProveriValidnostProstorije(ProstorijaValidacijaDTO prostorija)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostProstorije(prostorija);
        }

        public bool ProveriValidnostIzmeneProstorije(ProstorijaValidacijaDTO prostorija)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostIzmeneProstorije(prostorija);
        }

        public bool ProveriValidnostOpreme(String NazivOpreme, String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostOpreme(NazivOpreme, Kolicina);
        }

        public bool ProveriValidnostKolicineOpreme(String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpreme(Kolicina);
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpremePriPrebacivanju(Kolicina);
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            return ProstorijeServis.GetInstance().GetVrstuProstorije(IndexSelektovaneVrsteProstorije);
        }

        public List<StacionarnaOprema> GetStacionarnaOpremaProstorije(int index) 
        {
            return ProstorijeServis.GetInstance().GetStacionarnaOpremaProstorije(index);
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            ProstorijeServis.GetInstance().IzbrisiStacionarnuOpremuIzProstorije(indexProstorije, indexOpreme); 
        }

        public void IzmeniStacionarnuOpremuProstorije(int indexProstorije, int indexOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().IzmeniStacionarnuOpremuProstorije(indexProstorije, indexOpreme, kolicina);
        }

        public bool ProveriValidnostPrebacivanjaOpreme(String kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpreme(kolicina);
        }
        public void PrebaciStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            ProstorijeServis.GetInstance().PrebaciStacionarnuOpremuUProstoriju(indexIzKojeProstorije, indexUKojuProstoriju, nazivOpreme, kolicina);
        }

        public int GetIdProstorijeByBrojProstorije(String brojProstorije)
        {
            return ProstorijeServis.GetInstance().GetIdProstorijeByBrojProstorije(brojProstorije);
        }

        public Model.Enum.VrstaProstorije GetVrstaProstorijeByBrojProstorije(String brojProstorije)
        {
            return ProstorijeServis.GetInstance().GetVrstaProstorijeByBrojProstorije(brojProstorije);
        }

        public int GetSpratProstorijeByBrojProstorije(String brojProstorije)
        {
            return ProstorijeServis.GetInstance().GetSpratProstorijeByBrojProstorije(brojProstorije);
        }

        public void AzurirajStanjeOpremeAkoJeBiloPrebacivanja()
        {
            ProstorijeServis.GetInstance().AzurirajStanjeOpremeAkoJeBiloPrebacivanja();
        }

        public bool ProveriValidnostPretrage(String naziv, String kolicina, int index)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostPretrage(naziv, kolicina, index);
        }

        public void PretraziProstorijePoOpremi(String naziv, String kolicina, int index)
        {
            List<Prostorija> PretrazeneProstorije = ProstorijeServis.GetInstance().PretraziProstorijePoOpremi(naziv, kolicina, index);
            UpravnikWindow.GetInstance().TabelaProstorijaIzKojeSePrebacujeOprema.ItemsSource = new ObservableCollection<Prostorija>(PretrazeneProstorije);
            UpravnikWindow.GetInstance().TabelaProstorijaIzKojeSePrebacujeOprema.IsEnabled = false;
        }

        public Servis.TerminServis terminServis;
        public Servis.ProstorijeServis prostorijeServis;

        public int GetBrojProstorija(VrstaProstorije vrstaProstorije)
        {
            return prostorijeServis.GetByVrstaProstorije(vrstaProstorije).Count;
        }
        public void NamapirajStatickuOpremuMagacina(int index)
        {
            List <StacionarnaOprema> StacionarnaMagacin = GetMagacin().Staticka_;
            UpravnikWindow.GetInstance().NazivDinamickeOpremeIzmeni.Text = StacionarnaMagacin[index].TipStacionarneOpreme_;
            UpravnikWindow.GetInstance().KolicinaDinamickeOpremeIzmeni.Text = StacionarnaMagacin[index].Kolicina_.ToString();
        }

        public void NamapirajDinamickuOpremuMagacina(int index)
        {
            List<PotrosnaOprema> PotrosnaMagacin = GetMagacin().Potrosna_;
            UpravnikWindow.GetInstance().NazivStatickeOpremeIzmeni.Text = PotrosnaMagacin[index].TipOpreme_;
            UpravnikWindow.GetInstance().KolicinaStatickeOpremeIzmeni.Text = PotrosnaMagacin[index].KolicinaOpreme_.ToString();
        }

        public void NamapirajOpremuZaPrebacivanjeIzProstorije(int indexOpreme, int indexProstorije)
        {
            List<StacionarnaOprema> StacionarnaOpremaOdKojeSeUzima = ProstorijeServis.GetInstance().GetAll()[indexProstorije].Staticka_;
            UpravnikWindow.GetInstance().NazivOpremeSKojomSeRadi_Copy.Text = StacionarnaOpremaOdKojeSeUzima[indexOpreme].TipStacionarneOpreme_;
        }

        public void NamapirajOpremuZaPrebacivanjeUProstoriju(int indexOpreme, int indexProstorije)
        {
            List<StacionarnaOprema> StacionarnaOpremaUKojuSeDodaje = ProstorijeServis.GetInstance().GetAll()[indexProstorije].Staticka_;
            UpravnikWindow.GetInstance().NazivOpremeSKojomSeRadi.Text = StacionarnaOpremaUKojuSeDodaje[indexOpreme].TipStacionarneOpreme_;
        }

        public void NamapirajProstoriju(int index)
        {
            List<Prostorija> ListaProstorija = ProstorijeServis.GetInstance().GetAll();
            UpravnikWindow.GetInstance().BrojProstorijeTextBoxIzmeni.Text = ListaProstorija[index].BrojSobe_;
            UpravnikWindow.GetInstance().SpratTextBoxIzmeni.Text = ListaProstorija[index].Sprat_.ToString();
            UpravnikWindow.GetInstance().KvadraturaTextBoxIzmeni.Text = ListaProstorija[index].Kvadratura_.ToString();

            if (ListaProstorija[index].VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_preglede)
            {
                UpravnikWindow.GetInstance().VrstaProstorijeComboBoxIzmeni.Text = "Soba za preglede";
            }
            else if (ListaProstorija[index].VrstaProstorije_ == Model.Enum.VrstaProstorije.Operaciona_sala)
            {
                UpravnikWindow.GetInstance().VrstaProstorijeComboBoxIzmeni.Text = "Operaciona sala";
            }
            else if (ListaProstorija[index].VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
            {
                UpravnikWindow.GetInstance().VrstaProstorijeComboBoxIzmeni.Text = "Soba za bolesnike";
            }
            else if (ListaProstorija[index].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
            {
                UpravnikWindow.GetInstance().VrstaProstorijeComboBoxIzmeni.Text = "Magacin";
            }
        }
    }
}