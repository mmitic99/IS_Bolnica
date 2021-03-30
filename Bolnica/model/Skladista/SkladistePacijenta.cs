using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model.Skladista
{
    public class SkladistePacijenta
    {
        private SkladistePacijenta()
        {
            Lokacija = "..\\..\\SkladistePodataka\\pacijenti.xml";
        }

        public static SkladistePacijenta GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladistePacijenta();
            }
            return instance;
        }

        public List<Pacijent> GetAll()
        {
            List<Pacijent> pacijenti = new List<Pacijent>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Pacijent>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                pacijenti = (List<Pacijent>)serializer.Deserialize(stream);
                stream.Close();
            }

            return pacijenti;
        }

        public Pacijent getByJmbg(String jmbg)
        {
            List<Pacijent> pacijenti = this.GetAll();
            Pacijent p1 = new Pacijent();
            foreach (Pacijent p in pacijenti)
            {
                if (p.Jmbg != null)
                {
                    if (p.Jmbg.Equals(jmbg))
                    {
                        p1 = p;
                        break;
                    }
                }
            }
            return p1;
        }

        public void Save(Model.Pacijent pacijent)
        {
            List<Pacijent> pacijenti = GetAll();
            pacijenti.Add(pacijent);

            SaveAll(pacijenti);
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pacijent>));
                serializer.Serialize(stream, pacijenti);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;
        private static SkladistePacijenta instance = null;
    }
}