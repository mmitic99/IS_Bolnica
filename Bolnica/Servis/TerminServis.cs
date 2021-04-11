using Model;
using Repozitorijum;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Servis
{
    public class TerminServis
    {
        public TerminServis()
        {
            skladisteZaObavestenja = SkladisteZaObavestenja.GetInstance();
            skladisteZaTermine = new SkladisteZaTermine();
        }

        public bool ZakaziTermin(Model.Termin termin)
        {
            skladisteZaTermine.Save(termin);

            return true;
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniTermin(Termin termin)
        {
            bool uspesno = true;

            skladisteZaTermine.RemoveByID(termin.IDTermina);
            termin.IDTermina = termin.generateRandId();

            Obavestenje obavestenjePacijent = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgPacijenta,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            skladisteZaObavestenja.Save(obavestenjePacijent);

            Obavestenje obavestenjeLekar = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgLekara,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + termin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            skladisteZaObavestenja.Save(obavestenjeLekar);

            skladisteZaTermine.Save(termin);

            return uspesno;
        }

        public void RemoveByID(string iDTermina)
        {
            skladisteZaTermine.RemoveByID(iDTermina);
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

        public SkladisteZaTermine skladisteZaTermine;
        public SkladisteZaObavestenja skladisteZaObavestenja;

        internal IEnumerable GetBuduciTerminPacLekar()
        {
            return skladisteZaTermine.GetBuduciTerminPacLekar();
        }
    }
}