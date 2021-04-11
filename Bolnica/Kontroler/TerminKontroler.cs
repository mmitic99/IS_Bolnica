using Model;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class TerminKontroler
    {
        public bool ZakaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public List<Termin> DobaviMoguceTerminePoLekaru(int idLekara)
        {
            // TODO: implement
            return null;
        }

        public List<Termin> DobaviTerminZaInterval(DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return null;
        }

        public List<Termin> DobaviTerminPoLekaruZaInterval(int idLekara, DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return null;
        }

        public bool ProveriTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public List<Termin> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Termin termin)
        {
            // TODO: implement
        }

        public void SaveAll(List<Termin> termini)
        {
            // TODO: implement
        }

        public Servis.TerminServis terminServis;

    }
}