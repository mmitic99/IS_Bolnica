using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaRenoviranjaXml : ISkladisteZaRenoviranja
    {
        public String Lokacija { get; set; }
        private static SkladisteZaRenoviranjaXml instance = null;

        public static SkladisteZaRenoviranjaXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaRenoviranjaXml();
            }
            return instance;
        }


        public List<Renoviranje> Renoviranja
        {
            get;
            set;
        }

        public SkladisteZaRenoviranjaXml()
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
