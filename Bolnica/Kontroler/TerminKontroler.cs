using Model;
using Servis;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kontroler
{
    public class TerminKontroler
    {
        public Servis.TerminServis terminServis;

        public static TerminKontroler instance = null;

        public static TerminKontroler getInstance()
        {
            if(instance == null)
            {
                return new TerminKontroler();
            }
            else
            {
                return instance;
            }
        }

        public TerminKontroler()
        {
            terminServis = new TerminServis();
            instance = this;
        }

        public bool ZakaziTermin(Termin termin)
        {
            return terminServis.ZakaziTermin(termin);
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return terminServis.OtkaziTermin(termin);
        }

        public bool IzmeniTermin(Termin termin)
        {
            return terminServis.IzmeniTermin(termin);
        }

        public List<Termin> DobaviMoguceTerminePoLekaru(int idLekara)
        {
            // TODO: implement
            return terminServis.DobaviMoguceTerminePoLekaru(idLekara);
        }

        public List<Termin> DobaviTerminZaInterval(DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return terminServis.DobaviTerminZaInterval(pocetak,kraj);
        }

        public List<Termin> DobaviTerminPoLekaruZaInterval(int idLekara, DateTime pocetak, DateTime kraj)
        {
            // TODO: implement
            return terminServis.DobaviTerminPoLekaruZaInterval(idLekara,pocetak,kraj);
        }

        public bool ProveriTermin(Model.Termin termin)
        {
            // TODO: implement
            return terminServis.ProveriTermin(termin);
        }

        public List<Termin> GetAll()
        {
            // TODO: implement
            return terminServis.GetAll();
        }

        public void Save(Termin termin)
        {
            // TODO: implement
            terminServis.Save(termin);
        }

        public void SaveAll(List<Termin> termini)
        {
            // TODO: implement
            terminServis.SaveAll(termini);
        }

        public List<Termin> NadjiTermineZaParametre(String jmbgLekara, String jmbgPacijenta, List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, int prioritet, String tegobe)
        {
            return TerminServis.getInstance().NadjiTermineZaParametre(jmbgLekara, jmbgPacijenta, dani, pocetak, kraj, prioritet, tegobe);
        }



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