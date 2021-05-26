using Bolnica.DTOs;
using Bolnica.Servis;
using Bolnica.view.UpravnikView;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class LekKontroler
    {
        private static LekKontroler instance = null;
        public static LekKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new LekKontroler();
            }
            return instance;
        }

        public void DodajLek(LekDTO lekDTO)
        {
            Lek lek = new Lek(lekDTO.VrstaLeka, lekDTO.KolicinaLeka, lekDTO.NazivLeka, lekDTO.KlasaLeka, lekDTO.JacinaLeka, lekDTO.ZamenskiLek, lekDTO.SastavLeka);
            LekServis.GetInstance().DodajLek(lek);
        }

        public void IzmeniLek(int index, LekDTO lekDTO)
        {
            Lek lek = new Lek(lekDTO.VrstaLeka, lekDTO.KolicinaLeka, lekDTO.NazivLeka, lekDTO.KlasaLeka, lekDTO.JacinaLeka, lekDTO.ZamenskiLek, lekDTO.SastavLeka);
            LekServis.GetInstance().IzmeniLek(index, lek);
        }
        public void IzmeniLekLekar(int index, LekDTO lekDTO)
        {
            Lek lek = new Lek(lekDTO.VrstaLeka, lekDTO.KolicinaLeka, lekDTO.NazivLeka, lekDTO.KlasaLeka, lekDTO.JacinaLeka, lekDTO.ZamenskiLek, lekDTO.SastavLeka);
            LekServis.GetInstance().IzmeniLekLekar(index, lek);
        }

        public void IzbrisiLek(int index)
        {
            LekServis.GetInstance().IzbrisiLek(index);
        }

        public bool ProveriValidnostLeka(LekValidacijaDTO lek, String dodajIzmeni, int selektovaniLek)
        {
           return LekServis.GetInstance().ProveriValidnostLeka(lek, dodajIzmeni, selektovaniLek);
        }

        public Model.Enum.VrstaLeka GetVrstuLeka(int IndexSelektovaneVrsteLeka)
        {
            return LekServis.GetInstance().GetVrstuLeka(IndexSelektovaneVrsteLeka);
        }


        public Model.Enum.KlasaLeka GetKlasuLeka(int IndexSelektovaneKlaseLeka)
        {
            return LekServis.GetInstance().GetKlasuLeka(IndexSelektovaneKlaseLeka);
        }

        public List<LekDTO> GetAll()
        {
            List<LekDTO> lekovi = new List<LekDTO>();
            foreach (Lek lek in LekServis.GetInstance().GetAll())
            {
                lekovi.Add(new LekDTO()
                {
                    VrstaLeka = lek.VrstaLeka,
                    KolicinaLeka = lek.KolicinaLeka,
                    NazivLeka = lek.NazivLeka,
                    KlasaLeka = lek.KlasaLeka,
                    JacinaLeka = lek.JacinaLeka,
                    ZamenskiLek = lek.ZamenskiLek,
                    SastavLeka = lek.SastavLeka
                });
            }
            return lekovi;
        }

        public void Save(Lek lek)
        {
            LekServis.GetInstance().Save(lek);
        }

        public void SaveAll(List<Lek> lekovi)
        {
            LekServis.GetInstance().SaveAll(lekovi);
        }
        public Lek getById(int id)
        {
            return LekServis.GetInstance().getById(id);
        }
    }
}
