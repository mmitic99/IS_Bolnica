using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace Model.Skladista
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


        public List<Termin> getByJmbg(String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.pacijent.Jmbg.Equals(jmbg))
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