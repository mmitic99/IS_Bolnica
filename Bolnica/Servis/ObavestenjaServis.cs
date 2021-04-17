using Model;
using Repozitorijum;
using System.Collections.Generic;

namespace Servis
{
    public class ObavestenjaServis
    {

        public static ObavestenjaServis instance;

        public static ObavestenjaServis getInstance()
        {
            if (instance == null)
            {
                return new ObavestenjaServis();
            }
            else
            {
                return instance;
            }
        }

        public ObavestenjaServis()
        {
            skladisteZaObavestenja = SkladisteZaObavestenja.GetInstance();
        }

        public List<Obavestenje> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Model.Obavestenje obavestenje)
        {
            // TODO: implement
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            // TODO: implement
        }

        public Repozitorijum.SkladisteZaObavestenja skladisteZaObavestenja;

    }
}