using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaNaprednaRenoviranjaXml : ISkladisteZaNaprednaRenoviranja
    {
        public String Lokacija { get; set; }
        private static SkladisteZaNaprednaRenoviranjaXml instance = null;

        public static SkladisteZaNaprednaRenoviranjaXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaNaprednaRenoviranjaXml();
            }
            return instance;
        }


        public List<NaprednoRenoviranje> NaprednaRenoviranja
        {
            get;
            set;
        }

        public SkladisteZaNaprednaRenoviranjaXml()
        {
            NaprednaRenoviranja = new List<NaprednoRenoviranje>();
            Lokacija = "..\\..\\SkladistePodataka\\napredna renoviranja.xml";
        }

        public List<NaprednoRenoviranje> GetAll()
        {
            NaprednaRenoviranja = new List<NaprednoRenoviranje>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<NaprednoRenoviranje>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                NaprednaRenoviranja = (List<NaprednoRenoviranje>)serializer.Deserialize(reader);
                reader.Close();
            }
            return NaprednaRenoviranja;
        }

        public void Save(NaprednoRenoviranje renoviranje)
        {
            NaprednaRenoviranja = GetAll();

            NaprednaRenoviranja.Add(renoviranje);

            XmlSerializer serializer = new XmlSerializer(typeof(List<NaprednoRenoviranje>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, NaprednaRenoviranja);
            writer.Close();
        }

        public void SaveAll(List<NaprednoRenoviranje> renoviranja)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<NaprednoRenoviranje>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, renoviranja);
            writer.Close();
        }
    }
}
