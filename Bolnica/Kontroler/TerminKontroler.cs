using Model;
using Servis;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kontroler
{
    public class TerminKontroler
    {
        public TerminKontroler()
        {
            terminServis = new TerminServis();
        }

        public bool ZakaziTermin(Termin termin)
        {
            return terminServis.ZakaziTermin(termin);
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniTermin(Termin termin)
        {
            return terminServis.IzmeniTermin(termin);
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

        public IEnumerable GetBuduciTerminPacLekar()
        {
            return terminServis.GetBuduciTerminPacLekar();
        }

        public void RemoveByID(string iDTermina)
        {
            terminServis.RemoveByID(iDTermina);
        }
    }
}