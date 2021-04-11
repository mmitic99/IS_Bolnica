using Model;
using Repozitorijum;
using System.Collections.Generic;

namespace Servis
{
    public class ObavestenjaServis
    {
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