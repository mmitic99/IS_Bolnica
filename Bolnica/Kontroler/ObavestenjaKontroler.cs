using Model;
using Servis;
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

        public void Save(Obavestenje obavestenje)
        {
            obavestenjaServis.Save(obavestenje);
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

        public void napraviPodsetnik(string jmbgPacijenta, Recept r, int hours)
        {
            ObavestenjaServis.getInstance().napraviPodsetnik(jmbgPacijenta, r, hours);
        }

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            return ObavestenjaServis.getInstance().DobaviPodsetnikeZaTerapiju(jmbgPacijenta);
        }

       

        public Servis.ObavestenjaServis obavestenjaServis;

    }
}