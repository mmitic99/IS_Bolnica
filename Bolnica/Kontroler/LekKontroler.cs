using Bolnica.DTOs;
using Bolnica.Servis;
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

        public void DodajLek(Lek lek)
        {
            LekServis.GetInstance().DodajLek(lek);
        }

        public void IzmeniLek(int index, Lek lek)
        {
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

        public bool ProveriValidnostLeka(LekDTO lek, String dodajIzmeni)
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
    }
}
