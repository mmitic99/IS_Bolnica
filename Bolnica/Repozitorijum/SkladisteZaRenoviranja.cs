using Bolnica.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bolnica.Repozitorijum
{
    public class SkladisteZaRenoviranja
    {
        public String Lokacija { get; set; }
        private static SkladisteZaRenoviranja instance = null;

        public static SkladisteZaRenoviranja GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaRenoviranja();
            }
            return instance;
        }


        public List<Renoviranje> Renoviranja
        {
            get;
            set;
        }

        public SkladisteZaRenoviranja()
        {
            Renoviranja = new List<Renoviranje>();
            Lokacija = "..\\..\\SkladistePodataka\\renoviranja.xml";
        }

        public List<Renoviranje> GetAll()
        {
            Renoviranja = new List<Renoviranje>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Renoviranje>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                Renoviranja = (List<Renoviranje>)serializer.Deserialize(reader);
                reader.Close();
            }
            return Renoviranja;
        }

        public void Save(Renoviranje renoviranje)
        {
            Renoviranja = GetAll();

            Renoviranja.Add(renoviranje);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Renoviranje>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, Renoviranja);
            writer.Close();
        }

        public void SaveAll(List<Renoviranje> renoviranja)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Renoviranje>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, renoviranja);
            writer.Close();
        }
    }
}
