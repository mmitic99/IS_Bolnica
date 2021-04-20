using Bolnica.viewActions;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repozitorijum
{
    public class SkladisteZaTermine
    {
        public String Lokacija { get; set; }
        private List<Termin> termini;

        private static SkladisteZaTermine instance = null;

        public static SkladisteZaTermine getInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaTermine();
            }
            return instance;
        }


        public SkladisteZaTermine()
        {
            this.termini = new List<Termin>();
            Lokacija = "..\\..\\SkladistePodataka\\termini.xml";
        }

        public List<Termin> GetAll()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));


            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);
                termini = (List<Termin>)serializer.Deserialize(reader);
                reader.Close();
            }
            return termini;
        }

        public List<TerminPacijentLekar> GetBuduciTerminPacLekar()
        {

            List<TerminPacijentLekar> termini = new List<TerminPacijentLekar>();
            foreach (Termin termin in SkladisteZaTermine.getInstance().GetAll())
            {
                if (termin.DatumIVremeTermina >= DateTime.Now)
                {
                    TerminPacijentLekar t = new TerminPacijentLekar { termin = termin, pacijent = SkladistePacijenta.GetInstance().getByJmbg(termin.JmbgPacijenta), lekar = SkladisteZaLekara.GetInstance().getByJmbg(termin.JmbgLekara) };
                    termini.Add(t);
                }
            }

            return termini;
        }


        public List<Termin> getByJmbg(String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.JmbgPacijenta.Equals(jmbg))
                {
                    odgovTermini.Add(t);
                }
            }
            return odgovTermini;
        }
        public List<Termin> getByDateForLekar(DateTime datum,String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.JmbgLekara.Equals(jmbg) & t.DatumIVremeTermina.Date.Equals(datum.Date))
                {
                    odgovTermini.Add(t);
                }
            }
            return odgovTermini;
        }
        public List<Termin> getByJmbgLekar(String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.JmbgLekara.Equals(jmbg))
                {
                    odgovTermini.Add(t);
                }
            }
            return odgovTermini;
        }

        public Termin getById(String id)
        {
            Termin ter = new Termin();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.IDTermina.Equals(id))
                {
                    ter = t;
                    break;
                }
            }
            return ter;
        }

        public void RemoveByID(String id)
        {
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.IDTermina.Equals(id))
                {
                    sviTermini.Remove(t);
                    break;
                }
            }
            this.SaveAll(sviTermini);
        }


        public void Save(Model.Termin termin)
        {
            termini = GetAll();

            termini.Add(termin);

            SaveAll(termini);
        }

        public void SaveAll(List<Termin> termini)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
                serializer.Serialize(stream, termini);
            }
            finally
            {
                stream.Close();
            }
        }

    }


}