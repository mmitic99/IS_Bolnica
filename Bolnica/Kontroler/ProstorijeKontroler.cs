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
using System.Text.RegularExpressions;

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
            List<ProstorijaDTO> prostorijeDTO = new List<ProstorijaDTO>();
            foreach (Prostorija prostorija in prostorijeServis.GetAll())
            {
                ProstorijaDTO prostorijaDTO = new ProstorijaDTO();
                prostorijaDTO.BrojSobe = prostorija.BrojSobe;
                prostorijaDTO.IdProstorije = prostorija.IdProstorije;
                prostorijaDTO.Sprat = prostorija.Sprat;
                prostorijaDTO.VrstaProstorije = prostorija.VrstaProstorije;
                prostorijaDTO.Kvadratura = prostorija.Kvadratura;
                prostorijaDTO.RenoviraSe = prostorija.RenoviraSe;
                prostorijaDTO.Potrosna = new List<PotrosnaOpremaDTO>();
                prostorijaDTO.Staticka = new List<StacionarnaOpremaDTO>();
                foreach (StacionarnaOprema stacionarna in prostorija.Staticka)
                {
                    StacionarnaOpremaDTO oprema = new StacionarnaOpremaDTO(stacionarna.TipStacionarneOpreme, stacionarna.Kolicina);
                    prostorijaDTO.Staticka.Add(oprema);
                }
                foreach (PotrosnaOprema potrosna in prostorija.Potrosna)
                {
                    PotrosnaOpremaDTO oprema = new PotrosnaOpremaDTO(potrosna.TipOpreme, potrosna.KolicinaOpreme);
                    prostorijaDTO.Potrosna.Add(oprema);
                }
                prostorijeDTO.Add(prostorijaDTO);
            }
            return prostorijeDTO;
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

        public ProstorijaDTO GetMagacin()
        {
            Prostorija magacin = ProstorijeServis.GetInstance().GetMagacin();
            ProstorijaDTO magacinDTO = new ProstorijaDTO()
            {
                BrojSobe = magacin.BrojSobe,
                Sprat = magacin.Sprat,
                VrstaProstorije = magacin.VrstaProstorije,
                Kvadratura = magacin.Kvadratura,
                Staticka = new List<StacionarnaOpremaDTO>(),
                Potrosna = new List<PotrosnaOpremaDTO>()
            };
            foreach (StacionarnaOprema stacionarna in magacin.Staticka)
            {
                StacionarnaOpremaDTO oprema = new StacionarnaOpremaDTO(stacionarna.TipStacionarneOpreme, stacionarna.Kolicina);
                magacinDTO.Staticka.Add(oprema);
            }
            foreach (PotrosnaOprema potrosna in magacin.Potrosna)
            {
                PotrosnaOpremaDTO oprema = new PotrosnaOpremaDTO(potrosna.TipOpreme, potrosna.KolicinaOpreme);
                magacinDTO.Potrosna.Add(oprema);
            }
            return magacinDTO;
        }

        public void DodajProstoriju(ProstorijaDTO prostorijaDTO)
        {
            Prostorija prostorija = new Prostorija(prostorijaDTO.BrojSobe, prostorijaDTO.Sprat, prostorijaDTO.VrstaProstorije, prostorijaDTO.Kvadratura);
            ProstorijeServis.GetInstance().DodajProstoriju(prostorija);
        }

        public void IzmeniProstoriju(int index, ProstorijaDTO prostorijaDTO)
        {
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

        public bool ValidirajBrojProstorije(Regex sablon, String unos)
        {
            return ProstorijeServis.GetInstance().ValidirajBrojProstorije(sablon, unos);
        }

        public bool ProveriValidnostIzmeneProstorije(ProstorijaValidacijaDTO prostorija, int indexProstorije)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostIzmeneProstorije(prostorija, indexProstorije);
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

        public void IzmeniStacionarnuOpremuProstorije(IzmenaOpremeInfoDTO izmenaOpremeInfo)
        {
            ProstorijeServis.GetInstance().IzmeniStacionarnuOpremuProstorije(izmenaOpremeInfo);
        }

        public bool ProveriValidnostPrebacivanjaOpreme(String kolicina)
        {
            return ProstorijeServis.GetInstance().ProveriValidnostKolicineOpreme(kolicina);
        }
        public void PrebaciStacionarnuOpremuUProstoriju(PrebacivanjeOpremeInfoDTO prebacivanjeInfo, int indexOpreme)
        {
            ProstorijeServis.GetInstance().PrebaciStacionarnuOpremuUProstoriju(prebacivanjeInfo, indexOpreme);
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

        public List<ProstorijaDTO> PretraziProstorijePoOpremi(PretragaInfoDTO info)
        {
            List<Prostorija> PretrazeneProstorije = ProstorijeServis.GetInstance().PretraziProstorijePoOpremi(info);
            List<ProstorijaDTO> pretrazeneDTO = new List<ProstorijaDTO>();
            foreach (Prostorija prostorija in PretrazeneProstorije)
            {
                ProstorijaDTO prostorijaDTO = new ProstorijaDTO()
                {
                    BrojSobe = prostorija.BrojSobe,
                    Sprat = prostorija.Sprat,
                    VrstaProstorije = prostorija.VrstaProstorije,
                    Kvadratura = prostorija.Kvadratura,
                    Staticka = new List<StacionarnaOpremaDTO>(),
                    Potrosna = new List<PotrosnaOpremaDTO>()
                };
                foreach (StacionarnaOprema stacionarna in prostorija.Staticka)
                {
                    StacionarnaOpremaDTO oprema = new StacionarnaOpremaDTO(stacionarna.TipStacionarneOpreme, stacionarna.Kolicina);
                    prostorijaDTO.Staticka.Add(oprema);
                }
                foreach (PotrosnaOprema potrosna in prostorija.Potrosna)
                {
                    PotrosnaOpremaDTO oprema = new PotrosnaOpremaDTO(potrosna.TipOpreme, potrosna.KolicinaOpreme);
                    prostorijaDTO.Potrosna.Add(oprema);
                }
                 pretrazeneDTO.Add(prostorijaDTO);
            }
            return pretrazeneDTO;
        }

        public void DodajNaprednoRenoviranje(NaprednoRenoviranjeDTO renoviranjeDTO)
        {
            NaprednoRenoviranje renoviranje = new NaprednoRenoviranje()
            {
                BrojGlavneProstorije = renoviranjeDTO.BrojGlavneProstorije,
                BrojProstorije1 = renoviranjeDTO.BrojProstorije1,
                BrojProstorije2 = renoviranjeDTO.BrojProstorije2,
                DatumPocetkaRenoviranja = renoviranjeDTO.DatumPocetkaRenoviranja,
                DatumZavrsetkaRenoviranja = renoviranjeDTO.DatumZavrsetkaRenoviranja,
                Spajanje = renoviranjeDTO.Spajanje,
                Podela = renoviranjeDTO.Podela
            };
            ProstorijeServis.GetInstance().DodajNaprednoRenoviranje(renoviranje);
        }

        public Servis.TerminServis terminServis;
        public Servis.ProstorijeServis prostorijeServis;

        public int GetBrojProstorija(VrstaProstorije vrstaProstorije)
        {
            return prostorijeServis.GetByVrstaProstorije(vrstaProstorije).Count;
        }

        public List<RenoviranjeDTO> GetAllRenoviranja()
        {
            List<RenoviranjeDTO> renoviranja = new List<RenoviranjeDTO>();
            foreach (Renoviranje renoviranje in prostorijeServis.GetAllRenoviranja())
            {
                renoviranja.Add(new RenoviranjeDTO()
                {
                    BrojProstorije = renoviranje.BrojProstorije,
                    IdProstorije = renoviranje.IdProstorije,
                    VrstaProstorije = renoviranje.VrstaProstorije,
                    Sprat = renoviranje.Sprat,
                    DatumPocetkaRenoviranja = renoviranje.DatumPocetkaRenoviranja,
                    DatumZavrsetkaRenoviranja = renoviranje.DatumZavrsetkaRenoviranja
                });
            }
            return renoviranja;
        }
        public List<NaprednoRenoviranjeDTO> GetAllNaprednaRenoviranja()
        {
            List<NaprednoRenoviranjeDTO> renoviranjaDTO = new List<NaprednoRenoviranjeDTO>();
            foreach (NaprednoRenoviranje renoviranje in prostorijeServis.GetAllNaprednaRenoviranja())
            {
                renoviranjaDTO.Add(new NaprednoRenoviranjeDTO()
                {
                    BrojGlavneProstorije = renoviranje.BrojGlavneProstorije,
                    BrojProstorije1 = renoviranje.BrojProstorije1,
                    BrojProstorije2 = renoviranje.BrojProstorije2,
                    DatumPocetkaRenoviranja = renoviranje.DatumPocetkaRenoviranja,
                    DatumZavrsetkaRenoviranja = renoviranje.DatumZavrsetkaRenoviranja,
                    Spajanje = renoviranje.Spajanje,
                    Podela = renoviranje.Podela
            });
            }
            return renoviranjaDTO;
        }

    }
}