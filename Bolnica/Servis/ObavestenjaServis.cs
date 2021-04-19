using Bolnica.view;
using Model;
using Repozitorijum;
using System;
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

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            Pacijent p = SkladistePacijenta.GetInstance().getByJmbg(jmbgPacijenta);
            p.zdravstveniKarton.izvestaji = new List<Izvestaj>();
            Izvestaj i = new Izvestaj();
            i.recepti = new List<Recept>();
            List<TimeSpan> terminiUzimanja = new List<TimeSpan>();
           /* List
            Recept r = new Recept()
            {
                lek = new Lek()
                {
                    NazivLeka = "Brufen"
                };
                
            };
           */
            return null;
        }



        public Repozitorijum.SkladisteZaObavestenja skladisteZaObavestenja;

    }
}