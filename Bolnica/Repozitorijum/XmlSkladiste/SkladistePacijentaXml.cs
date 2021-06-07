using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladistePacijentaXml : ISkladistePacijenta
    {
        private SkladistePacijentaXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\pacijenti.xml";
        }

        public static SkladistePacijentaXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladistePacijentaXml();
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

        public Pacijent GetByJmbg(String jmbg)
        {
            List<Pacijent> pacijenti = this.GetAll();
            Pacijent pacijent1 = new Pacijent();
            foreach (Pacijent pacijent in pacijenti)
            {
                if (pacijent.Jmbg != null)
                {
                    if (pacijent.Jmbg.Equals(jmbg))
                    {
                        pacijent1 = pacijent;
                        break;
                    }
                }
            }
            return pacijent1;
        }

        public void Save(Pacijent pacijent)
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
        private static SkladistePacijentaXml instance = null;
    }
}