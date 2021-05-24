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
        public void IzmeniLekLekar(int index, Lek lek)
        {
            LekServis.GetInstance().IzmeniLekLekar(index, lek);
        }

        public void IzbrisiLek(int index)
        {
            LekServis.GetInstance().IzbrisiLek(index);
        }

        public bool ProveriValidnostLeka(LekValidacijaDTO lek, String dodajIzmeni)
        {
           return LekServis.GetInstance().ProveriValidnostLeka(lek, dodajIzmeni);
        }

        public Model.Enum.VrstaLeka GetVrstuLeka(int IndexSelektovaneVrsteLeka)
        {
            return LekServis.GetInstance().GetVrstuLeka(IndexSelektovaneVrsteLeka);
        }


        public Model.Enum.KlasaLeka GetKlasuLeka(int IndexSelektovaneKlaseLeka)
        {
            return LekServis.GetInstance().GetKlasuLeka(IndexSelektovaneKlaseLeka);
        }

        public List<Lek> GetAll()
        {
            return LekServis.GetInstance().GetAll();
        }

        public void NamapirajVrstuLeka(int index)
        {
            List<Lek> SviLekovi = LekServis.GetInstance().GetAll();
            if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Gel)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Gel";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Kapsula)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Kapsula";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Sirup)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Sirup";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Sprej)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Sprej";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.SumecaTableta)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Šumeća tableta";
            }
            else if (SviLekovi[index].VrstaLeka == Model.Enum.VrstaLeka.Tableta)
            {
                UpravnikWindow.GetInstance().VrstaLekaIzmeni.Text = "Tableta";
            }
        }

        public void NamapirajKlasuLeka(int index)
        {
            List <Lek> SviLekovi = LekServis.GetInstance().GetAll();
            if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Analgetik)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Analgetik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antibiotik)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Antibiotik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antimalarijski_Lek)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Antimalarijski lek";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antipiretik)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Antipiretik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Antiseptik)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Antiseptik";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Hormonska_Zamena)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Hormonska zamena";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Oralni_Kontraceptiv)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Oralni kontraceptiv";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Stabilizator_Raspolozenja)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Stabilizator raspoloženja";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Statin)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Statin";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Stimulant)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Stimulant";
            }
            else if (SviLekovi[index].KlasaLeka == Model.Enum.KlasaLeka.Trankvilajzer)
            {
                UpravnikWindow.GetInstance().KlasaLekaIzmeni.Text = "Trankvilajzer";
            }
        }

        public void NamapirajInfoOLeku(int index)
        {
            List<Lek> SviLekovi = LekServis.GetInstance().GetAll();
            UpravnikWindow.GetInstance().NazivLekaIzmeni.Text = SviLekovi[index].NazivLeka;
            UpravnikWindow.GetInstance().JacinaLekaIzmeni.Text = SviLekovi[index].JacinaLeka.ToString();
            UpravnikWindow.GetInstance().KolicinaLekaIzmeni.Text = SviLekovi[index].KolicinaLeka.ToString();
            UpravnikWindow.GetInstance().ZamenskiLekIzmeni.Text = SviLekovi[index].ZamenskiLek;
            UpravnikWindow.GetInstance().SastavLekaIzmeni.Text = SviLekovi[index].SastavLeka;
        }
    }
}
