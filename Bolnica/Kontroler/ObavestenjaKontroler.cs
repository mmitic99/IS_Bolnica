using Model;
using Servis;
using System.Collections.Generic;

namespace Kontroler
{
    public class ObavestenjaKontroler
    {
        public ObavestenjaKontroler()
        {
            obavestenjaServis = new ObavestenjaServis();
        }

        public List<Obavestenje> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Obavestenje obavestenje)
        {
            // TODO: implement
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            // TODO: implement
        }

        public Servis.ObavestenjaServis obavestenjaServis;

    }
}