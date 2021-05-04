using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class ObavestenjaKontroler
    {
        public static ObavestenjaKontroler instance =null;

        public static ObavestenjaKontroler getInstance()
        {
            if(instance==null)
            {
                return new ObavestenjaKontroler();
            }
            else
            {
                return instance;
            }
        }
        public ObavestenjaKontroler()
        {
            instance = this;
            obavestenjaServis = new ObavestenjaServis();
        }



        public List<Obavestenje> GetAll()
        {
            // TODO: implement
            return obavestenjaServis.GetAll();
        }

        public bool Save(Obavestenje obavestenje)
        {
            return obavestenjaServis.Save(obavestenje);
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            obavestenjaServis.SaveAll(obavestenje);
        }

        public List<Obavestenje> GetByJmbg(string jmbg)
        {
            return obavestenjaServis.GetByJmbg(jmbg);
        }

        public List<Obavestenje> GetPodsetnici(string jmbg)
        {
            return ObavestenjaServis.getInstance().GetPodsetnici(jmbg);
        }

        public bool napraviPodsetnik(string jmbgPacijenta, Recept r, int hours)
        {
            return ObavestenjaServis.getInstance().napraviPodsetnik(jmbgPacijenta, r, hours);
        }

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            return ObavestenjaServis.getInstance().DobaviPodsetnikeZaTerapiju(jmbgPacijenta);
        }

        public bool IzmeniObavestenje(Obavestenje staroObavestenje, Obavestenje novoObavestenje)
        {
            return obavestenjaServis.IzmeniObavestenje(staroObavestenje, novoObavestenje);
        }

        public Servis.ObavestenjaServis obavestenjaServis;

        public bool obrisiObavestenje(Obavestenje obavestenje)
        {
            return obavestenjaServis.obrisiObavestenje(obavestenje);
        }

        internal void PosaljiKvartalnuAnketu()
        {
            ObavestenjaServis.getInstance().PosaljiKvartalnuAnketu();
        }

        internal void PosaljiAnketuOLekaru(string JmbgPacijenta, string JmbgLekara)
        {
            ObavestenjaServis.getInstance().PosaljiAnketuOLekaru(JmbgPacijenta, JmbgLekara);
        }
    }
}